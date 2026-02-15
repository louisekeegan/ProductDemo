namespace ProductsDemo.WebAPI.Features.Products.Update;

public sealed record UpdateProductRequest(
    string Name,
    decimal Price,
    string? Description);
