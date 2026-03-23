using CadAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiController] // indica que é o controller da API, habilitando comportamentos automáticos
[Route("api/clientes")] // define rota base para todos os endpoints deste controller
public class ClientesController : ControllerBase // classe base otimizada para APIs
{
    private readonly AppDbContext _context;

    public ClientesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Criar(CriarClienteDto dto)
    {
        var cliente = Cliente.Criar(
            dto.Nome, dto.Cpf,
            dto.Email, dto.DataNascimento);
        
        _context.Cliente.Add(cliente);
        _context.SaveChanges();
        return Ok(cliente);

    }
    
}

