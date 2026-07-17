using System;
using System.Threading;

namespace Task15;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsnumber)
    {
        if (step <= 0)
        {
            throw new ArgumentException(nameof(step), "Размер шага разбиения должен быть больше нуля."); 
        }
        if (threadsnumber <= 0)
        {
            throw new ArgumentException(nameof(threadsnumber), "Число потоков должно быть больше нуля.");
        }

        ArgumentNullException.ThrowIfNull(function);

        if (a == b)
        {
            return 0;
        }

        bool isReversed = false;
        if (a > b)
        {
            double temp = a;
            a = b;
            b = temp;
            isReversed = true;
        }

        object syncRoot = new object();
        var barrier = new Barrier(threadsnumber + 1);
        double result = 0;
        double segmentLength = (b - a) / threadsnumber;

        for (int i = 0; i < threadsnumber; i++)
        {
            double segmentStart = a + i * segmentLength;
            double segmentEnd = i == threadsnumber - 1 ? b : segmentStart + segmentLength;

            Thread worker = new Thread(() =>
            {
                double localSum = 0;

                for (double x1 = segmentStart; x1 < segmentEnd;)
                {
                    double x2 = Math.Min(x1 + step, segmentEnd);
                    localSum += (function(x1) + function(x2)) / 2.0 * (x2 - x1);
                    x1 = x2;
                }

                lock (syncRoot)
                {
                    result += localSum;
                }

                barrier.SignalAndWait();
            });
            worker.Start();
        }

        barrier.SignalAndWait();
        return isReversed ? -result : result;
    }

    public static double SolveSingleThread(double a, double b, Func<double, double> function, double step)
    {
        if (step <= 0)
        {
            throw new ArgumentException(nameof(step), "Размер шага должен быть больше нуля.");
        }

        ArgumentNullException.ThrowIfNull(function);

        if (a == b)
        {
            return 0;
        }

        bool isReversed = false;
        if (a > b)
        {
            double temp = a; a = b; b = temp;
            isReversed = true;
        }

        double result = 0;
        for (double x1 = a; x1 < b;)
        {
            double x2 = Math.Min(x1 + step, b);
            result += (function(x1) + function(x2)) / 2.0 * (x2 - x1);
            x1 = x2;
        }

        return isReversed ? -result : result;
    }
}
