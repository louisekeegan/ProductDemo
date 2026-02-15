namespace ProductsDemo.WebAPI.Features.Products.Get;

public interface IGetProductService
{
    Task<GetProductResult> GetByIdAsync(long id, CancellationToken ct);
}

public sealed class GetProductService: IGetProductService
{
    private readonly IGetProductRepository _repository;

    public GetProductService(IGetProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetProductResult> GetByIdAsync(long id, CancellationToken ct)
    {
        try
        {
            var product = await _repository.GetByIdAsync(id, ct);

            if (product is null)
                return GetProductResult.NotFound();

            return GetProductResult.Ok(product);
        }
        catch (Exception ex)
        {
            return GetProductResult.Failure(ex.Message);
        }
    }
}
