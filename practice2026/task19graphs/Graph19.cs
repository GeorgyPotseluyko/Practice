using ScottPlot;
using ScottPlot.TickGenerators;

namespace Task19.Graphs;

internal static class Graph19
{
    private const int ImageWidth = 1000;
    private const int ImageHeight = 600;

    private static readonly string ReportDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "images"));

    private static void Main()
    {
        Directory.CreateDirectory(ReportDirectory);

        ExperimentResult result = RunExperiment();
        CreateExecutionOrderChart(result.ExecutionOrder);
        CreateCommandCallsChart(result.Commands);
        CreateRoundRobinOrderChart();
        CreateRoundRobinProgressChart();
        CreateQuantumEfficiencyChart();


        Console.WriteLine();
        Console.WriteLine("Графики созданы:");
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

    private static void CreateRoundRobinOrderChart()
    {
        double[] iterations = Enumerable.Range(1, 12).Select(number => (double)number).ToArray();

        double[] selectedCommands =
        {
            3, 2, 1,
            3, 2, 1,
            3, 2, 1,
            3, 2, 1
        };

        Plot plot = new();
        plot.Add.Scatter(iterations, selectedCommands);
        plot.Title("Порядок выполнения команд Round Robin");
        plot.XLabel("Номер вызова Execute");
        plot.YLabel("Выбранная команда");

        Tick[] commandTicks =
        {
            new(1, "C"),
            new(2, "B"),
            new(3, "A")
        };

        plot.Axes.Left.TickGenerator = new NumericManual(commandTicks);
        plot.Axes.Bottom.TickGenerator = CreateIntegerTicks(1, 12);
        plot.Font.Automatic();

        string pngPath = Path.Combine(ReportDirectory, "round_robin_order.png");
        plot.SavePng(pngPath, ImageWidth, ImageHeight);
    }

    private static void CreateRoundRobinProgressChart()
    {
        double[] iterations = Enumerable.Range(1, 12).Select(number => (double)number).ToArray();

        double[] progressA = { 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4 };
        double[] progressB = { 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4 };
        double[] progressC = { 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4 };

        Plot plot = new();

        var commandA = plot.Add.Scatter(iterations, progressA);
        commandA.LegendText = "Команда A";

        var commandB = plot.Add.Scatter(iterations, progressB);
        commandB.LegendText = "Команда B";

        var commandC = plot.Add.Scatter(iterations, progressC);
        commandC.LegendText = "Команда C";

        plot.Title("Прогресс длительных команд");
        plot.XLabel("Номер вызова Execute");
        plot.YLabel("Количество выполненных шагов");
        plot.Axes.Bottom.TickGenerator = CreateIntegerTicks(1, 12);
        plot.Axes.Left.TickGenerator = CreateIntegerTicks(0, 4);
        plot.ShowLegend();
        plot.Font.Automatic();

        string pngPath = Path.Combine(ReportDirectory, "round_robin_progress.png");
        plot.SavePng(pngPath, ImageWidth, ImageHeight);
    }

    private static void CreateQuantumEfficiencyChart()
    {
        double[] quantumMilliseconds = { 1, 5, 10, 20, 50, 100 };

        const double usefulWorkMilliseconds = 1000;
        const double schedulerOverheadMilliseconds = 0.5;

        double[] quantumCount = quantumMilliseconds.Select(quantum => Math.Ceiling(usefulWorkMilliseconds / quantum)).ToArray();

        double[] totalTimeMilliseconds = quantumCount.Select(count => usefulWorkMilliseconds + count * schedulerOverheadMilliseconds).ToArray();

        double[] efficiencyPercent = totalTimeMilliseconds.Select(totalTime => usefulWorkMilliseconds / totalTime * 100).ToArray();

        Plot plot = new();
        plot.Add.Scatter(quantumMilliseconds, efficiencyPercent);
        plot.Title("Зависимость эффективности от кванта времени");
        plot.XLabel("Квант времени, мс");
        plot.YLabel("Эффективность, %");
        plot.Axes.Bottom.TickGenerator = new NumericManual(quantumMilliseconds.Select(value => new Tick(value, value.ToString("0"))).ToArray());
        plot.Font.Automatic();

        string pngPath = Path.Combine(ReportDirectory, "quantum_efficiency.png");
        plot.SavePng(pngPath, ImageWidth, ImageHeight);
    }

    private static NumericManual CreateIntegerTicks(int first, int last)
    {
        Tick[] ticks = Enumerable.Range(first, last - first + 1).Select(value => new Tick(value, value.ToString())).ToArray();

        return new NumericManual(ticks);
    }

    private sealed record ExperimentResult(List<TestCommand> Commands, List<int> ExecutionOrder);
}
