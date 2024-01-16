using Microsoft.EntityFrameworkCore;

namespace WebShop.Models
{
    internal class WebshopContext : DbContext
    {
        public DbSet<Models.Customer> Customers { get; set; }
        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.Order> Orders { get; set; }
        public DbSet<Models.Payment> Payments { get; set; }
        public DbSet<Models.Delivery> Deliveries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLExpress;Database=Webshop;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order>()
                .Property(s => s.Sum)
                .HasColumnType("decimal(18,2)");

        }
    }
}
