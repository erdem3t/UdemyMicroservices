using Microsoft.EntityFrameworkCore;

namespace FreeCourse.Services.Order.Infrastructure
{
    public class OrderDbContext : DbContext
    {
        public const string DefaultSchema = "Ordering"; 
        
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }

        public DbSet<Domain.OrderAggregate.Order> Orders
        {
            get; set;
        }

        public DbSet<Domain.OrderAggregate.OrderItem> OrderItems
        {
            get; set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.OrderAggregate.Order>().ToTable("Orders", DefaultSchema);
            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().ToTable("OrderItems", DefaultSchema);
            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Domain.OrderAggregate.Order>().OwnsOne(o => o.Address).WithOwner(); // Owner Type belirmemiz sayesinde Efcore Order tablosunda adres ile ilgili alanlar oluşturur
            base.OnModelCreating(modelBuilder);
        }
    }
}
