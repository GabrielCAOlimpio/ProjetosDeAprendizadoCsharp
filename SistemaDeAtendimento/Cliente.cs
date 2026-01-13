namespace SistemaDeAtendimentos;

public class Cliente
{
    private int idCliente;
    private string nome = string.Empty;
    private string email = string.Empty;

    public int IdCliente
    {
        get => idCliente;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException("O ID do cliente deve ser um número positivo.");
            }
            idCliente = value;
        }
    }
    public string Nome
    {
        get => nome;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("O nome do cliente não pode ser vazio.");
            }
            nome = value;
        }
    }
    public string Email
    {
        get => email;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
            {
                throw new ArgumentException("O email do cliente é inválido.");
            }
            email = value;
        }
    }

    public Cliente(int idCliente, string nome, string email)
    {
        IdCliente = idCliente;
        Nome = nome;
        Email = email;
    }

}