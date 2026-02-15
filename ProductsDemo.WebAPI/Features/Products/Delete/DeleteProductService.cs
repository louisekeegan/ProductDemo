namespace ProductsDemo.WebAPI.Features.Products.Delete;

public interface IDeleteProductService
{
    Task<DeleteProductResult> DeleteAsync(long id, CancellationToken ct);
}

public sealed class DeleteProductService: IDeleteProductService
{
    private readonly IDeleteProductRepository _repository;

    public DeleteProductService(IDeleteProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeleteProductResult> DeleteAsync(long id, CancellationToken ct)
    {
        try
        {
            var deleted = await _repository.DeleteAsync(id, ct);

            return deleted ? DeleteProductResult.Ok() : DeleteProductResult.NotFound();
        }
        catch (Exception ex)
        {
            return DeleteProductResult.Failure(ex.Message);
        }
    }
}
