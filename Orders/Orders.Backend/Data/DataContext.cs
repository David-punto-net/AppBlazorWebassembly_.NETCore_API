using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Orders.Shared.Entities;

namespace Orders.Backend.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<City>().HasIndex(c => new { c.StateId, c.Name }).IsUnique();
            modelBuilder.Entity<State>().HasIndex(c => new { c.CountryId, c.Name }).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(c => c.Name).IsUnique();

            DisebleCascadingDelete(modelBuilder);
        }

        private void DisebleCascadingDelete(ModelBuilder modelBuilder)
        {
            var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
            foreach (var relationship in relationships)
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}