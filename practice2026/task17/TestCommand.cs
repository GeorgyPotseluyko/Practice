namespace Task19;

public class TestCommand : ILongCommand
{
    private readonly int id;
    private readonly Action<TestCommand>? afterExecute;

    public int Id => id;
    public int Counter { get; private set; }

    public bool IsCompleted => false;

    public TestCommand(int id, Action<TestCommand>? afterExecute = null)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "Идентификатор должен быть положительным.");
        }

        this.id = id;
        this.afterExecute = afterExecute;
    }

    public void Execute()
    {
        Counter++;
        Console.WriteLine($"Поток {id} вызов {Counter}");
        afterExecute?.Invoke(this);
    }
}
