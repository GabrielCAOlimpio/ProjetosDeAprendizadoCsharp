using System;
namespace Program;

public class Pessoas : IPessoa
{
    private string nome = string.Empty;
    private int idade;

    public string Nome
    {
        get => string.IsNullOrEmpty(nome) ? "Nome não informado" : nome;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("O nome não pode ser vazio.");
            }
            nome = value;
        }
    }
    public int Idade
    {
        get => idade < 0 ? 0 : idade;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("A idade não pode ser negativa.");
            }
            idade = value;
        }
    }
    
    public Pessoas(string nome, int idade)
    {
        Nome = nome;
        Idade = idade;
    }   
}

public class VerificarEConverter : IVerificarEConverter
{
    public string VerificarTexto(string? resposta)
    {
        if (string.IsNullOrWhiteSpace(resposta))
        {
            throw new ArgumentException("O texto não pode ser vazio.");
        }
        return resposta;
    }
    public int VerficarNumero(string? resposta)
    {
        if (!int.TryParse(resposta, out int numero) || numero < 0)
        {
            throw new ArgumentException("O número deve ser um inteiro não negativo.");
        }
        return numero;
    }
}