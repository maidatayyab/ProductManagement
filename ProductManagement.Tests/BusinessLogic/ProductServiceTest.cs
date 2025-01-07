using Moq;
using NUnit.Framework;
using ProductManagement.Models;
using ProductManagement.DataAccess;
using ProductManagement.BusinessLogic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.Tests.BusinessLogic
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private ProductService _productService;

        [SetUp]
        public void SetUp()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_mockProductRepository.Object);
        }

        [Test]
        public async Task GetProductsAsync_ReturnsAllProducts()
        {
            var mockProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Apple", Price = 1.99m, Category = "Food", ProductCode = "A123", StockQuantity = 100, DateAdded = DateTime.UtcNow },
                new Product { Id = 2, Name = "Smartphone", Price = 499.99m, Category = "Electronics", ProductCode = "S456", StockQuantity = 50, DateAdded = DateTime.UtcNow }
            };

            _mockProductRepository.Setup(repo => repo.GetAllProductsAsync()).ReturnsAsync(mockProducts);

            var result = await _productService.GetProductsAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Is.EqualTo(mockProducts));
        }

        [Test]
        public async Task GetProductByIdAsync_ReturnsProduct_WhenProductExists()
        {
            var mockProduct = new Product { Id = 1, Name = "Apple", Price = 1.99m, Category = "Food", ProductCode = "A123", StockQuantity = 100, DateAdded = DateTime.UtcNow };

            _mockProductRepository.Setup(repo => repo.GetProductByIdAsync(1)).ReturnsAsync(mockProduct);

            var result = await _productService.GetProductByIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(mockProduct));
        }

        [Test]
        public async Task GetProductByIdAsync_ReturnsNull_WhenProductDoesNotExist()
        {
            _mockProductRepository.Setup(repo => repo.GetProductByIdAsync(999)).ReturnsAsync((Product)null);

            var result = await _productService.GetProductByIdAsync(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task AddProductAsync_ReturnsTrue_WhenProductIsAdded()
        {
            var newProduct = new Product { Id = 1, Name = "Apple", Price = 1.99m, Category = "Food", ProductCode = "A123", StockQuantity = 100, DateAdded = DateTime.UtcNow };

            _mockProductRepository.Setup(repo => repo.AddProductAsync(newProduct)).ReturnsAsync(true);

            var result = await _productService.AddProductAsync(newProduct);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task UpdateProductAsync_ReturnsTrue_WhenProductIsUpdated()
        {
            var existingProduct = new Product { Id = 1, Name = "Apple", Price = 1.99m, Category = "Food", ProductCode = "A123", StockQuantity = 100, DateAdded = DateTime.UtcNow };
            var updateProduct = new UpdateProduct { Name = "Updated Apple", Price = 2.99m };

            _mockProductRepository.Setup(repo => repo.GetProductByIdAsync(1)).ReturnsAsync(existingProduct);
            _mockProductRepository.Setup(repo => repo.UpdateProductAsync(1, updateProduct)).ReturnsAsync(true);

            var result = await _productService.UpdateProductAsync(1, updateProduct);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task UpdateProductAsync_ReturnsFalse_WhenProductDoesNotExist()
        {
            var updateProduct = new UpdateProduct { Name = "Updated Apple", Price = 2.99m };

            _mockProductRepository.Setup(repo => repo.GetProductByIdAsync(999)).ReturnsAsync((Product)null);

            var result = await _productService.UpdateProductAsync(999, updateProduct);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteProductAsync_ReturnsTrue_WhenProductIsDeleted()
        {
            var existingProduct = new Product { Id = 1, Name = "Apple", Price = 1.99m, Category = "Food", ProductCode = "A123", StockQuantity = 100, DateAdded = DateTime.UtcNow };

            _mockProductRepository.Setup(repo => repo.GetProductByIdAsync(1)).ReturnsAsync(existingProduct);
            _mockProductRepository.Setup(repo => repo.DeleteProductAsync(1)).ReturnsAsync(true);

            var result = await _productService.DeleteProductAsync(1);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task DeleteProductAsync_ReturnsFalse_WhenProductDoesNotExist()
        {
            _mockProductRepository.Setup(repo => repo.GetProductByIdAsync(999)).ReturnsAsync((Product)null);

            var result = await _productService.DeleteProductAsync(999);

            Assert.That(result, Is.False);
        }
    }
}
