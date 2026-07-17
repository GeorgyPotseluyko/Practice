namespace Task17;

public class SoftStop : ICommand
{
    private readonly ServerThread serverThread;

    public SoftStop(ServerThread serverThread)
    {
        this.serverThread = serverThread;
    }

    public void Execute()
    {
        serverThread.RequestSoftStop();
    }
}
