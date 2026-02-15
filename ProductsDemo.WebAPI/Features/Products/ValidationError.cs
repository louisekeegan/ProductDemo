namespace ProductsDemo.WebAPI.Features.Products;

public sealed record ValidationError(string Field, string Message);