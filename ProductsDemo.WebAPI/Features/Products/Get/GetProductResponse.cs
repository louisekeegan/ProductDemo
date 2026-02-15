namespace ProductsDemo.WebAPI.Features.Products.Get;

public sealed record GetProductResponse(
    long Id,
    string Name,
    decimal Price,
    string? Description,
    DateTime CreatedAt,
    DateTime? UpdatedAt);