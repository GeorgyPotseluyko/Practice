using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Task11;

public class CalculatorExample
{
    public async Task Run()
    {
        string code = @"
            public class Calculator : ICalculator
            {
                public int Add(int a, int b) => a + b;
                public int Minus(int a, int b) => a - b;
                public int Mul(int a, int b) => a * b;
                public int Div(int a, int b) 
                {
                    if (b == 0) throw new DivideByZeroException(""Ошибка. Деление на ноль."");
                    return a / b;
                }
            }
            return new Calculator();
        ";

        try
        {
            var options = ScriptOptions.Default.WithReferences(typeof(ICalculator).Assembly).WithImports("System", "Task11");;

            ICalculator calc = await CSharpScript.EvaluateAsync<ICalculator>(code, options);

            Console.WriteLine($"Сложение: 10 + 5 = {calc.Add(10, 5)}");
            Console.WriteLine($"Вычитание: 10 - 5 = {calc.Minus(10, 5)}");
            Console.WriteLine($"Умножение: 10 * 5 = {calc.Mul(10, 5)}");
            Console.WriteLine($"Деление: 10 / 5 = {calc.Div(10, 5)}");
        }
        catch (CompilationErrorException ex)
        {
            Console.WriteLine("Ошибка компиляции, класс в строке не соответствует интерфейсу: ");
            Console.WriteLine(ex.Message);
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine($"Ошибка выполнения: {ex.Message}");
        }
    }
}
