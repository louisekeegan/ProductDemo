using FastEndpoints;
using FastEndpoints.Swagger;
using Npgsql;
using ProductsDemo.WebAPI.Database;
using ProductsDemo.WebAPI.Features.Products.Create;
using ProductsDemo.WebAPI.Features.Products.Delete;
using ProductsDemo.WebAPI.Features.Products.Get;
using ProductsDemo.WebAPI.Features.Products.GetAll;
using ProductsDemo.WebAPI.Features.Products.Update;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddFastEndpoints().SwaggerDocument();

builder.Services.AddScoped<NpgsqlConnection>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("productsdb");
    return new NpgsqlConnection(connectionString);
});

builder.Services.AddScoped<ICreateProductRepository, CreateProductRepository>();
builder.Services.AddScoped<ICreateProductService, CreateProductService>();

builder.Services.AddScoped<IGetAllProductsRepository, GetAllProductsRepository>();
builder.Services.AddScoped<IGetAllProductsService, GetAllProductsService>();

builder.Services.AddScoped<IGetProductRepository, GetProductRepository>();
builder.Services.AddScoped<IGetProductService, GetProductService>();

builder.Services.AddScoped<IUpdateProductRepository, UpdateProductRepository>();
builder.Services.AddScoped<IUpdateProductService, UpdateProductService>();

builder.Services.AddScoped<IDeleteProductRepository, DeleteProductRepository>();
builder.Services.AddScoped<IDeleteProductService, DeleteProductService>();

var app = builder.Build();

var connectionString = app.Configuration.GetConnectionString("productsdb");
if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine("Connection string is empty");
}
else
{
    Migrator.RunMigrations(connectionString);    
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseFastEndpoints().UseSwaggerGen();
app.MapDefaultEndpoints();

app.Run();