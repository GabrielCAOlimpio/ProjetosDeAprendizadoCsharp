using System;

namespace Program;

public class User
{
    private string name = string.Empty;
    private string nickname = string.Empty;
    private string password = string.Empty;


    public string Name
    {
        get => string.IsNullOrEmpty(name) ? "unknown" : name;
        set
        {
            if (!string.IsNullOrEmpty(value))
                name = value;
        }
    }
    public string Nickname
    {
        get => string.IsNullOrEmpty(nickname) ? "unknown" : nickname;
        set
        {
            if (!string.IsNullOrEmpty(value))
                nickname = value;
            else
                throw new ArgumentException("Nickname cannot be null or empty");
        
        }
    }
    public string Password
    {
        get => password;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length >= 4)
                    password = value;
                else
                    throw new ArgumentException("Password must be at least 4 characters long");
            
            } 
            else
            {
                throw new ArgumentException("Password cannot be null or empty");
            }   
        }
    }

    public User(string name, string nickname, string password)
    {
        Name = name;
        Nickname = nickname;
        Password = password;
    }
    

}
public class Uteis
{
    public static string VerificarTexto(string? response, string sujeito)
    {
        if (string.IsNullOrEmpty(response))
        {
            while(string.IsNullOrEmpty(response))
            {
                Console.WriteLine($"{sujeito} INVALIDO! TEXTO NÃO PODE SER NULL OU VAZIO");
                Console.Write($"DIGITE O {sujeito} CORRETO: ");
                response = Console.ReadLine();
            }
        }
        return response;
    }
    public static int VerificarNumero(string? response, string sujeito)
    {
        if (string.IsNullOrEmpty(response) || !int.TryParse(response, out int n))
        {
            while(string.IsNullOrEmpty(response) || !int.TryParse(response, out n))
            {
                Console.WriteLine($"{sujeito} INVALIDO! TEXTO NÃO PODE SER NULL OU VAZIO");
                Console.Write($"DIGITE O {sujeito} CORRETO: ");
                response = Console.ReadLine();
            }
        }
        return n;

    }
}