namespace ProductsDemo.WebAPI.Features.Products;

public class ProductEntity
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public decimal Price { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
}