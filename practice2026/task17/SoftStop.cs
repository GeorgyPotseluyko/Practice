namespace Task18;

public class SoftStop : ICommand
{
    private readonly ServerThread server;

    public SoftStop(ServerThread server)
    {
        this.server = server;
    }

    public void Execute()
    {
        server.RequestSoftStop();
    }
}
