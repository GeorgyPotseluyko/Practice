using Xunit;

namespace Task14.Tests;

public class Tests
{
    Func<double, double> X = x => x;
    Func<double, double> SIN = Math.Sin;

    [Fact]
    public void IntegralOfLinearFunctionIsZero()
    {
        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, X, 1e-4, 2), 1e-4);
    }

    [Fact]
    public void IntegralOfSinIsZero()
    {
        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, SIN, 1e-5, 8), 1e-4);
    }

    [Fact]
    public void IntegralOfLinearFunctionIsCalculatedCorrectly()
    {
        Assert.Equal(12.5, DefiniteIntegral.Solve(0, 5, X, 1e-6, 8), 1e-5);
    }
}
