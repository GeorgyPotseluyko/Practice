namespace Task18;

public class ServerThread
{
    private readonly Queue<ICommand> commands = new();
    private readonly IScheduler scheduler;
    private readonly object locker = new();
    private readonly Thread thread;

    private bool hardStopRequested;
    private bool softStopRequested;
    private bool started;

    public Action<ICommand, Exception>? ExceptionHandler { get; set; }

    public ServerThread()
    {
        scheduler = new RoundRobinScheduler();
        thread = new Thread(ProcessCommands)
        {
            IsBackground = true
        };
    }

    public void AddCommand(ICommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        lock (locker)
        {
            if (hardStopRequested || softStopRequested)
            {
                throw new InvalidOperationException("Поток уже останавливается. Новые команды добавить нельзя.");
            }

            commands.Enqueue(command);

            Monitor.Pulse(locker);
        }
    }

    public void Start()
    {
        lock (locker)
        {
            if (started)
            {
                throw new InvalidOperationException("Поток уже был запущен.");
            }

            started = true;
            thread.Start();
        }
    }

    public bool Wait(int millisecondsTimeout = 5000)
    {
        return thread.Join(millisecondsTimeout);
    }

    internal void RequestHardStop()
    {
        CheckCurrentThread();

        lock (locker)
        {
            hardStopRequested = true;
            Monitor.PulseAll(locker);
        }
    }

    internal void RequestSoftStop()
    {
        CheckCurrentThread();

        lock (locker)
        {
            softStopRequested = true;
            Monitor.PulseAll(locker);
        }
    }

    private void CheckCurrentThread()
    {
        if (Thread.CurrentThread != thread)
        {
            throw new InvalidOperationException("Команда остановки должна выполняться в своем ServerThread.");
        }
    }

    private void ProcessCommands()
    {
        while (true)
        {
            ICommand command;

            lock (locker)
            {
                while (commands.Count == 0 && !scheduler.HasCommand() && !hardStopRequested && !softStopRequested)
                {
                    Monitor.Wait(locker);
                }

                if (hardStopRequested)
                {
                    return;
                }

                if (softStopRequested && commands.Count == 0 && !scheduler.HasCommand())
                {
                    return;
                }

                command = commands.Count > 0 ? commands.Dequeue() : scheduler.Select();
            }

            bool executedSuccessfully = false;

            try
            {
                command.Execute();
                executedSuccessfully = true;
            }
            catch (Exception exception)
            {
                ExceptionHandler?.Invoke(command, exception);
            }

            if (executedSuccessfully && command is ILongCommand longCommand && !longCommand.IsCompleted)
            {
                lock (locker)
                {
                    scheduler.Add(command);
                }
            }
        }
    }
}
