using Catalog.API.Data;
using Catalog.API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;

namespace Catalog.API.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;
        public ProductRepository(ICatalogContext catalogContext)
        {
            _context = catalogContext;
        }
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context
                           .Products
                           .Find(p => true)
                           .ToListAsync();
                            
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _context
                           .Products
                           .Find(p => id == p.Id)
                           .FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByNameAsync(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await _context
                           .Products
                           .Find(filter)
                           .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategoryAsync(string category)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, category);

            return await _context
                           .Products
                           .Find(filter)
                           .ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var update = await _context
                                    .Products
                                    .ReplaceOneAsync(filter: a => a.Id == product.Id, replacement: product);
            return update.IsAcknowledged
                   && update.ModifiedCount > 0;
        }


        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq( p => p.Id, id);

            var deleteResult = await _context
                                          .Products
                                          .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                   && deleteResult.DeletedCount > 0;
        }

    }
}
