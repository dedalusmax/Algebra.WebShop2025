using Algebra.WebShop2025.App.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Algebra.WebShop2025.App.Data;

public class ApplicationDbContext : IdentityDbContext
{
    #region Domain entities

    public DbSet<Category> Categories { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductCategory> ProductCategories { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    #endregion

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
       

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Category>()
            .HasIndex(x => x.Name)
            .IsUnique();

        builder.Entity<Product>()
            .HasIndex(x => x.Name)
            .IsUnique();

        builder.Entity<OrderItem>()
            .HasIndex(x => new { x.OrderId, x.ProductId })
            .IsUnique();

        base.OnModelCreating(builder);
    }
}
