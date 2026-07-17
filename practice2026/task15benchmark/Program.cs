using System;
using System.Diagnostics;

namespace Task15;

class Program
{
    public static void Main()
    {
        double tolerance = 1e-4;
        double[] steps = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
        double optimalStep = 0;

        Console.WriteLine("Проверка размера шага:");
        foreach (double step in steps)
        {
            double result = DefiniteIntegral.SolveSingleThread(-100, 100, Math.Sin, step);
            double actualError = Math.Abs(result);
            double errorBound = 200 * step * step / 12.0;

            Console.WriteLine($"Шаг: {step:G}; фактическая погрешность: {actualError:E2}; " +$"максимальная погрешность: {errorBound:E2}");
            if (errorBound <= tolerance)
            {
                optimalStep = step;
                break;
            }
        }

        if (optimalStep == 0)
        {
            Console.WriteLine("Подходящий размер шага не найден.");
            return;
        }

        Console.WriteLine($"\nВыбранный размер шага: {optimalStep:G}");
        Console.WriteLine("\nВремя решения для разного числа потоков:");

        DefiniteIntegral.Solve(-100, 100, Math.Sin, optimalStep, 1);
        DefiniteIntegral.SolveSingleThread(-100, 100, Math.Sin, optimalStep);

        int maxThreads = Math.Max(1, Environment.ProcessorCount);
        int optimalThreads = 1;
        double bestParallelTime = double.MaxValue;

        for (int threads = 1; threads <= maxThreads; threads++)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            double result = DefiniteIntegral.Solve(-100, 100, Math.Sin, optimalStep, threads);
            stopwatch.Stop();

            Console.WriteLine($"Потоков: {threads}; время решения: {stopwatch.Elapsed.TotalMilliseconds:F2} мс; " +$"результат: {result:E2}");
            if (stopwatch.Elapsed.TotalMilliseconds < bestParallelTime)
            {
                bestParallelTime = stopwatch.Elapsed.TotalMilliseconds;
                optimalThreads = threads;
            }
        }

        Stopwatch singleThreadStopwatch = Stopwatch.StartNew();
        double singleThreadResult = DefiniteIntegral.SolveSingleThread(-100, 100, Math.Sin, optimalStep);
        singleThreadStopwatch.Stop();

        double difference = (singleThreadStopwatch.Elapsed.TotalMilliseconds - bestParallelTime) / singleThreadStopwatch.Elapsed.TotalMilliseconds * 100;

        Console.WriteLine($"\nОптимальное число потоков: {optimalThreads}");
        Console.WriteLine($"Лучшее время многопоточного решения: {bestParallelTime:F2} мс");
        Console.WriteLine($"Время однопоточного решения: {singleThreadStopwatch.Elapsed.TotalMilliseconds:F2} мс; " +$"результат: {singleThreadResult:E2}");
        Console.WriteLine($"Разница многопоточной и однопоточной версий: {difference:F2} %");
    }
}
