using System;
using CadAPI.Domain.Validations;

namespace CadAPI.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Cpf { get; private set; }
        public string Email { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public StatusCliente Status { get; private set; }

        private readonly List<HistoricoCliente> _historicoClientes = new();
        public IReadOnlyCollection<HistoricoCliente> HistoricoClientes => _historicoClientes;

        protected Cliente()
        {

        }

        private Cliente(Guid id, string nome, string cpf, string email,
            DateTime dataNascimento, StatusCliente status)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Cpf = CpfValidation.RemoverFormatacao(cpf);
            Email = email;
            DataNascimento = dataNascimento;
            DataCadastro = DateTime.UtcNow;
            Status = status;    
        }

        public enum StatusCliente
        {
            Ativo,
            Inativo,
            Bloqueado
        }

        // Factory method para criar um novo cliente
        public static Cliente Criar(string nome, string cpf, string email, DateTime dataNascimento)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório");

            if (string.IsNullOrWhiteSpace(cpf))
                throw new ArgumentException("CPF é obrigatório");

            if (!CpfValidation.ValidarCpf(cpf))
                throw new ArgumentException("CPF inválido");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email é obrigatório");

            if (!email.Contains("@"))
                throw new ArgumentException("Email inválido");

            if (dataNascimento == default)
                throw new ArgumentException("Data de nascimento é obrigatória");
            
            var hoje = DateTime.Now;
            var idade = hoje.Year - dataNascimento.Year;

            var aniversarioEsteAno = dataNascimento.AddYears(idade);

            if (hoje < aniversarioEsteAno)
            {
                idade--;
            }
            
            if (idade < 18)
                throw new ArgumentException("Cliente deve ser maior de 18 anos.");
            if (idade > 120) 
                throw new ArgumentException("Data de nascimento inválida");

            return new Cliente(nome, cpf, email, dataNascimento);
        }

        public void AtualizarDados(string nome, string email)
        {
            if (!string.IsNullOrWhiteSpace(nome))
                Nome = nome;
            if (!string.IsNullOrWhiteSpace(email) && email.Contains("@"))
                Email = email;
        }

        public string cpfFormatado => CpfValidation.Formatar(cpf);
    }
}