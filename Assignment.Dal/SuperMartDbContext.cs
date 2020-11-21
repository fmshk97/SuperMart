using Assignment.Dal.Entities;
using Assignment.Dal.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Dal
{
    public class SuperMartDbContext : DbContext
    {
        public SuperMartDbContext(DbContextOptions<SuperMartDbContext> options) : base(options)
        {

        }

        public SuperMartDbContext() : base ("server=localhost:3506;database=supermart;user=root;password=!MySQL97;".StringToContextOptions())
        {

        }

        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureStoreContext();
            modelBuilder.ConfigureProductContext();
            base.OnModelCreating(modelBuilder);
        }
    }
}
