using System;
using System.Threading;
using System.Threading.Tasks;

namespace MonitorAwait
{
    class Program
    {
        static object lockObject = new object();

        static async Task Main(string[] args)
        {
            Monitor.Enter(lockObject);
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            var value = await GetValue();
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Monitor.Enter(lockObject);
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
