using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.Entities;

namespace StockTrackingSystem.DbContexts
{
    public class StockContext : DbContext
    {
        public StockContext(DbContextOptions<StockContext> options) 
            : base(options) 
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemProperty> ItemProperties { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<StockCard> StockCards { get; set; }
        public DbSet<Transfer> Transfers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transfer>(e =>
            {
                // Transfer.FromWarehouseId -> Warehouse
                e.HasOne(t => t.FromWarehouse)
                 .WithMany()                               
                 .HasForeignKey(t => t.FromWarehouseId)
                 .OnDelete(DeleteBehavior.Restrict);      

                // Transfer.ToWarehouseId -> Warehouse
                e.HasOne(t => t.ToWarehouse)
                 .WithMany()
                 .HasForeignKey(t => t.ToWarehouseId)
                 .OnDelete(DeleteBehavior.Restrict);

                // Transfer.ItemId -> Item 
                e.HasOne(t => t.Item)
                 .WithMany()
                 .HasForeignKey(t => t.ItemId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
