namespace ProductsDemo.WebAPI.Features.Products.GetAll;

public interface IGetAllProductsService
{
    Task<GetAllProductsResult> GetAllAsync(CancellationToken ct);
}

public sealed class GetAllProductsService: IGetAllProductsService
{
    private readonly IGetAllProductsRepository _repository;

    public GetAllProductsService(IGetAllProductsRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetAllProductsResult> GetAllAsync(CancellationToken ct)
    {
        try
        {
            var products = await _repository.GetAllAsync(ct);
            return GetAllProductsResult.Ok(products);
        }
        catch (Exception ex)
        {
            return GetAllProductsResult.Failure(ex.Message);
        }
    }
}
