using System;
using System.Threading;

namespace DoubleObjectLock
{
    class Program
    {
        static object lock1 = new object();
        static object lock2 = new object();

        static void Main(string[] args)
        {
            Thread t1 = new Thread(ThreadOne);
            Thread t2 = new Thread(ThreadTwo);
            Console.WriteLine("Start");
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
            Console.WriteLine("End");
            Console.ReadLine();
        }

        static void ThreadOne()
        {
            lock (lock1)
            {
                Console.WriteLine("ThreadOne got lock1");
                Thread.Sleep(1000);
                lock (lock2)
                {
                    Console.WriteLine("ThreadOne got both locks!");
                    Thread.Sleep(1000);
                }
            }
        }

        static void ThreadTwo()
        {
            lock (lock2)
            {
                Console.WriteLine("ThreadTwo got lock2");
                Thread.Sleep(1000);
                lock (lock1)
                {
                    Console.WriteLine("ThreadTwo got both locks!");
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
