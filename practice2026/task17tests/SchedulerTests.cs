using Xunit;

namespace Task19.Tests;

public class SchedulerTests
{
    [Fact]
    public void FiveCommands_AreExecutedThreeTimesEach()
    {
        ScenarioResult result = RunScenario();

        Assert.True(result.ThreadStopped);
        Assert.All(result.Commands, command => Assert.Equal(3, command.Counter));
    }

    [Fact]
    public void RoundRobin_ExecutesCommandsInCyclicOrder()
    {
        ScenarioResult result = RunScenario();

        int[] expectedOrder =
        {
            1, 2, 3, 4, 5,
            1, 2, 3, 4, 5,
            1, 2, 3, 4, 5
        };

        Assert.Equal(expectedOrder, result.ExecutionOrder);
    }

    [Fact]
    public void HardStop_PreventsFourthExecution()
    {
        ScenarioResult result = RunScenario();

        Assert.Equal(15, result.ExecutionOrder.Count);
        Assert.DoesNotContain(result.Commands, command => command.Counter > 3);
    }

    [Fact]
    public void TestCommand_RemainsLongRunningUntilExternalStop()
    {
        TestCommand command = new(1);

        command.Execute();
        command.Execute();
        command.Execute();

        Assert.Equal(3, command.Counter);
        Assert.False(command.IsCompleted);
    }

    [Fact]
    public void HardStop_ExecutedOutsideTargetThread_ThrowsException()
    {
        ServerThread server = new();
        HardStop hardStop = new(server);

        Assert.Throws<InvalidOperationException>(() => hardStop.Execute());
    }

    private static ScenarioResult RunScenario()
    {
        ServerThread server = new();
        List<TestCommand> commands = new();
        List<int> executionOrder = new();
        int commandsExecutedThreeTimes = 0;

        void AfterExecute(TestCommand command)
        {
            executionOrder.Add(command.Id);

            if (command.Counter == 3)
            {
                commandsExecutedThreeTimes++;

                if (commandsExecutedThreeTimes == 5)
                {
                    server.AddCommand(new HardStop(server));
                }
            }
        }

        for (int id = 1; id <= 5; id++)
        {
            TestCommand command = new(id, AfterExecute);
            commands.Add(command);
            server.AddCommand(command);
        }

        server.Start();
        bool stopped = server.Wait();

        return new ScenarioResult(commands, executionOrder, stopped);
    }

    private sealed record ScenarioResult(List<TestCommand> Commands, List<int> ExecutionOrder, bool ThreadStopped);
}
