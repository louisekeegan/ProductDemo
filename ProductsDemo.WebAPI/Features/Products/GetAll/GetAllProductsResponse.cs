namespace ProductsDemo.WebAPI.Features.Products.GetAll;

public sealed record GetAllProductsResponse(
    long Id,
    string Name,
    decimal Price,
    string? Description,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
