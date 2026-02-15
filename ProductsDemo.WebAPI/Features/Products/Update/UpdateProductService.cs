namespace ProductsDemo.WebAPI.Features.Products.Update;

public interface IUpdateProductService
{
    Task<UpdateProductResult> UpdateAsync(long id, string name, decimal price, string? description,
        CancellationToken ct);
}

public sealed class UpdateProductService: IUpdateProductService
{
    private readonly IUpdateProductRepository _repository;

    public UpdateProductService(IUpdateProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<UpdateProductResult> UpdateAsync(long id, string name, decimal price, string? description, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(name))
            return UpdateProductResult.ValidationError(new ValidationError("Name", "Name is required"));

        if (price < 0)
            return UpdateProductResult.ValidationError(new ValidationError("Price", "Price cannot be negative"));

        try
        {
            var updated = await _repository.UpdateAsync(id, name, price, description, ct);
            if (updated is null)
                return UpdateProductResult.NotFound();

            return UpdateProductResult.Ok(updated);
        }
        catch (Exception ex)
        {
            return UpdateProductResult.Failure(ex.Message);
        }
    }
}
