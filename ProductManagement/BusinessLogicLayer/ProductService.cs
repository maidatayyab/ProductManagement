using ProductManagement.DataAccess;
using ProductManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.BusinessLogic
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            await _productRepository.AddProductAsync(product);
            return true;
        }

        public async Task<bool> UpdateProductAsync(int id, UpdateProduct product)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(id);
            if (existingProduct != null)
            {
                await _productRepository.UpdateProductAsync(id, product);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product != null)
            {
                await _productRepository.DeleteProductAsync(id);
                return true;
            }
            return false;
        }
    }
}
