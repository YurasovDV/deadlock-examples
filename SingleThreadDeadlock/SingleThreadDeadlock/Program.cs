using System.Threading;

namespace SingleThreadDeadlock
{
    class Program
    {
        static void Main(string[] _)
        {
            System.Console.WriteLine("started execution");
            Thread.CurrentThread.Join();
            System.Console.WriteLine("never get here");
        }
    }
}
