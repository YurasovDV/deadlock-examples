using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LockOnIndexKey
{
    class Program
    {
        static void Main(string[] args)
        {
            // don't expect anything interesting
            var connStr = "Data Source=DESKTOP-C6UM8VP\\ACER2;Initial Catalog=DeadLockDB;Integrated Security=True;Connect Timeout=10;";
            var context = new ModelContext(connStr);
            context.Database.Initialize(true);
            context.Dispose();

            try
            {
                Debugger.Break();
                Console.WriteLine("db is ready, starting locking");

                using (var conn1 = new SqlConnection(connStr))
                {
                    using (var conn2 = new SqlConnection(connStr))
                    {
                        conn1.Open();
                        conn2.Open();

                        Console.WriteLine("Connections open");

                        var tran1 = conn1.BeginTransaction();
                        var tran2 = conn2.BeginTransaction();

                        Console.WriteLine("Transactions began");

                        var command1 = conn1.CreateCommand();
                        var command2 = conn2.CreateCommand();

                        command1.CommandTimeout = 100_000;
                        command2.CommandTimeout = 100_000;

                        command1.Transaction = tran1;
                        command2.Transaction = tran2;

                        command1.CommandText = "UPDATE DeadlockIndexTest SET [Name] = 'Value1' WHERE [Name] = 'Value2'";
                        command2.CommandText =
                            "SELECT [Name] FROM DeadlockIndexTest WITH (UPDLOCK, FORCESEEK) WHERE [Name] IN ('Value1', 'Value2')";

                        var changed1 = command1.ExecuteNonQuery();
                        Console.WriteLine("command 1 step 1: completed");

                        Task<int> task2 = Task.Run(async () =>
                        {
                            var c2 = await command2.ExecuteNonQueryAsync();
                            Console.WriteLine("command 2 step 1: completed");
                            command2.Dispose();
                            tran2.Commit();
                            Console.WriteLine("Transaction 2 is committed");
                            return c2;
                        });


                        command1.Dispose();

                        command1 = conn1.CreateCommand();
                        command1.CommandText =
                            "SELECT [Name] FROM DeadlockIndexTest WITH (UPDLOCK, FORCESEEK) WHERE [Name] = 'Value1'";
                        command1.CommandTimeout = 100_000;
                        command1.Transaction = tran1;

                        var t1 = Task.Run(async () =>
                        {
                            await Task.Delay(4000);
                            var c1 = await command1.ExecuteNonQueryAsync();
                            Console.WriteLine("command 1 step 2: completed");
                            command1.Dispose();
                            tran1.Commit();
                            Console.WriteLine("Transaction 1 is committed");
                            return c1;
                        });
                        Task.WhenAll(t1, task2).Wait();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");

                if (ex is AggregateException aggregated)
                {
                    if (aggregated.InnerException is SqlException sqex)
                    {
                        Console.WriteLine(sqex.Message);
                        Console.WriteLine($"SqlException.Number: {sqex.Number}");
                        Console.WriteLine("Deadlock number - 1205");
                    }
                }
            }
            Console.WriteLine("Press enter");
            Console.ReadLine();
        }
    }
}
