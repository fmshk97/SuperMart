using SuperMart.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace SuperMart.Dal.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureStoreContext(this ModelBuilder builder)
        {
            builder.Entity<Store>(store =>
            {
                store.ToTable("Stores").HasKey(x => x.Id);

                store.Property(x => x.StoreName).IsRequired();
                store.Property(x => x.City).IsRequired();
                store.Property(x => x.Country).IsRequired();
                store.Property(x => x.Pin).IsRequired();
                store.Property(x => x.JoinedOn).IsRequired();

                store.HasIndex(s => s.StoreName).IsUnique();
                
                store.HasMany(x => x.Products).WithOne(x => x.Store)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public static void ConfigureProductContext(this ModelBuilder builder)
        {
            builder.Entity<Product>(product =>
            {
                product.ToTable("Products").HasKey(x => x.Id);

                product.Property(x => x.Name).IsRequired();
                product.Property(x => x.Description);
                product.Property(x => x.Price).IsRequired();
                product.Property(x => x.Category).IsRequired();
                product.Property(x => x.AddedOn).IsRequired();

                product.HasIndex("StoreId", nameof(Product.Name)).IsUnique();
            });
        }
    }
}
