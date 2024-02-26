using E_Commerce_Backend.Models.ENTITYS;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Dbcontext
{
    public class ShoesDbcontext:DbContext
    {
        private readonly IConfiguration _configuration;
        public string Csting;
        public ShoesDbcontext(IConfiguration configuration)
        {
            _configuration = configuration;
            Csting = _configuration["ConnectionStrings:DefaultConnection"];
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Csting);
        }
        public DbSet<Users> users { get; set; }
        public DbSet<Products> product { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Cart>cart  { get; set; }
        public DbSet<CartItem>cartItems  { get; set; }
        public DbSet<OrderMain>orders { get; set; }
        public DbSet<OrderItem>orderitems { get; set; }
        public DbSet<WhishList>whishLists { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Products>()
                .HasOne(c => c.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Users>()
                .HasOne(u => u.cart)
                .WithOne(c => c.users)
                .HasForeignKey<Cart>(c => c.UserId);

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.cartItems)
                .WithOne(ci => ci.cart)
                .HasForeignKey(ci => ci.CartId);

            modelBuilder.Entity<CartItem>()
                 .HasOne(ci => ci.product)
                 .WithMany(p => p.CartItems)
                 .HasForeignKey(p => p.ProductId);


            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<WhishList>()
                .HasOne(w => w.users)
                .WithMany(u => u.whishLists)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<WhishList>()
                .HasOne(w => w.products)
                .WithMany()
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<OrderMain>()
                .HasOne(o => o.users)
                .WithMany(u => u.Orders)
                .HasForeignKey(u => u.userId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(o => o.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            modelBuilder.Entity<Users>()
                .Property(u => u.isBlocked)
                .HasDefaultValue(false);
            modelBuilder.Entity<OrderMain>()
                .Property(o => o.OrderStatus)
                .HasDefaultValue("processing");


            



            base.OnModelCreating(modelBuilder);
        }


    }
}
