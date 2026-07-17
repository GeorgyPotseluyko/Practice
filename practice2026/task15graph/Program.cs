using ScottPlot;

double[] threads = { 1, 2, 3, 4, 5, 6, 7, 8 };
double[] averageTimeMs = { 10.691, 7.299, 5.876, 5.428, 5.150, 4.882, 5.463, 5.880 };

Plot plot = new();
plot.Add.Scatter(averageTimeMs, threads);

plot.Title("Зависимость времени Solve от числа потоков");
plot.XLabel("Среднее время вычисления Solve, мс");
plot.YLabel("Количество потоков");

plot.Axes.SetLimitsX(4, 11.5);
plot.Axes.SetLimitsY(0, 9);

string outputPath = Path.GetFullPath("graph_threads.png");
plot.SavePng(outputPath, 900, 600);

Console.WriteLine($"График сохранён: {outputPath}");
Console.WriteLine("Оптимальный результат: 6 потоков, среднее время 4.882 мс.");
