namespace Task17;

public class HardStop : ICommand
{
    private readonly ServerThread serverThread;

    public HardStop(ServerThread serverThread)
    {
        this.serverThread = serverThread;
    }

    public void Execute()
    {
        serverThread.RequestHardStop();
    }
}
