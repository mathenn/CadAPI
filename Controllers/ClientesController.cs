using CadAPI.Application.DTOs;
using CadAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

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
    public async task<IActionResult> Criar([FromBody] CriarClienteDto dto)
    {
        try
        {
            // validações de negócio básicas
            if (await _context.Clientes.AnyAsync(c => c.Cpf == dto.Cpf))
                return Conflict(new { mensagem = "CPF já cadastrado" });
            if (await _context.Clientes.AnyAsync(c => c.Email == dto.Email))
                return Conflict(new { mensagem = "Email já cadastrado" });

            var cliente = Cliente.Criar(
                dto.Nome, dto.Cpf, dto.Email, dto.DataNascimento);

            await _context.Clientes.AddAsync(cliente);
            await _context.Clientes.SaveChangesAsync();

            _logger.LogInformation("Cliente {Id} criado com sucesso", cliente, Id);
            return CreatedAtAction(nameof(ObterPorId), new { Id = cliente.Id }, cliente);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Cliente não encontrado" });
        }
    }
    
}

