using FashionShop.Models.Domain;
using FashionShop.Models.DTO.UserDTO;
using FashionShop.Models.DTO.ProductDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FashionShop.Models.ViewModel;

namespace FashionShop.Data
{
    public class FashionShopDBContext : IdentityDbContext
    {
        public FashionShopDBContext(DbContextOptions<FashionShopDBContext> options) : base(options) 
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }
        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<RevenueStatisticViewModel> RevenueStatisticViewModel { get; set; }

        protected override async void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderID, e.ProductID });
            });           

            modelBuilder.Entity<FavoriteProduct>(entity =>
            {
                entity.HasKey(e => new { e.UserID, e.ProductID });
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.HasIndex(v => v.DiscountCode).IsUnique();
            });

            modelBuilder.Entity<Order>()
            .HasOne(o => o.Province)
            .WithMany(p => p.Orders)
            .HasForeignKey(o => o.ProvinceID)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Ward>()
            .HasOne(w => w.Province)
            .WithMany(p => p.Wards)
            .HasForeignKey(w => w.ProvinceID)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Ward>()
                .HasOne(w => w.District)
                .WithMany(d => d.Wards)
                .HasForeignKey(w => w.DistrictID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RevenueStatisticViewModel>(entity =>
            {
                entity.HasNoKey();
            });

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            // Tạo sẵn quyền quản trị viên, nhân viên và khách hàng
            var adminRoleId = "25d9875c-878d-414e-8e6f-b4c28815f739";
            var memberRoleId = "3195156e-ef20-4c3d-9406-7bc7e87fd6f6";
            var userRoleId = "9cd0f7a2-741d-405a-a8a3-a34b22da200c";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Quản trị viên",
                    NormalizedName = "Quản trị viên".ToUpper(),
                },
                new IdentityRole
                 {
                    Id = memberRoleId,
                    ConcurrencyStamp = memberRoleId,
                    Name = "Nhân viên",
                    NormalizedName = "Nhân viên".ToUpper()
                 },
                new IdentityRole
                 {
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId,
                    Name = "Khách hàng",
                    NormalizedName = "Khách hàng".ToUpper()
                 }

            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
       
    }
}
