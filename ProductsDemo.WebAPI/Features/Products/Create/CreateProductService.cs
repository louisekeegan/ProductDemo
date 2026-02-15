namespace ProductsDemo.WebAPI.Features.Products.Create;

public interface ICreateProductService
{
    Task<CreateProductResult> CreateProductAsync(string name, decimal price, string? description, CancellationToken token);
}

public class CreateProductService: ICreateProductService
{
    private ICreateProductRepository _repository;
    
    public CreateProductService(ICreateProductRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    public async Task<CreateProductResult> CreateProductAsync(string name, decimal price, string? description, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(name))
            return CreateProductResult.ValidationError(new ValidationError("Name", "Name is required"));

        if (price < 0)
            return CreateProductResult.ValidationError(new  ValidationError("Price", "Price is required"));
        
        try
        {
            var id = await _repository.CreateProductAsync(name, price, description, token);

            var entity = new ProductEntity
            {
                Id = id,
                Name = name,
                Price = price,
                Description = description,
                CreatedAt = DateTime.UtcNow
            };

            return CreateProductResult.Ok(entity);
        }
        catch (Exception ex)
        {
            return CreateProductResult.Failure(ex.Message);
        }
    }
}