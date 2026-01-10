using System;

namespace Program;

public class Program
{
    public static void Main(string[] args)
    {
        List<User> users = new List<User>
        {
            new User("Maria Silva", "maria", "pass123"),
            new User("Bruno Costa", "brunoc", "b12345"),
            new User("Carla Souza", "carla_sz", "cs2024"),
            new User("Daniel Oliveira", "dan_oli", "dany789"),
            new User("Eduarda Lima", "edu_lima", "edu@321"),
            new User("Fabio Santos", "fabinho", "fabio99"),
            new User("Giovanna Rocha", "gi_rocha", "gio123"),
            new User("Henrique Melo", "henri_m", "hm7654"),
            new User("Isabela Farias", "isa_farias", "isa#2026"),
            new User("João Pereira", "jpereira", "jp@pass")
        };

        while (true)
        {
            
            Console.WriteLine($"============ SISTEMA SIMPLES DE LOGIN/CADASTRO ============");
            Console.WriteLine($"SEJA BEM VINDO AO SISTEMA DE LOGIN E CADASTRO!");
            Console.WriteLine("O QUE VOCÊ QUER FAZER HOJE: ");
            string[] optionsMens = new string[]{"[1] CADASTRAR","[2] LOGIN","[3] LISTAR USUARIOS","[4] SAIR"};
            foreach (var optMens in optionsMens)
            {
                Console.WriteLine(optMens);
            }
            Console.Write($"ESCOLHA UMA OPC: ");

            string? response = Console.ReadLine();
            bool convertResult = int.TryParse(response, out int option);

            if (string.IsNullOrEmpty(response) || !convertResult)
            {
                while (string.IsNullOrEmpty(response) || !convertResult)
                {
                    Console.WriteLine("=============================================================");
                    Console.WriteLine($"RESPOSTA INVALIDA! VERIFIQUE SUA OPÇÃO E TENTE NOVAMENTE!");
                    Console.WriteLine("=============================================================");
                    Console.WriteLine("O QUE VOCÊ QUER FAZER HOJE: ");
                    foreach (var optMens in optionsMens)
                    {
                        Console.WriteLine(optMens);
                    }
                    Console.WriteLine($"ESCOLHA UMA OPC: ");
                    response = Console.ReadLine();
                    convertResult = int.TryParse(response, out option);
                }
            }

            if (option == 4)
            {
                break;
            }


            
            switch (option)
            {
                case 1:
                    Console.WriteLine("=============================================================");
                    Console.WriteLine($"------------------ CADASTRO DE USUARIOS ------------------");
                    Console.Write($"DIGITE O NOME DO USUARIO: ");
                    response = Console.ReadLine();

                    
                    string name = Uteis.VerificarTexto(response,"NOME");

                    Console.Write($"DIGITE O NICKNAME DO USUARIO: ");
                    response = Console.ReadLine();

                    string nickname = Uteis.VerificarTexto(response,"NICKNAME");

                    Console.Write($"DIGITE A SENHA DO USUARIO: ");
                    response = Console.ReadLine();
                    string password = Uteis.VerificarTexto(response,"SENHA");

                    if (password.Length < 4)
                    {
                        while (password.Length < 4)
                        {
                            Console.WriteLine($"ERRO! SENHA DEVE SER MAIOR QUE 4 CARACTERES! ");
                            Console.Write($"DIGITE A SENHA DO USUARIO: ");
                            response = Console.ReadLine();
                            password = Uteis.VerificarTexto(response,"SENHA");
                        }
                    }

                    users.Add(new User(name,nickname,password));

                    Console.WriteLine($"USUARIO ADICIONADO COM SUCESSO!");
                    break;
            
                case 2:
                    Console.WriteLine("=============================================================");
                    Console.WriteLine($"------------------ LOGIN DE USUARIOS ------------------");
                    
                    Console.Write($"QUAL É O NICKNAME DA CONTA: ");
                    response = Console.ReadLine();
                    nickname = Uteis.VerificarTexto(response,"NICKNAME");

                    Console.Write($"QUAL É A SENHA: ");
                    response = Console.ReadLine();
                    password = Uteis.VerificarTexto(response,"SENHA");


                    User userAnonimo = new User(nickname,nickname,password);

                    bool verficarUsuario = false;
                    bool verificarSenha = false;

                    foreach (var user in users)
                    {
                        verficarUsuario = user.Nickname == userAnonimo.Nickname;
                        if (verficarUsuario)
                        {
                            verificarSenha = user.Password == userAnonimo.Password;               
                            while (true)
                            {
                                
                                if(verificarSenha)
                                {
                                    while (true)
                                    {
                                        Console.WriteLine($"=================================================");
                                        Console.WriteLine($"-------------- PAINEL DE CONTROLE --------------");
                                        Console.WriteLine($"=================================================");
                                        Console.WriteLine($"O QUE VOCÊ DESEJA FAZER: ");
                                        Console.WriteLine($"[1] MOSTRAR INFORMAÇÕES DO USUARIO");
                                        Console.WriteLine($"[2] SAIR DA CONTA");
                                        Console.Write($"QUAL É SUA OPC: ");
                                        response = Console.ReadLine();

                                        int painelOption = Uteis.VerificarNumero(response,"Opção");

                                        if (painelOption == 1)
                                        {
                                            Console.WriteLine("====================================================");
                                            Console.WriteLine($"NOME DA CONTA: {user.Name}");
                                            Console.WriteLine($"NICKNAME DA CONTA: {user.Nickname}");
                                            Console.WriteLine("====================================================");
                                        }
                                        if (painelOption == 2)
                                            break;
                                        
                                        Console.WriteLine("PRESSIONE ENTER PARA CONTINUAR....");
                                        Console.ReadLine();
                                    }
                                    break;
                                }
                                else
                                {
                                    while (!verificarSenha)
                                    {
                                        Console.WriteLine($"==========================================");
                                        Console.Write($"SENHA INCORRETA! DIGITE A SENHA CORRETA:");
                                        response = Console.ReadLine();
                                        password = Uteis.VerificarTexto(response,"SENHA");

                                        if (password.Length < 4)
                                        {
                                            while (password.Length < 4)
                                            {
                                                Console.WriteLine($"ERRO! SENHA DEVE SER MAIOR QUE 4 CARACTERES! ");
                                                Console.Write($"DIGITE A SENHA DO USUARIO: ");
                                                response = Console.ReadLine();
                                                password = Uteis.VerificarTexto(response,"SENHA");
                                            }
                                        }
                                        userAnonimo.Password =  password;
                                        verificarSenha = user.Password == userAnonimo.Password;
                                    }
                                }
                            }
                            
                        }
                    }
                    if (!verficarUsuario)
                    {
                        Console.WriteLine($"NÃO ENCONTAMOS NENHUM USUARIO COM ESSE NICKNAME");
                        Console.WriteLine($"VERIFIQUE SUA RESPOSTA E TENTE NOVAMENTE MAIS TARDE");
                    }
                    break;
                case 3:
                    Console.WriteLine("=============================================================");
                    Console.WriteLine($"------------------ LISTA DE USUARIOS ------------------");
                    foreach (var user in users)
                    {
                        Console.WriteLine($"NOME : {user.Name}");
                        Console.WriteLine($"NICKNAME: {user.Nickname}");
                        Console.WriteLine();
                    }
                    break;
                default:
                    Console.WriteLine("RESPOSTA INVALIDA! DIGITE UMA OPÇÃO CORRETA ENTRE 1 E 4");
                    break;
            }
            Console.WriteLine($"PRESSIONE ENTER PARA CONTINUAR....");
            Console.ReadLine();
            Console.Clear();
        }
        Console.WriteLine($"MUITO OBRIGADO PELA ATENÇÃO! TENHA UM OTIMO DIA");
        Console.WriteLine($"============================================================");
        
    }
}