using System;
namespace Program;

public interface IPessoa
{
    string Nome { get; set; }
    int Idade { get; set; }
}
public interface IVerificarEConverter
{
    string VerificarTexto(string? resposta);
    int VerficarNumero(string? resposta);
    
}