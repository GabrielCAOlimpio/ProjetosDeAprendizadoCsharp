using System;

namespace Program;

public class Program
{
    public static void Main(string[] args)
    {
        ICalculator calculator = new Calculador();
        Console.WriteLine("======== Simple Calculator ========");
        
        try
        {
            Console.Write($"Write a number: ");
            string? response = Console.ReadLine();
            double a = Convertor.ConverterToDouble(response);

            Console.Write($"Write another number: ");
            response = Console.ReadLine();
            double b = Convertor.ConverterToDouble(response);

            while (true)
            {
                Console.WriteLine($"Select an operation: ");
                Console.WriteLine($"[1] - Addition");
                Console.WriteLine($"[2] - Subtraction");
                Console.WriteLine($"[3] - Multiplication");
                Console.WriteLine($"[4] - Division");
                Console.WriteLine($"[5] - Re-enter numbers");
                Console.WriteLine($"[0] - Exit");
                Console.Write($"Your choice: ");
                string? operation = Console.ReadLine();
                int op = Convertor.ConvertToInt(operation);
                switch (op)
                {
                    case 0:
                        Console.WriteLine("Exiting the calculator. Goodbye!");
                        return;
                    case 1:
                        Console.WriteLine($"Result: {a} + {b} = {calculator.Add(a, b)}");
                        break;
                    case 2:
                        Console.WriteLine($"Result: {a} - {b} = {calculator.Subtract(a,b)}");
                        break;
                    case 3:
                        Console.WriteLine($"Result: {a} * {b} = {calculator.Multiply(a,b)}");
                        break;
                    case 4:
                        Console.WriteLine($"Result: {a} / {b} = {calculator.Divide(a,b)}");
                        break;  
                    case 5:
                        Console.Write($"Write a number: ");
                        response = Console.ReadLine();
                        a = Convertor.ConverterToDouble(response);

                        Console.Write($"Write another number: ");
                        response = Console.ReadLine();
                        b = Convertor.ConverterToDouble(response);

                        Console.WriteLine($"Numbers updated successfully.");    
                        
                        break;
                    default:
                        Console.WriteLine("Invalid operation selected. Try again.");
                        continue;
                }
                Console.WriteLine($"Press Enter to continue...");
                Console.ReadLine();
                Console.Clear();
            }
        }
        catch (FormatException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
        finally
        {
            Console.WriteLine($"Thank you for using the Simple Calculator!");
            Console.WriteLine($"===================================");
        }
    }
}