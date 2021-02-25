using System.Data.Entity;

namespace LockOnIndexKey
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
            var rows = context.Database.ExecuteSqlCommand("delete from dbo.DeadlockIndexTest");
            context.SaveChanges();

            using (var tran = context.Database.BeginTransaction())
            {
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.DeadlockIndexTest ON");
                context.Rows.Add(new Model() { ID = 1, Name = "Value1", AdditionalInfo = "add1" });
                context.Rows.Add(new Model() { ID = 2, Name = "Value2", AdditionalInfo = "add2" });
                tran.Commit();
            }

            context.SaveChanges();
        }
    }
}