using Store_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Store_App.Models 
{
    // App.Models.AppDbContext
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);    

            modelBuilder.Entity<Category>()
                .Property(entity => entity.CategoryId)
                .ValueGeneratedOnAdd();
        }

        public DbSet<Book> Books { get; set; }


        public DbSet<Cart> Carts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set;}

    }
}