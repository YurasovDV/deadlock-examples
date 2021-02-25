using System.Data.Entity;

namespace LockOnIndexKey
{
    class ModelContext : DbContext
    {
        public ModelContext(string connString) : base(connString)
        {
            Database.SetInitializer(new TwoRowsInitializer());
        }

        public DbSet<Model> Rows { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Model>()
                .HasIndex(f => f.Name)
                .IsClustered(false)
                .IsUnique(false);
        }
    }
}
