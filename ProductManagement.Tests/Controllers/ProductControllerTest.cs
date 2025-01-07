using Moq;
using NUnit.Framework;
using ProductManagement.Models;
using ProductManagement.BusinessLogic;
using ProductManagementApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagementApi.Tests
{
    [TestFixture]
    public class ProductControllerTests
    {
        private Mock<IProductService> _mockProductService;
        private ProductController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductController(_mockProductService.Object);
        }

        [Test]
        public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
        {
            var mockProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Apple", Price = 1.99m, Category = "Food", ProductCode = "A123", StockQuantity = 100, DateAdded = DateTime.UtcNow },
                new Product { Id = 2, Name = "Smartphone", Price = 499.99m, Category = "Electronics", ProductCode = "S456", StockQuantity = 50, DateAdded = DateTime.UtcNow }
            };

            _mockProductService.Setup(service => service.GetProductsAsync()).ReturnsAsync(mockProducts);

            var result = await _controller.GetProducts();

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var returnValue = okResult.Value as List<Product>;
            Assert.That(returnValue, Is.Not.Null);
            Assert.That(returnValue.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetProduct_ReturnsOkResult_WithProduct()
        {
            var mockProduct = new Product { Id = 1, Name = "Apple", Price = 1.99m, Category = "Food", ProductCode = "A123", StockQuantity = 100, DateAdded = DateTime.UtcNow };

            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(mockProduct);

            var result = await _controller.GetProduct(1);

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var returnValue = okResult.Value as Product;
            Assert.That(returnValue, Is.Not.Null);
            Assert.That(returnValue.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task GetProduct_ReturnsNotFoundResult_WhenProductDoesNotExist()
        {
            _mockProductService.Setup(service => service.GetProductByIdAsync(999)).ReturnsAsync((Product)null);

            var result = await _controller.GetProduct(999);

            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task PostProduct_ReturnsCreatedAtAction_WhenProductIsCreated()
        {
            var newProduct = new Product { Id = 1, Name = "Apple", Price = 1.99m, Category = "Food", ProductCode = "A123", StockQuantity = 100, DateAdded = DateTime.UtcNow };

            _mockProductService.Setup(service => service.AddProductAsync(newProduct)).ReturnsAsync(true);

            var result = await _controller.PostProduct(newProduct);

            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.That(createdAtActionResult, Is.Not.Null);
            Assert.That(createdAtActionResult.ActionName, Is.EqualTo("GetProduct"));
            Assert.That(createdAtActionResult.RouteValues["id"], Is.EqualTo(newProduct.Id));
        }

        [Test]
        public async Task PostProduct_ReturnsBadRequest_WhenProductCannotBeCreated()
        {
            var newProduct = new Product { Id = 1, Name = "Apple", Price = 1.99m, Category = "Food", ProductCode = "A123", StockQuantity = 100, DateAdded = DateTime.UtcNow };

            _mockProductService.Setup(service => service.AddProductAsync(newProduct)).ReturnsAsync(false);

            var result = await _controller.PostProduct(newProduct);

            Assert.That(result.Result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task PutProduct_ReturnsNoContent_WhenProductIsUpdated()
        {
            var updateProduct = new UpdateProduct { Name = "Updated Apple", Price = 2.99m };

            var existingProduct = new Product { Id = 1, Name = "Apple", Price = 1.99m, Category = "Food", ProductCode = "A123", StockQuantity = 100, DateAdded = DateTime.UtcNow };
            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(existingProduct);
            _mockProductService.Setup(service => service.UpdateProductAsync(1, updateProduct)).ReturnsAsync(true);

            var result = await _controller.PutProduct(1, updateProduct);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteProduct_ReturnsNoContent_WhenProductIsDeleted()
        {
            var existingProduct = new Product { Id = 1, Name = "Apple", Price = 1.99m, Category = "Food", ProductCode = "A123", StockQuantity = 100, DateAdded = DateTime.UtcNow };
            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(existingProduct);
            _mockProductService.Setup(service => service.DeleteProductAsync(1)).ReturnsAsync(true);

            var result = await _controller.DeleteProduct(1);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            _mockProductService.Setup(service => service.GetProductByIdAsync(999)).ReturnsAsync((Product)null);

            var result = await _controller.DeleteProduct(999);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
