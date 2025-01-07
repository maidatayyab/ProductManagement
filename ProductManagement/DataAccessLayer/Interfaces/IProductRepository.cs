using ProductManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.DataAccess
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<bool> AddProductAsync(Product product);
        Task<bool> UpdateProductAsync(int id, UpdateProduct product);
        Task<bool> DeleteProductAsync(int id);
    }
}
