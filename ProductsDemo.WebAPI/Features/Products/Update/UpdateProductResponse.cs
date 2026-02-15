namespace ProductsDemo.WebAPI.Features.Products.Update;


public sealed record UpdateProductResponse(
    long Id,
    string Name,
    decimal Price,
    string? Description,
    DateTime CreatedAt,
    DateTime UpdatedAt);