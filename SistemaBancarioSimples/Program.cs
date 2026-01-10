using System;

namespace Program;

public class Program
{
    public static void Main(string[] args)
    {
        List<Account> accounts = new List<Account>()
        {
            new Account(1, "Gabriel", 10000),
            new Account(2, "Ana Costa", 1500),
            new Account(3, "Lucas Silva", 2750),
            new Account(4, "Beatriz Souza", 5000),
            new Account(5, "Ricardo Oliveira", 1200),
            new Account(6, "Fernanda Lima", 8900),
            new Account(7, "Marcos Rocha", 430),
            new Account(8, "Juliana Alves", 3200),
            new Account(9, "Bruno Mendes", 6150),
            new Account(10, "Camila Pires", 10500)
        };

        while (true)
        {
            Console.WriteLine("=======================================================================");
            Console.WriteLine($"--------------- BEM VINDO AO NOSSO BANCO SIMPLES ---------------");
            Console.WriteLine("=======================================================================");
            Console.Write("DIGITE O ID DA CONTA: ");
            string? response = Console.ReadLine();
            bool converterResult = int.TryParse(response, out int id);

            if (string.IsNullOrEmpty(response) || !converterResult)
            {
                while (string.IsNullOrEmpty(response) || !converterResult)
                {
                    Console.Write($"ID INVALIDO! POR FAVOR DIGITE UM ID VALIDO: ");
                    response = Console.ReadLine();
                    Console.WriteLine();

                    converterResult = int.TryParse(response, out id);    
                }
            }
            Account? specificAccount = accounts.FirstOrDefault(u => u.Id == id);

            if (specificAccount == null)
            {
                Console.WriteLine($"NÃO ENCONTRAMOS NENHUM USUARIO COM ESSE ID!");
                break;
            }
            else
            {
                Console.WriteLine($"---------- PAINEL DE CONTROLE ----------");
                Console.WriteLine($"O QUE VOCÊ DESEJA FAZER HOJE: ");
                Console.WriteLine($"[1] DEPOSITAR ");
                Console.WriteLine($"[2] SACAR ");
                Console.WriteLine($"[3] VER INFORMAÇÕES DA CONTA ");
                Console.WriteLine($"[4] SAIR ");
                Console.Write($"QUAL É A SUA OPC: ");
                response = Console.ReadLine();

                converterResult = int.TryParse(response, out int opt);
                if (!converterResult || (opt > 4 || opt < 1))
                {
                    while (!converterResult || (opt > 4 || opt < 1))
                    {
                        Console.WriteLine("===========================================================");
                        Console.WriteLine($"ERRO! RESPOSTA INVALIDA! DIGITE UMA OPC VALIDA: ");
                        Console.Write($"QUAL É A SUA OPC: ");
                        response = Console.ReadLine();
                        converterResult = int.TryParse(response, out opt);
                    }
                }
                switch (opt)
                {
                    case 1:
                        Console.Write("DIGITE O VALOR A DEPOSITAR: ");
                        response = Console.ReadLine();
                        converterResult = decimal.TryParse(response, out decimal depositAmount);
                        if (converterResult && depositAmount > 0)
                        {
                            specificAccount.Depositar(depositAmount);
                            Console.WriteLine($"DEPÓSITO REALIZADO COM SUCESSO!");
                        }
                        else
                        {
                            while (!converterResult && depositAmount <= 0)
                            {
                                Console.WriteLine("VALOR INVÁLIDO! NÃO FOI POSSIVEL EXECUTAR OPERAÇÃO");
                                Console.WriteLine($"DIGITE UM VALOR DE DEPOSITO VALIDO: ");
                                response = Console.ReadLine();
                                converterResult = decimal.TryParse(response, out depositAmount);
                            }
                            specificAccount.Depositar(depositAmount);
                            Console.WriteLine($"DEPÓSITO REALIZADO COM SUCESSO!");  
                        }
                        break;
                    
                    case 2:
                        Console.Write("DIGITE O VALOR A SACAR: ");
                        response = Console.ReadLine();
                        converterResult = decimal.TryParse(response, out decimal withdrawAmount);
                        if (converterResult && withdrawAmount > 0)
                        {
                            try
                            {
                                specificAccount.Sacar(withdrawAmount);
                                Console.WriteLine($"SAQUE FEITO COM SUCESSO!");
                            }
                            catch (ArgumentOutOfRangeException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine($"OCORREU UM ERRO DESCONHECIDO! TENTE NOVAMENTE MAIS TARDE!");
                            }
                        }
                        else
                        {
                            while (!converterResult || withdrawAmount <= 0)
                            {
                                Console.WriteLine("VALOR INVÁLIDO! DIGITE UM VALOR DE SAQUE VÁLIDO: ");
                                response = Console.ReadLine();
                                converterResult = decimal.TryParse(response, out withdrawAmount);
                            }
                            try
                            {
                                specificAccount.Sacar(withdrawAmount);
                                Console.WriteLine($"SAQUE FEITO COM SUCESSO!");
                            }
                            catch (ArgumentOutOfRangeException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine($"OCORREU UM ERRO DESCONHECIDO! TENTE NOVAMENTE MAIS TARDE!");
                            }
                        }
                        break;
                    case 3:
                        Console.WriteLine("===========================================================");
                        Console.WriteLine($"ID: {specificAccount.Id}");
                        Console.WriteLine($"NOME: {specificAccount.Name}");
                        Console.WriteLine($"SALDO: R$ {specificAccount.Balance:F2}");
                        Console.WriteLine("===========================================================");
                        break;
                    
                    case 4:
                        return;
                }
                
            }
            
        }
        Console.WriteLine($"OBRIGADO POR EXPERIMENTAR NOSSO CÓDIGO!");
        Console.WriteLine($"TENHA UM ÓTIMO DIA 😁");
    }
}