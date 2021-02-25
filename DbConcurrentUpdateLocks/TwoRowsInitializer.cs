using System.Data.Entity;

namespace DbConcurrentUpdateLocks
{
    internal class TwoRowsInitializer : IDatabaseInitializer<ModelContext>
    {
        public void InitializeDatabase(ModelContext context)
        {
            if (context.Database.Exists())
            {
                context.Database.Delete();
            }
            context.Database.Create();
            var rows = context.Database.ExecuteSqlCommand("delete from dbo.Rows");
            context.SaveChanges();


            using (var tran = context.Database.BeginTransaction())
            {
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Rows ON");
                context.Rows.Add(new Model() { ID = 1, Value = "string 1" });
                context.Rows.Add(new Model() { ID = 2, Value = "string 2" });
                tran.Commit();
            }

            context.SaveChanges();
        }
    }
}