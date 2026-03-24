using CadAPI.Application.DTOs;
using CadAPI.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

[ApiController] // indica que é o controller da API, habilitando comportamentos automáticos
[Route("api/clientes")] // define rota base para todos os endpoints deste controller
public class ClientesController : ControllerBase // classe base otimizada para APIs
{
    private readonly AppDbContext _context;
    private readonly ILogger<ClientesController> _logger;

    public ClientesController(AppDbContext context, ILogger<ClientesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar([FromBody] CriarClienteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            // validações de negócio básicas (verificando duplicatas)
            if (await _context.Clientes.AnyAsync(c => c.Cpf == dto.Cpf))
                return Conflict(new { mensagem = "CPF já cadastrado" });
            if (await _context.Clientes.AnyAsync(c => c.Email == dto.Email))
                return Conflict(new { mensagem = "Email já cadastrado" });

            if (!ValidarCpf(dto.Cpf))
                return BadRequest(new { mensagem = "CPF inválido." });

            var cliente = Cliente.Criar(
                dto.Nome, dto.Cpf, dto.Email, dto.DataNascimento);

            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Cliente {Id} criado com sucesso", cliente.Id);m 
            return CreatedAtAction(nameof(ObterPorId), new { id = cliente.Id }, cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar cliente");
            return StatusCode(500, new { mensagem = "Erro interno ao processar requisição." });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanho = 10)
    {
        var query = _context.Clientes.AsNoTracking();
        
        var total = await query.CountAsync();
        var clientes = await query  
            .Skip((pagina - 1) * tamanho)
            .Take(tamanho)
            .ToListAsync();

        return Ok(new {total, pagina, tamanho, dados = clientes });
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        
        if (cliente == null)
            return NotFound(new { mensagem = "Cliente não encontrado" });
        
        return Ok(cliente);
    }
    
}

