using ProductManagement.DataAccess;
using ProductManagement.BusinessLogic;
using ProductManagement.Models;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product Management API",
        Version = "v1"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Management API V1");
        c.RoutePrefix = string.Empty;  // Makes Swagger UI available at / instead of /swagger
    });
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    SeedData(context);

    app.UseHttpsRedirection();
    app.UseCors("AllowReactApp");
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

    static void SeedData(ApplicationDbContext context)
    {
        if (!context.Products.Any())  // Only seed if the Products table is empty
        {
            context.Products.AddRange(
                new Product
                {
                    Id = 1,
                    Category = "Food",
                    Name = "Apple",
                    ProductCode = "A123",
                    Price = 1.99m,
                    StockQuantity = 100,
                    DateAdded = DateTime.UtcNow
                },
                new Product
                {
                    Id = 2,
                    Category = "Electronics",
                    Name = "Smartphone",
                    ProductCode = "S456",
                    Price = 499.99m,
                    StockQuantity = 50,
                    DateAdded = DateTime.UtcNow
                }
            );
            context.SaveChanges();
        }
    }
}
