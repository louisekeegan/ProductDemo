namespace ProductsDemo.WebAPI.Features.Products.Get;

public enum GetProductByIdStatus
{
    Ok,
    NotFound,
    Failure
}

public sealed record GetProductResult(
    GetProductByIdStatus Status,
    ProductEntity? Product,
    InfrastructureError? Error)
{
    public static GetProductResult Ok(ProductEntity product)
        => new(GetProductByIdStatus.Ok, product, null);

    public static GetProductResult NotFound()
        => new(GetProductByIdStatus.NotFound, null, null);

    public static GetProductResult Failure(string message)
        => new(GetProductByIdStatus.Failure, null, new InfrastructureError(message));
}
