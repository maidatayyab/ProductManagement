using ProductManagement.Models;
using Microsoft.Data.Sqlite;  // Use the correct namespace for SQLite
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.DataAccess
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;
        private readonly SqliteConnection _connection;

        public ProductRepository(string connectionString = "Data Source=ProductManagement.db")
        {
            _connectionString = connectionString;

            _connection = new SqliteConnection(_connectionString);
        }

        public ProductRepository(SqliteConnection connection)
        {
            _connection = connection;
        }

        private async Task InitializeDatabaseAsync()
        {
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Products (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Price REAL NOT NULL,
                    Category TEXT NOT NULL,
                    ProductCode TEXT NOT NULL,
                    StockQuantity INTEGER NOT NULL,
                    DateAdded TEXT NOT NULL
                )";

            using (var command = new SqliteCommand(createTableQuery, _connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();
            string query = "SELECT Id, Name, Price, Category, ProductCode, StockQuantity, DateAdded FROM Products";

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            await InitializeDatabaseAsync();

            using (var command = new SqliteCommand(query, _connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        products.Add(new Product
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Price = reader.GetDecimal(2),
                            Category = reader.GetString(3),
                            ProductCode = reader.GetString(4),
                            StockQuantity = reader.GetInt32(5),
                            DateAdded = reader.GetDateTime(6),
                        });
                    }
                }
            }

            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            Product product = null;
            string query = "SELECT Id, Name, Price, Category, ProductCode, StockQuantity, DateAdded FROM Products WHERE Id = @Id";

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            await InitializeDatabaseAsync();

            using (var command = new SqliteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        product = new Product
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Price = reader.GetDecimal(2),
                            Category = reader.GetString(3),
                            ProductCode = reader.GetString(4),
                            StockQuantity = reader.GetInt32(5),
                            DateAdded = reader.GetDateTime(6),
                        };
                    }
                }
            }

            return product;
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            string query = "INSERT INTO Products (Name, Price, Category, ProductCode, StockQuantity, DateAdded) VALUES (@Name, @Price, @Category, @ProductCode, @StockQuantity, @DateAdded)";

            try
            {
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    await _connection.OpenAsync();
                }

                await InitializeDatabaseAsync();

                using (var command = new SqliteCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Category", product.Category);
                    command.Parameters.AddWithValue("@ProductCode", product.ProductCode);
                    command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                    command.Parameters.AddWithValue("@DateAdded", DateTime.UtcNow);
                    await command.ExecuteNonQueryAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateProductAsync(int id, UpdateProduct product)
        {
            string query = "UPDATE Products SET Name = @Name, Price = @Price WHERE Id = @Id";
            try
            {
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    await _connection.OpenAsync();
                }

                await InitializeDatabaseAsync();

                using (var command = new SqliteCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            string query = "DELETE FROM Products WHERE Id = @Id";

            try
            {
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    await _connection.OpenAsync();
                }

                await InitializeDatabaseAsync();

                using (var command = new SqliteCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
