using System;
using System.Threading;
using System.Threading.Tasks;

namespace MonitorAwait
{
    class Program
    {
        private static object lockObject = new object();

        static async Task Main(string[] args)
        {
            Monitor.Enter(lockObject);
            Console.WriteLine($"ThreadId = {Thread.CurrentThread.ManagedThreadId}");
            _ = await GetValue();
            Console.WriteLine($"ThreadId = {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("Reentering lock");
            Monitor.Enter(lockObject);
            Console.WriteLine("Never get here");
            Monitor.Exit(lockObject);
            Monitor.Exit(lockObject);
        }

        private static async Task<int> GetValue()
        {
            await Task.Delay(10);
            return await Task.FromResult(10);
        }
    }
}
