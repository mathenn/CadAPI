namespace CadAPI.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Cpf { get; private set; }
        public string Email { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public StatusCliente Status { get; private set; }

        private readonly List<HistoricoCliente> _historicoClientes = new();
        public IReadOnlyCollection<HistoricoCliente> HistoricoClientes => _historicoClientes;

        protected Cliente()
        {
            
        }

        private Cliente(Guid id, string nome, string cpf, string email, 
                        DateTime dataNascimento, StatusCliente status)
        {
            Id = id;
            Nome = nome;
            Cpf = cpf;
            Email = email;
            DataNascimento = dataNascimento;
            Status = status;
        }

        public enum StatusCliente
        {
            Ativo,
            Inativo,
            Bloqueado
        }
    }
}