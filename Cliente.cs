namespace CadAPI
{
    public class Cliente
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public StatusCliente Status { get; set; }

        private readonly List<HistoricoCliente> _historicoClientes = new();
        public IReadOnlyCollection<HistoricoCliente> HistoricoClientes => _historicoClientes;

        protected Cliente()
        {
            
        }

        private Cliente(Guid id, string nome, string cpf, string email, DateTime dataNascimento, StatusCliente status)
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