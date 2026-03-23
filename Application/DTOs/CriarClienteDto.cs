using System.ComponentModel.DataAnnotations;

namespace CadAPI.Application.DTOs
{
    public class CriarClienteDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; }
        [Required]
        [CpfValidation] // validação simples
        public string Cpf { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        
    }
}