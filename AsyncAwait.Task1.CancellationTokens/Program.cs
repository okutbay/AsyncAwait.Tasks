/*
* Study the code of this application to calculate the sum of integers from 0 to N, and then
* change the application code so that the following requirements are met:
* 1. The calculation must be performed asynchronously.
* 2. N is set by the user from the console. The user has the right to make a new boundary in the calculation process,
* which should lead to the restart of the calculation.
* 3. When restarting the calculation, the application should continue working without any failures.
*/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal class Program
{
    /// <summary>
    /// The Main method should not be changed at all.
    /// </summary>
    /// <param name="args"></param>
    /// 
    private static CancellationTokenSource tokenSource;

    private static void Main(string[] args)
    {
        Console.WriteLine("Mentoring program L2. Async/await.V1. Task 1");
        Console.WriteLine("Calculating the sum of integers from 0 to N.");
        
        ShowEnterN(1);

        tokenSource = new CancellationTokenSource();
        var input = Console.ReadLine();

        while (input.Trim().ToUpper() != "Q")
        {
            if (int.TryParse(input, out var n))
            {
                Task.Run(() =>
                {
                    CalculateSum(n);
                }).Wait();
            }
            else
            {
                if (input.Trim().ToUpper() == "N")
                {
                    tokenSource.Cancel();
                    tokenSource.Dispose();
                    tokenSource = new CancellationTokenSource();
                }
                else
                {
                    Console.WriteLine($"Invalid integer: '{input}'. Please try again.");
                }
            }

            input = Console.ReadLine();
        }

        Console.WriteLine("Press any key to continue");
        Console.ReadLine();
    }

    private static void CalculateSum(int n)
    {
        var token = tokenSource.Token;

        Console.WriteLine($"The task for {n} started... Enter 'N' to cancel the request: ");

        try
        {
            // todo: make calculation asynchronous
            var sum = Calculator.CalculateAsync(n, token);

            Console.WriteLine($"Sum for {n} = {sum}.");
        }
        catch (OperationCanceledException ex)
        {
            // todo: add code to process cancellation and uncomment this line    
            Console.WriteLine($"Sum for {n} cancelled... {ex.Message}");
        }
        finally
        {
            ShowEnterN(2);
        }
    }

    private static void ShowEnterN(int step)
    {
        Console.WriteLine("Use 'Q' key to exit...");
        Console.WriteLine();
        Console.Write($"({step}) Enter N: ");
    }
}
