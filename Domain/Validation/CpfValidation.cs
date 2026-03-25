namespace CadAPI.Domain.Entities.Validation;

public class CpfValidation
{
    public bool ValidarCpf(string cpf)
    {
        // Formatação e validação de CPF
        cpf = cpf.Replace(".", "").Replace("-", "").Replace(" ", "");
        
        if (cpf.Length != 11)
            return false;

        bool todosIguais = true;
        for (int i = 1; i < 11; i++)
        {
            if (cpf[i] != cpf[0])
            {
                todosIguais = false;
                break;
            }
        }

        if (todosIguais)
            return false;

        int soma = 0;
        for (int i = 0; i < 9; i++)
        {
            soma += int.Parse(cpf[i].ToString()) * (10 - i);
        }
        
        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        for (int i = 0; i < 10; i++)
        {
            soma += int.Parse(cpf[i].ToString()) * (11 - i);
        }
        
        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        // comparação final dos dígitos calculados com os dígitos reais
        return digito1 == int.Parse(cpf[9].ToString()) &&
               digito2 == int.Parse(cpf[10].ToString());
    }
}