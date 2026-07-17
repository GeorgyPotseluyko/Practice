using ScottPlot;
using ScottPlot.TickGenerators;
using Task17;

namespace Task19.Graphs;

internal static class Graph19
{
    private const int ImageWidth = 1000;
    private const int ImageHeight = 600;

    private static readonly string ReportDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));

    private static void Main()
    {
        Directory.CreateDirectory(ReportDirectory);

        ExperimentResult result = RunExperiment();
        CreateExecutionOrderChart(result.ExecutionOrder);
        CreateCommandCallsChart(result.Commands);

        Console.WriteLine();
        Console.WriteLine("Графики созданы в папке:");
        Console.WriteLine(ReportDirectory);
    }

    private static ExperimentResult RunExperiment()
    {
        ServerThread server = new();
        List<TestCommand> commands = new();
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
            commands.Add(command);
            server.AddCommand(command);
        }

        server.Start();

        if (!server.Wait())
        {
            throw new TimeoutException("ServerThread не завершился за отведенное время.");
        }

        return new ExperimentResult(commands, executionOrder);
    }

    private static void CreateExecutionOrderChart(List<int> executionOrder)
    {
        double[] callNumbers = Enumerable.Range(1, executionOrder.Count).Select(value => (double)value).ToArray();

        double[] commandIds = executionOrder.Select(value => (double)value).ToArray();

        Plot plot = new();
        plot.Add.Scatter(callNumbers, commandIds);
        plot.Title("Порядок выполнения пяти длительных команд");
        plot.XLabel("Номер вызова Execute");
        plot.YLabel("Идентификатор команды");
        plot.Axes.Left.TickGenerator = CreateIntegerTicks(1, 5);
        plot.Axes.Bottom.TickGenerator = CreateIntegerTicks(1, 15);
        plot.Font.Automatic();

        string path = Path.Combine(ReportDirectory, "execution_order.png");
        plot.SavePng(path, ImageWidth, ImageHeight);
    }

    private static void CreateCommandCallsChart(List<TestCommand> commands)
    {
        double[] positions = commands.Select(command => (double)command.Id).ToArray();
        double[] values = commands.Select(command => (double)command.Counter).ToArray();

        Plot plot = new();
        var bars = plot.Add.Bars(positions, values);

        foreach (var bar in bars.Bars)
        {
            bar.Label = bar.Value.ToString("0");
        }

        bars.ValueLabelStyle.Bold = true;
        bars.ValueLabelStyle.FontSize = 16;

        plot.Title("Количество вызовов каждой команды до HardStop");
        plot.XLabel("Идентификатор команды");
        plot.YLabel("Количество вызовов Execute");
        plot.Axes.Bottom.TickGenerator = CreateIntegerTicks(1, 5);
        plot.Axes.Left.TickGenerator = CreateIntegerTicks(0, 3);
        plot.Axes.Margins(bottom: 0, top: 0.2);
        plot.Font.Automatic();

        string path = Path.Combine(ReportDirectory, "command_calls.png");
        plot.SavePng(path, ImageWidth, ImageHeight);
    }

    private static NumericManual CreateIntegerTicks(int first, int last)
    {
        Tick[] ticks = Enumerable.Range(first, last - first + 1).Select(value => new Tick(value, value.ToString())).ToArray();

        return new NumericManual(ticks);
    }

    private sealed record ExperimentResult(List<TestCommand> Commands, List<int> ExecutionOrder);
}
