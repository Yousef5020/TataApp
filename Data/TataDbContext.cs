using Microsoft.EntityFrameworkCore;
using TataApp.Models.Database;

namespace TataApp.Data
{
    public class TataDbContext : DbContext
    {
        public TataDbContext() { }

        public TataDbContext(DbContextOptions<TataDbContext> options) : base(options) { }

        public DbSet<Animal> Animals { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("DataSource=TataApp.db;Cache=Shared");
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}
