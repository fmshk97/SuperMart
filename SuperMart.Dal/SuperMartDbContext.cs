using SuperMart.Dal.Entities;
using SuperMart.Dal.Extensions;
using Microsoft.EntityFrameworkCore;

namespace SuperMart.Dal
{
    public class SuperMartDbContext : DbContext
    {
        public SuperMartDbContext(DbContextOptions<SuperMartDbContext> options) : base(options)
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
