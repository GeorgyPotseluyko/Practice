namespace Task18;

public class HardStop : ICommand
{
    private readonly ServerThread server;

    public HardStop(ServerThread server)
    {
        this.server = server;
    }

    public void Execute()
    {
        server.RequestHardStop();
    }
}
