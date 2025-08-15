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

    }
}
