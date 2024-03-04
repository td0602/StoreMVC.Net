using Store_App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Store_App.Models.CustomIdentity;

namespace Store_App.Models 
{
    // App.Models.AppDbContext
    public class AppDbContext : IdentityDbContext<AppUser, CustomIdentityRole, int> // int cuoi cung la kieu du lieu Id cua AppUser va  CustomIdentityRole
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
            //Cấu hình để xóa đi Tiền tố "AspNet" trong các bảng generate mặc định của Identity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }  

            //Đổi tên khóa chính của AppUser
            modelBuilder.Entity<AppUser>().Property(u => u.Id).HasColumnName("AppUserId");
        }

        public DbSet<Book> Books { get; set; }


        public DbSet<Cart> Carts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

    }
}