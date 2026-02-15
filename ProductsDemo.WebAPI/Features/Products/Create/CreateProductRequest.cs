namespace ProductsDemo.WebAPI.Features.Products.Create;

public class CreateProductRequest
{
    public string Name { get; set; }
    
    public decimal Price { get; set; }
    
    public string? Description { get; set; }
}