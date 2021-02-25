using System.Data.Entity;

namespace DbConcurrentUpdateLocks
{
    class ModelContext : DbContext
    {
        public ModelContext(string connString) : base(connString)
        {
            Database.SetInitializer<ModelContext>(new TwoRowsInitializer());
        }

        public DbSet<Model> Rows { get; set; }
    }
}
