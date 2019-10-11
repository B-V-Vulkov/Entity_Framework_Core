namespace P03_SalesDatabase.Data
{
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class SalesContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<Store> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureCustomerEntity(modelBuilder);

            ConfigureProductEntity(modelBuilder);

            ConfigureSaleEntity(modelBuilder);

            ConfigureStoreEntity(modelBuilder);
        }

        private void ConfigureCustomerEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.CustomerId);

            modelBuilder.Entity<Customer>()
                .HasMany(s => s.Sales)
                .WithOne(c => c.Customer);

            modelBuilder.Entity<Customer>()
                .Property(n => n.Name)
                .HasMaxLength(100)
                .IsUnicode(true);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Email)
                .HasMaxLength(80)
                .IsUnicode(true);
        }

        private void ConfigureProductEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(s => s.Sales)
                .WithOne(p => p.Product);

            modelBuilder.Entity<Product>()
                .Property(n => n.Name)
                .HasMaxLength(50)
                .IsUnicode(true);
        }

        private void ConfigureSaleEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>()
                .HasKey(s => s.SaleId);
        }

        private void ConfigureStoreEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>()
                .HasKey(s => s.StoreId);

            modelBuilder.Entity<Store>()
                .HasMany(s => s.Sales)
                .WithOne(s => s.Store);

            modelBuilder.Entity<Store>()
                .Property(n => n.Name)
                .HasMaxLength(80)
                .IsUnicode(true);
        }
    }
}
