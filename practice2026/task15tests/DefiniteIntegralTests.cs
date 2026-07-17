using Xunit;

namespace Task15;

public class DefiniteIntegralTests
{
    [Fact]
    public void SolveSingleThread_IntegratesRemainingPartialInterval()
    {
        double result = DefiniteIntegral.SolveSingleThread(0, 1, _ => 1, 0.3);

        Assert.Equal(1, result, 12);
    }

    [Fact]
    public void Solve_ProducesSameResultAsSingleThread()
    {
        double singleThreadResult = DefiniteIntegral.SolveSingleThread(-100, 100, Math.Sin, 1e-3);
        double parallelResult = DefiniteIntegral.Solve(-100, 100, Math.Sin, 1e-3, 4);

        Assert.Equal(singleThreadResult, parallelResult, 8);
    }

    [Fact]
    public void Solve_IntegratesSinOnSymmetricIntervalWithRequiredAccuracy()
    {
        double result = DefiniteIntegral.Solve(-100, 100, Math.Sin, 1e-1, 2);

        Assert.InRange(Math.Abs(result), 0, 1e-4);
    }
}
