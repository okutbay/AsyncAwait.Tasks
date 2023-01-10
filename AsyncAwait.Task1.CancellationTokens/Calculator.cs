using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal static class Calculator
{
    // todo: change this method to support cancellation token
    public static long Calculate(int n /*, CancellationToken token*/)
    {
        long sum = 0;

        for (var i = 0; i < n; i++)
        {
            // i + 1 is to allow 2147483647 (Max(Int32)) 
            sum = sum + (i + 1);
            Thread.Sleep(10);
        }

        return sum;
    }

    public static async Task<long> CalculateAsync(int n, CancellationToken token)
    {
        long sum = 0;

        Parallel.For(0, n, (i, pls) =>
        {
            // i + 1 is to allow 2147483647 (Max(Int32)) 
            sum = sum + (i + 1);
            Thread.Sleep(10);

            if (token.IsCancellationRequested)
            {
                sum = -1;
                pls.Break();
            }
        });

        await Task.CompletedTask;
        return sum;
    }
}
