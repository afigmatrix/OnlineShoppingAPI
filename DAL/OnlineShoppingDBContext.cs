using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingAPI.Entites;

namespace OnlineShoppingAPI.DAL
{
    public class OnlineShoppingDBContext : IdentityDbContext<IdentityUser>
    {
        public OnlineShoppingDBContext(DbContextOptions<OnlineShoppingDBContext> options) : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>().HasQueryFilter(m => !m.IsDeleted);
            base.OnModelCreating(builder);
        }

    }
}
