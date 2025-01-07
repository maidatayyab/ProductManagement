using ProductManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.BusinessLogic
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<bool> AddProductAsync(Product product);
        Task<bool> UpdateProductAsync(int id, UpdateProduct product);
        Task<bool> DeleteProductAsync(int id);
    }
}
