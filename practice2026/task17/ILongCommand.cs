namespace Task19;

public interface ILongCommand : ICommand
{
    bool IsCompleted { get; }
}
