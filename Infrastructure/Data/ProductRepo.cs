using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ProductRepo : IProductRepo
    {
        private readonly StoreContext _store;

        public ProductRepo(StoreContext store)
        {
            _store = store;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _store.ProductBrands.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _store.Products
                                    .Include(p => p.ProductBrand)
                                    .Include(p => p.ProductType)
                                    .FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            var products = await _store.Products
                                    .Include(p => p.ProductBrand)
                                    .Include(p => p.ProductType)
                                    .ToListAsync();
            return products;
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypeAsync()
        {
            return await _store.ProductTypes.ToListAsync();
        }
    }
}
