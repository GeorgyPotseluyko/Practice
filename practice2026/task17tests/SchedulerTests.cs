using Xunit;

namespace Task17.Tests;

public class SchedulerTests
{
    [Fact]
    public void RoundRobin_ExecutesLongCommandsInTurns()
    {
        List<string> log = new();
        ServerThread server = new();

        server.AddCommand(new StepCommand("A", 3, log));
        server.AddCommand(new StepCommand("B", 3, log));
        server.AddCommand(new SoftStop(server));

        server.Start();

        Assert.True(server.Wait());
        Assert.Equal(new[] { "A1", "B1", "A2", "B2", "A3", "B3" }, log);
    }

    [Fact]
    public void LongCommand_DoesNotBlockOrdinaryCommand()
    {
        List<string> log = new();
        ServerThread server = new();

        server.AddCommand(new StepCommand("Long", 3, log));
        server.AddCommand(new TestCommand(() => log.Add("Simple")));
        server.AddCommand(new SoftStop(server));

        server.Start();

        Assert.True(server.Wait());
        Assert.Equal(new[] { "Long1", "Simple", "Long2", "Long3" }, log);
    }

    [Fact]
    public void SoftStop_WaitsForLongCommandsToFinish()
    {
        StepCommand command = new("A", 4, new List<string>());
        ServerThread server = new();

        server.AddCommand(command);
        server.AddCommand(new SoftStop(server));

        server.Start();

        Assert.True(server.Wait());
        Assert.True(command.IsCompleted);
        Assert.Equal(4, command.ExecutedSteps);
    }

    [Fact]
    public void HardStop_DoesNotWaitForLongCommand()
    {
        StepCommand longCommand = new("A", 5, new List<string>());
        bool lastCommandExecuted = false;
        ServerThread server = new();

        server.AddCommand(longCommand);
        server.AddCommand(new HardStop(server));
        server.AddCommand(new TestCommand(() => lastCommandExecuted = true));

        server.Start();

        Assert.True(server.Wait());
        Assert.Equal(1, longCommand.ExecutedSteps);
        Assert.False(longCommand.IsCompleted);
        Assert.False(lastCommandExecuted);
    }

    [Fact]
    public void EmptyThread_WakesWhenNewCommandIsAdded()
    {
        using ManualResetEventSlim executed = new(false);
        ServerThread server = new();

        server.Start();
        server.AddCommand(new TestCommand(() => executed.Set()));
        server.AddCommand(new SoftStop(server));

        Assert.True(executed.Wait(2000));
        Assert.True(server.Wait());
    }

    [Fact]
    public void ExceptionInCommand_DoesNotStopNextCommand()
    {
        bool nextCommandExecuted = false;
        Exception? receivedException = null;
        ServerThread server = new();

        server.ExceptionHandler = (_, exception) => receivedException = exception;
        server.AddCommand(new TestCommand(() => throw new InvalidOperationException("Ошибка команды")));
        server.AddCommand(new TestCommand(() => nextCommandExecuted = true));
        server.AddCommand(new SoftStop(server));

        server.Start();

        Assert.True(server.Wait());
        Assert.IsType<InvalidOperationException>(receivedException);
        Assert.True(nextCommandExecuted);
    }

    [Fact]
    public void HardStop_OutsideItsServerThread_ThrowsException()
    {
        ServerThread server = new();
        HardStop command = new(server);

        Assert.Throws<InvalidOperationException>(() => command.Execute());
    }

    [Fact]
    public void SoftStop_OutsideItsServerThread_ThrowsException()
    {
        ServerThread server = new();
        SoftStop command = new(server);

        Assert.Throws<InvalidOperationException>(() => command.Execute());
    }

    private class TestCommand : ICommand
    {
        private readonly Action action;

        public TestCommand(Action action)
        {
            this.action = action;
        }

        public void Execute()
        {
            action();
        }
    }

    private class StepCommand : ILongCommand
    {
        private readonly string name;
        private readonly int totalSteps;
        private readonly List<string> log;

        public int ExecutedSteps { get; private set; }
        public bool IsCompleted => ExecutedSteps >= totalSteps;

        public StepCommand(string name, int totalSteps, List<string> log)
        {
            this.name = name;
            this.totalSteps = totalSteps;
            this.log = log;
        }

        public void Execute()
        {
            ExecutedSteps++;
            log.Add($"{name}{ExecutedSteps}");
        }
    }
}
