using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(CatalogContext dbContext) : base(dbContext)
        {
           
        }

        public async Task<IEnumerable<Product>> getProductByCategory(string category)
        {
            return await _dbContext.Products.Where(p=>p.Category == category).ToListAsync();
        }
    }
}
