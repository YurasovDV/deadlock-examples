using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DbConcurrentUpdateLocks
{
    class Program
    {
        static void Main(string[] args)
        {
            // don't expect anything interesting
            var connStr = "Data Source=localhost;Initial Catalog=DeadLockDB;Integrated Security=True;Connect Timeout=30;";
            var context = new ModelContext(connStr);
            context.Database.Initialize(true);
            context.Dispose();


            try
            {
                Console.WriteLine("db is ready, starting locking");

                SqlConnection conn1 = new SqlConnection(connStr);
                SqlConnection conn2 = new SqlConnection(connStr);

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

                command1.CommandText = "update dbo.Rows SET [value] = 'tran1' WHERE ID = 1";
                command2.CommandText = "update dbo.Rows SET [value] = 'tran2' WHERE ID = 2";

                var changed1 = command1.ExecuteNonQuery();
                Console.WriteLine("command 1 step 1: completed");
                var changed2 = command2.ExecuteNonQuery();
                Console.WriteLine("command 2 step 1: completed");

                command1.Dispose();
                command2.Dispose();

                command1 = conn1.CreateCommand();
                command2 = conn2.CreateCommand();

                command1.CommandText = "update dbo.Rows SET [value] = 'tran1' WHERE ID = 2";
                command2.CommandText = "update dbo.Rows SET [value] = 'tran2' WHERE ID = 1";

                command1.CommandTimeout = 100_000;
                command2.CommandTimeout = 100_000;

                command1.Transaction = tran1;
                command2.Transaction = tran2;

                var t1 = Task.Run(() =>
                {
                    command1.ExecuteNonQuery();
                    Console.WriteLine("command 1 step 2: completed");
                });

                var t2 = Task.Run(() =>
                {
                    command2.ExecuteNonQuery();
                    Console.WriteLine("command 2 step 2: completed");
                });

                Task result = Task.WhenAll(t1, t2);
                // here one transaction appears victim and we get aggregated exception with sql exception inside
                result.Wait();

                // never going come here
                tran1.Commit();
                Console.WriteLine("Transaction 1 is committed");
                tran2.Commit();
                Console.WriteLine("Transaction 2 is committed");

                command1.Dispose();
                command2.Dispose();

                conn1.Dispose();
                conn2.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");

                var aggregated = ex as AggregateException;
                if (aggregated != null)
                {
                    var sqex = aggregated.InnerException as SqlException;
                    if (sqex != null)
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
