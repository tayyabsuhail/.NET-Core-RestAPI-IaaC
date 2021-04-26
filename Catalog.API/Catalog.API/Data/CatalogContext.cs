using Catalog.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options)
            : base(options)
        {
            CatalogContextSeed.SeedData(this);
        }
        public DbSet<Product> Products { get; set; }
    }
}
