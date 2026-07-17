namespace Task19;

public class RoundRobinScheduler : IScheduler
{
    private readonly Queue<ICommand> commands = new();

    public bool HasCommand()
    {
        return commands.Count > 0;
    }

    public ICommand Select()
    {
        return commands.Dequeue();
    }

    public void Add(ICommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        commands.Enqueue(command);
    }
}
