using System;
using System.Collections.Generic;

namespace SistemaDeAtendimentos;

public class Program
{
    public static void Main(string[] args)
    {
        //Default clients for testing
        List<Cliente> clientes =
        [
            new Cliente(1, "Maria Silva","mariaSilva@gmail.com"),
            new Cliente(2, "João Santos","joaoSantos@gmail.com"),
            new Cliente(3, "Ana Oliveira","anaOliveira@gmail.com"),
            new Cliente(4, "Carlos Pereira","carlosPereira@gmail.com"),
            new Cliente(5, "Beatriz Costa","beatrizCosta@gmail.com"),
            new Cliente(6, "Felipe Alves","felipeAlves@gmail.com"),
            new Cliente(7, "Gabriela Sousa","gabrielaSousa@gmail.com"),
            new Cliente(8, "Lucas Martins","lucasMartins@gmail.com"),
            new Cliente(9, "Fernanda Rocha","fernandaRocha@gmail.com"),
            new Cliente(10, "Rafael Lima","rafaelLima@gmail.com")
        ];
        Queue<Cliente> filaAtendimentos = new Queue<Cliente>();
        Stack<Cliente> historicoAtendimentos = new Stack<Cliente>();
        Cliente[] ultimosAtendimentos = new Cliente[5];

        while (true)
        {
            Console.WriteLine($"Sistema de Atendimentos - Menu Principal");
            Console.WriteLine("1. Listar Clientes");
            Console.WriteLine("2. Buscar Cliente por ID");
            Console.WriteLine("3. Buscar por Emails Cadastrados");
            Console.WriteLine("4. Ver Fila de Atendimentos");
            Console.WriteLine("5. Historico de Atendimentos");
            Console.WriteLine("6. Ver ultimos 5 atendimentos");
            Console.WriteLine("0. Sair");
            Console.Write("Escolha uma opção: ");
            string opcao = Console.ReadLine() ?? string.Empty;
            bool converter = int.TryParse(opcao, out int opcaoInt);

            if (string.IsNullOrWhiteSpace(opcao) || !converter)
            {
                while (string.IsNullOrEmpty(opcao) || !converter)
                {
                    Console.WriteLine($"Opção inválida. Tente novamente.");
                    Console.Write("Escolha uma opção válida: ");
                    opcao = Console.ReadLine() ?? string.Empty;
                    converter = int.TryParse(opcao, out opcaoInt);
                }
            }

            Console.WriteLine();
            switch (opcaoInt)
            {
                case 0:
                    Console.WriteLine($"Obrigado por usar o sistema.");
                    Console.WriteLine("Saindo do sistema...");
                    Console.WriteLine("Até logo!");
                    return;
                case 1:
                    ListarClientes(clientes);
                    break;
                case 2:
                    Console.WriteLine("Buscando Cliente por ID...");
                    BuscarClientesPorId(clientes);
                    break;
                case 3:
                    Console.WriteLine("Buscando por Emails Cadastrados...");
                    BuscarClientePorEmail(clientes);
                    break;  
                case 4:
                    Console.WriteLine("Ver Fila de Atendimentos...");
                    FilaDeAtendimentos(clientes, ref filaAtendimentos, ref historicoAtendimentos);
                    break;
                case 5:
                    HistoricoDeAtendimentos(historicoAtendimentos);
                    break;
                case 6:
                    Console.WriteLine("Ver ultimos 5 atendimentos...");
                    UltimosCincoAtendimentos(historicoAtendimentos, ref ultimosAtendimentos);
                    break;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
            Console.WriteLine();
            Console.WriteLine("Pressione Enter para continuar...");
            Console.ReadLine();
            Console.Clear();
        }


    }
    public static void ListarClientes(List<Cliente> clientes)
    {
        Console.WriteLine("Lista de Clientes:");
        foreach (var cliente in clientes)
        {
            Console.WriteLine($"ID: {cliente.IdCliente}, Nome: {cliente.Nome}, Email: {cliente.Email}");
        }
    }
    public static void BuscarClientesPorId(List<Cliente> clientes)
    {
        Console.Write("Digite o ID do cliente que deseja buscar: ");
        string input = Console.ReadLine() ?? string.Empty;
        bool converter = int.TryParse(input, out int idCliente);

        if (string.IsNullOrWhiteSpace(input) || !converter)
        {
            while (string.IsNullOrEmpty(input) || !converter)
            {
                Console.WriteLine($"ID inválido. Tente novamente.");
                Console.Write("Digite um ID válido: ");
                input = Console.ReadLine() ?? string.Empty;
                converter = int.TryParse(input, out idCliente);
            }
        }

        Dictionary<int, Cliente> clientePorId = new Dictionary<int, Cliente>();
        foreach (var cliente in clientes)
        {
            clientePorId[cliente.IdCliente] = cliente;
        }

        if (clientePorId.ContainsKey(idCliente))
        {
            Console.WriteLine($"==============================================");
            Console.WriteLine($"CLIENTE ENCONTRADO!");
            Console.WriteLine($"Id: {clientePorId[idCliente].IdCliente}");
            Console.WriteLine($"Nome: {clientePorId[idCliente].Nome}");
            Console.WriteLine($"Email: {clientePorId[idCliente].Email}");
            Console.WriteLine($"==============================================");
        }
        else
        {
            Console.WriteLine($"Cliente com ID {idCliente} não encontrado.");
        }
    }
    public static void BuscarClientePorEmail(List<Cliente> clientes)
    {
        HashSet<string> emailsCadastrados = [];
        foreach (var cliente in clientes)
        {
            emailsCadastrados.Add(cliente.Email);
        }

        Console.Write($"DIGITE O EMAIL QUE DESEJA BUSCAR:");
        string emailBusca = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(emailBusca))
        {
            while (string.IsNullOrEmpty(emailBusca))
            {
                Console.WriteLine($"Email inválido. Tente novamente.");
                Console.Write("Digite um email válido: ");
                emailBusca = Console.ReadLine() ?? string.Empty;
            }
        }
        if (emailsCadastrados.Contains(emailBusca))
        {
            Console.WriteLine($"=================================================");
            Console.WriteLine($"EMAIL ENCONTRADO NOS CADASTROS!");
            Console.WriteLine($"Email: {emailBusca}");
            foreach (var cliente in clientes)
            {
                if (cliente.Email == emailBusca)
                {
                    Console.WriteLine($"Id: {cliente.IdCliente}");
                    Console.WriteLine($"Nome: {cliente.Nome}");
                }
            }
            Console.WriteLine($"=================================================");
        }
        else
        {
            Console.WriteLine($"Email {emailBusca} não encontrado nos cadastros.");
        }
    }
    public static void FilaDeAtendimentos(List<Cliente> clientes, ref Queue<Cliente> filaAtendimentos, ref Stack<Cliente> historicoAtendimentos)
    {
        if (filaAtendimentos.Count == 0)
        {
            Console.WriteLine("A fila de atendimentos está vazia. Adicionando clientes à fila...");
            foreach (var cliente in clientes)
            {
                filaAtendimentos.Enqueue(cliente);
            }
        }
        
        Console.WriteLine("Fila de Atendimentos:");
        foreach (var cliente in filaAtendimentos)
        {
            Console.WriteLine($"ID: {cliente.IdCliente}, Nome: {cliente.Nome}, Email: {cliente.Email}");
        }
        while (true)
        {
            Console.Write($"Deseja atender o próximo cliente na fila? (s/n): ");
            string resposta = Console.ReadLine()?.ToLower().Trim() ?? string.Empty;

            while (string.IsNullOrEmpty(resposta) || (resposta[0] != 's' && resposta[0] != 'n'))
            {
                    Console.WriteLine($"Resposta inválida. Tente novamente.");
                    Console.Write("Deseja atender o próximo cliente na fila? (s/n): ");
                    resposta = Console.ReadLine()?.ToLower().Trim() ?? string.Empty;
            }
            if (resposta[0] == 's')
            {
                if (filaAtendimentos.TryDequeue(out Cliente? clienteAtendido))
                {
                    Console.WriteLine($"Atendendo Cliente ID: {clienteAtendido.IdCliente}, Nome: {clienteAtendido.Nome}");
                    historicoAtendimentos.Push(clienteAtendido);
                }
                else
                {
                    Console.WriteLine("Não há mais clientes na fila de atendimentos.");
                    break;
                }
            }
            else
            {
                Console.WriteLine("Encerrando atendimento.");
                break;
            }

        }
            
     }
    public static void HistoricoDeAtendimentos(Stack<Cliente> historicoAtendimentos)
    {
        if (historicoAtendimentos.Count == 0)
        {
            Console.WriteLine("Nenhum atendimento realizado ainda.");
            return;
        }

        Console.WriteLine("Histórico de Atendimentos:");
        foreach (var cliente in historicoAtendimentos)
        {
            Console.WriteLine("-----------------------------------");
            Console.WriteLine($"ID: {cliente.IdCliente}");
            Console.WriteLine($"Nome: {cliente.Nome}");
            Console.WriteLine($"Email: {cliente.Email}");
            Console.WriteLine("-----------------------------------");
        }
    }
    public static void UltimosCincoAtendimentos(Stack<Cliente> historicoAtendimentos, ref Cliente[] ultimosAtendimentos)
    {
        List<Cliente> tempList = new List<Cliente>(historicoAtendimentos);
        int count = 0;
        foreach (var histCliente in historicoAtendimentos)
        {
            tempList.Add(histCliente);
        }

        if (tempList.Count <= 0)
        {
            Console.WriteLine("Nenhum atendimento realizado ainda.");
            return;
        }
        Console.WriteLine("Últimos 5 Atendimentos:");

        foreach (var item in tempList)
        {
            if (count >= 5)
            {
                break;
            }
            Console.WriteLine($"ID: {item.IdCliente}, Nome: {item.Nome}, Email: {item.Email}");
            count++;
            
        }
    }
}
