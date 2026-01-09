using System;

namespace Program;


public class Program
{
    public static void Main(string[] args)
    {
        IVerificarEConverter verificarEConverter = new VerificarEConverter();
        List<IPessoa> pessoas = new List<IPessoa>();
        try
        {
            while (true)
            {
                Console.WriteLine($"============= Cadastro de Pessoas =============");
                Console.Write("Digite o nome da pessoa: ");
                string nomeInput = verificarEConverter.VerificarTexto(Console.ReadLine());
                Console.Write("Digite a idade da pessoa: ");
                int idadeInput = verificarEConverter.VerficarNumero(Console.ReadLine());

                IPessoa pessoa = new Pessoas(nomeInput, idadeInput);
                pessoas.Add(pessoa);

                Console.WriteLine($"PESSOAS CADASTRADAS:");
                Console.WriteLine($"{"Nome",-20} {"Idade",-5}"); // Header
                Console.WriteLine(new string('-', 25)); // Separator
                foreach (var p in pessoas)
                {
                    Console.WriteLine($"{p.Nome,-20} {p.Idade,-5}");
                }

                Console.Write("Deseja cadastrar outra pessoa? (s/n): ");
                string continuar = verificarEConverter.VerificarTexto(Console.ReadLine()).ToLower();

                if (continuar[0] != 's' && continuar[0] != 'n')
                {
                    while (true)
                    {
                        Console.Write("Entrada inválida. Deseja cadastrar outra pessoa? (s/n): ");
                        continuar = verificarEConverter.VerificarTexto(Console.ReadLine()).ToLower();
                        if (continuar[0] == 's' || continuar[0] == 'n')
                        {
                            break;
                        }
                    }
                }
                if (continuar[0] == 'n')
                {
                    break;
                }
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
        }
        finally
        {
            Console.WriteLine($"OBRIGADO POR USAR O SISTEMA DE CADASTRO DE PESSOAS!");
            Console.WriteLine("Programa encerrado.");
        }
    }
}