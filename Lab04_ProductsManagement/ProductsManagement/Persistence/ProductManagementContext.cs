using Microsoft.EntityFrameworkCore;
using ProductsManagement.Features.Products;

namespace ProductsManagement.Persistence;

public class ProductManagementContext(DbContextOptions<ProductManagementContext> options): DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.SKU)
            .IsUnique();
        
        modelBuilder.Entity<Product>()
            .HasIndex(p => new { p.Name, p.Brand })
            .IsUnique();
    }
}