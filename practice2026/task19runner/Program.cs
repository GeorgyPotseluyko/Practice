using System;
using System.Collections.Generic;

namespace Task19;

internal static class Program
{
    private static void Main()
    {
        ServerThread server = new();

        List<TestCommand> testCommands = new();
        List<int> executionOrder = new();

        int commandsExecutedThreeTimes = 0;

        void AfterExecute(TestCommand command)
        {
            executionOrder.Add(command.Id);

            if (command.Counter == 3)
            {
                commandsExecutedThreeTimes++;

                if (commandsExecutedThreeTimes == 5)
                {
                    server.AddCommand(new HardStop(server));
                }
            }
        }

        for (int id = 1; id <= 5; id++)
        {
            TestCommand command = new(id, AfterExecute);

            testCommands.Add(command);
            server.AddCommand(command);
        }

        server.Start();

        if (!server.Wait())
        {
            throw new TimeoutException("ServerThread не завершился за отведённое время.");
        }

        Console.WriteLine();
        Console.WriteLine("HardStop выполнен. Поток остановлен.");
        Console.WriteLine($"Общее количество вызовов Execute: {executionOrder.Count}");
    }
}
