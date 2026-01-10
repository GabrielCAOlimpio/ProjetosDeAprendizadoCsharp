public class Account
{
    private int id;
    private string name = string.Empty;
    private decimal balance;

    public int Id
    {
        get => id;
        set => id = value > 0 ? value : throw new ArgumentException("ID inválido");
    }

    public string Name
    {
        get => name;
        set => name = !string.IsNullOrEmpty(value) ? value : throw new ArgumentException("Nome inválido");
    }

    public decimal Balance
    {
        get => balance;
        set => balance = value >= 0 ? value : throw new ArgumentException("Saldo não pode ser negativo");
    }

    public Account(int id, string name, decimal balance)
    {
        Id = id;
        Name = name;
        Balance = balance;
    }

    public void Depositar(decimal value) => Balance += value;
    
    public void Sacar(decimal value)
    {
        if (value > Balance)
            throw new InvalidOperationException("Saldo insuficiente.");
        Balance -= value;
    }
}