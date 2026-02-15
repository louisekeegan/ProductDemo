namespace ProductsDemo.WebAPI.Features.Products.Delete;

public enum DeleteProductStatus
{
    Ok,
    NotFound,
    Failure
}

public sealed record DeleteProductResult(
    DeleteProductStatus Status,
    InfrastructureError? Error)
{
    public static DeleteProductResult Ok()
        => new(DeleteProductStatus.Ok, null);

    public static DeleteProductResult NotFound()
        => new(DeleteProductStatus.NotFound, null);

    public static DeleteProductResult Failure(string message)
        => new(DeleteProductStatus.Failure, new InfrastructureError(message));
}
