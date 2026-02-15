namespace ProductsDemo.WebAPI.Features.Products.Create;

public class CreateProductResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string? Description { get; set; }
}