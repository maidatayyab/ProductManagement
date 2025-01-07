using Moq;
using NUnit.Framework;
using ProductManagement.Models;
using ProductManagement.DataAccess;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.Tests.DataAccess
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        private ProductRepository _productRepository;
        private SqliteConnection _connection;

        [SetUp]
        public void SetUp()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            var createTableCommand = _connection.CreateCommand();
            createTableCommand.CommandText = @"
                CREATE TABLE Products (
                    Id INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Price REAL NOT NULL,
                    Category TEXT NOT NULL,
                    ProductCode TEXT NOT NULL,
                    StockQuantity INTEGER NOT NULL,
                    DateAdded TEXT NOT NULL
                )";
            createTableCommand.ExecuteNonQuery();

            _productRepository = new ProductRepository(_connection);
        }

        [Test]
        public async Task GetAllProductsAsync_ReturnsAllProducts()
        {
            var insertDataCmd = _connection.CreateCommand();
            insertDataCmd.CommandText = @"
                INSERT INTO Products (Name, Price, Category, ProductCode, StockQuantity, DateAdded)
                VALUES ('Apple', 1.99, 'Food', 'A123', 100, '2025-01-01'),
                       ('Smartphone', 499.99, 'Electronics', 'S456', 50, '2025-01-01')";
            insertDataCmd.ExecuteNonQuery();

            var result = await _productRepository.GetAllProductsAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Has.Exactly(1).Property("Name").EqualTo("Apple"));
            Assert.That(result, Has.Exactly(1).Property("Name").EqualTo("Smartphone"));
        }

        [Test]
        public async Task GetProductByIdAsync_ReturnsProduct()
        {
            var insertDataCmd = _connection.CreateCommand();
            insertDataCmd.CommandText = @"
                INSERT INTO Products (Name, Price, Category, ProductCode, StockQuantity, DateAdded)
                VALUES ('Apple', 1.99, 'Food', 'A123', 100, '2025-01-01')";
            insertDataCmd.ExecuteNonQuery();

            var result = await _productRepository.GetProductByIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Apple"));
        }

        [Test]
        public async Task AddProductAsync_AddsProduct()
        {
            var product = new Product
            {
                Name = "Laptop",
                Price = 79,
                Category = "Electronics",
                ProductCode = "L123",
                StockQuantity = 30
            };

            var result = await _productRepository.AddProductAsync(product);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task UpdateProductAsync_UpdatesProduct()
        {
            var insertDataCmd = _connection.CreateCommand();
            insertDataCmd.CommandText = @"
                INSERT INTO Products (Name, Price, Category, ProductCode, StockQuantity, DateAdded)
                VALUES ('Apple', 1.99, 'Food', 'A123', 100, '2025-01-01')";
            insertDataCmd.ExecuteNonQuery();

            var updateProduct = new UpdateProduct
            {
                Name = "Apple Updated",
                Price = 29
            };

            var result = await _productRepository.UpdateProductAsync(1, updateProduct);

            Assert.That(result, Is.True);

            var updatedProduct = await _productRepository.GetProductByIdAsync(1);
            Assert.That(updatedProduct.Name, Is.EqualTo("Apple Updated"));
            Assert.That(updatedProduct.Price, Is.EqualTo(29));
        }

        [Test]
        public async Task DeleteProductAsync_DeletesProduct()
        {
            var insertDataCmd = _connection.CreateCommand();
            insertDataCmd.CommandText = @"
                INSERT INTO Products (Name, Price, Category, ProductCode, StockQuantity, DateAdded)
                VALUES ('Apple', 1.99, 'Food', 'A123', 100, '2025-01-01')";
            insertDataCmd.ExecuteNonQuery();

            var result = await _productRepository.DeleteProductAsync(1);

            Assert.That(result, Is.True);

            var deletedProduct = await _productRepository.GetProductByIdAsync(1);
            Assert.That(deletedProduct, Is.Null);
        }

        [TearDown]
        public void TearDown()
        {
            _connection.Close();
        }
    }
}
