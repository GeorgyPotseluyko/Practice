namespace Task17;

public interface ILongCommand : ICommand
{
    bool IsCompleted { get; }
}
