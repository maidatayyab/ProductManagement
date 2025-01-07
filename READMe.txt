Prerequisites
Make sure you have the following installed on system:

.NET SDK
Node.js and npm

Getting Started
Follow these steps to run the application after cloning the repository:

Step 1: Clone the Repository

Step 2: Set Up the Backend
1. Navigate to the backend project directory:
2. Restore the .NET dependencies:
	dotnet restore
3. Apply database migrations:
	dotnet ef database update
4. Run the Backend:
	dotnet run
By default, the backend will be running at http://localhost:5000. You can access the API on this port.

Step 3: Set Up the Frontend
1. Navigate to the frontend project directory:
	npm install
2. Run the Frontend:
Start the frontend application by running:
	npm start
By default, the frontend will be running at http://localhost:3000.

Step 4: Test the Application

Open browser and go to http://localhost:3000.
The React app should be running and should be able to communicate with the backend API (http://localhost:5000) for any data-related requests.


API Documentation
Base URL
bash
Copy code
http://localhost:44348/api
Endpoints
1. Get All Products
Method: GET
Endpoint: /products
Description: Fetch all products from the database.
Response Example:
[
  {
    "id": 1,
    "name": "Apple",
    "price": 1.99,
    "category": "Food",
    "productCode": "A123",
    "stockQuantity": 100,
    "dateAdded": "2025-01-01T00:00:00"
  },
  {
    "id": 2,
    "name": "Smartphone",
    "price": 499.99,
    "category": "Electronics",
    "productCode": "S456",
    "stockQuantity": 50,
    "dateAdded": "2025-01-01T00:00:00"
  }
]
2. Get Product by ID
Method: GET
Endpoint: /products/{id}
Description: Get a specific product by ID.
Response Example:
{
  "id": 1,
  "name": "Apple",
  "price": 1.99,
  "category": "Food",
  "productCode": "A123",
  "stockQuantity": 100,
  "dateAdded": "2025-01-01T00:00:00"
}
3. Create Product
Method: POST
Endpoint: /products
Description: Create a new product.
Request Example:
{
  "name": "Laptop",
  "price": 79.99,
  "category": "Electronics",
  "productCode": "L123",
  "stockQuantity": 30
}
Response Example:
{
  "id": 1,
  "name": "Laptop",
  "price": 79.99,
  "category": "Electronics",
  "productCode": "L123",
  "stockQuantity": 30,
  "dateAdded": "2025-01-01T00:00:00"
}
4. Update Product
Method: PUT
Endpoint: /products/{id}
Description: Update a product by ID.
Request Example:
{
  "name": "Updated Laptop",
  "price": 89.99
}
Response Example:
{
  "id": 1,
  "name": "Updated Laptop",
  "price": 89.99,
  "category": "Electronics",
  "productCode": "L123",
  "stockQuantity": 30,
  "dateAdded": "2025-01-01T00:00:00"
}
5. Delete Product
Method: DELETE
Endpoint: /products/{id}
Description: Delete a product by ID.
Response Example:
{
  "message": "Product deleted successfully"
}
Error Responses
400 Bad Request: The request is invalid or missing required data.
404 Not Found: The requested resource (product) was not found.
500 Internal Server Error: Something went wrong on the server.