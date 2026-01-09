using System;

namespace Program;


public class VerificarResposta
{
    public bool VerificarNumero(double numero)
    {
        if (double.IsNaN(numero) || double.IsInfinity(numero))
        {
            return false;
        }
        return true;
    }
}
public class Convertor
{
    public static double ConverterToDouble(string? input)
    {
        if (double.TryParse(input, out double result))
        {
            return result;
        }
        else
        {
            throw new FormatException("Input is not a valid number.");
        }
    }
    public static int ConvertToInt(string? input)
    {
        if (int.TryParse(input, out int result))
        {
            return result;
        }
        else
        {
            throw new FormatException("Input is not a valid integer.");
        }
    }
}
public class Calculador : ICalculator
{
    private VerificarResposta verificador;

    public Calculador()
    {
        verificador = new VerificarResposta();
    }

    public double Add(double a, double b)
    {
        if (!verificador.VerificarNumero(a) || !verificador.VerificarNumero(b))
        {
            throw new ArgumentException("Invalid number provided. Try again.");
        }
        return a + b;
    }
    public double Subtract(double a, double b)
    {
        if (!verificador.VerificarNumero(a) || !verificador.VerificarNumero(b))
        {
            throw new ArgumentException("Invalid number provided. Try again.");
        } 
        return a - b;
    }
    public double Multiply(double a, double b)
    {
        if (!verificador.VerificarNumero(a) || !verificador.VerificarNumero(b))
        {
            throw new ArgumentException("Invalid number provided. Try again.");
        }
        return a * b;
    }
    public double Divide(double a, double b)
    {
        if (!verificador.VerificarNumero(a) || !verificador.VerificarNumero(b))
        {
            throw new ArgumentException("Invalid number provided. Try again.");
        }
        else if (b == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero.");
        }
        return a / b;
    }
}
