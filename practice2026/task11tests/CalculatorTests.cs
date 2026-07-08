using System.Threading.Tasks;
using Xunit;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Task11.Tests;

public class CalculatorTests
{
    [Fact]
    public async Task ValidString_ShouldRun()
    {
        string code = @"
            public class Calculator : ICalculator 
            {
                public int Add(int a, int b) => a + b;
                public int Minus(int a, int b) => a - b;
                public int Mul(int a, int b) => a * b;
                public int Div(int a, int b) => a / b;
            }
            return new Calculator();
        ";
        var options = ScriptOptions.Default.WithReferences(typeof(ICalculator).Assembly).WithImports("System", "Task11");;

        ICalculator calc = await CSharpScript.EvaluateAsync<ICalculator>(code, options);
        int resAdd = calc.Add(10, 5);
        int resMinus = calc.Minus(10, 5);
        int resMul = calc.Mul(10, 5);
        int resDiv = calc.Div(10, 5);

        Assert.Equal(15, resAdd);
        Assert.Equal(5, resMinus);
        Assert.Equal(50, resMul);
        Assert.Equal(2, resDiv);
    }

    [Fact]
    public async Task MissingInterface_ShouldFail()
    {
        string code = @"
            public class Calculator : ICalculator 
            {
                public int Add(int a, int b) => a + b;
            }
            return new Calculator();
        ";
        var options = ScriptOptions.Default.WithReferences(typeof(ICalculator).Assembly).WithImports("System", "Task11");;

        await Assert.ThrowsAsync<CompilationErrorException>(async () =>
        {
            await CSharpScript.EvaluateAsync<ICalculator>(code, options);
        });
    }

    [Fact]
    public async Task InvalidSyntax_ShouldFail()
    {
        string code = @"
            public class Calculator : ICalculator 
            { 
                public int Add(int a, int b) { return a + b } 
            }
            return new Calculator();
        ";

        await Assert.ThrowsAsync<CompilationErrorException>(async () =>
        {
            await CSharpScript.EvaluateAsync(code);
        });
    }

    [Fact]
    public async Task MathError_ShouldFail()
    {
        string code = @"
            public class Calculator : ICalculator 
            {
                public int Add(int a, int b) { return a + b; }
                public int Minus(int a, int b) { return a - b; }
                public int Mul(int a, int b) { return a * b; }
                public int Div(int a, int b) { return a / b; } 
            }
            return new Calculator();
        ";
        var options = ScriptOptions.Default.WithReferences(typeof(ICalculator).Assembly).WithImports("System", "Task11");;

        ICalculator calc = await CSharpScript.EvaluateAsync<ICalculator>(code, options);

        Assert.Throws<DivideByZeroException>(() => calc.Div(10, 0));
    }
}
