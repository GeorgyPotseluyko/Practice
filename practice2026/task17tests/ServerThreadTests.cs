using Xunit;

namespace Task17.Tests;

public class ServerThreadTests
{
    [Fact]
    public void HardStop_DoesNotExecuteCommandsAfterIt()
    {
        ServerThread server = new();
        int counter = 0;

        server.AddCommand(new TestCommand(() => counter++));
        server.AddCommand(new HardStop(server));
        server.AddCommand(new TestCommand(() => counter++));

        server.Start();

        Assert.True(server.Wait());
        Assert.Equal(1, counter);
    }

    [Fact]
    public void SoftStop_ExecutesCommandsAfterIt()
    {
        ServerThread server = new();
        int counter = 0;

        server.AddCommand(new TestCommand(() => counter++));
        server.AddCommand(new SoftStop(server));
        server.AddCommand(new TestCommand(() => counter++));

        server.Start();

        Assert.True(server.Wait());
        Assert.Equal(2, counter);
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
}
