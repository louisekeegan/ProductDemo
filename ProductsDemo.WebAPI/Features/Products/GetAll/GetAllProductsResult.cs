namespace ProductsDemo.WebAPI.Features.Products.GetAll;

public enum GetAllProductsStatus
{
    Ok,
    Failure
}

public sealed record GetAllProductsResult(
    GetAllProductsStatus Status,
    IReadOnlyCollection<ProductEntity>? Products,
    InfrastructureError? Error)
{
    public static GetAllProductsResult Ok(
        IReadOnlyCollection<ProductEntity> products)
        => new(GetAllProductsStatus.Ok, products, null);

    public static GetAllProductsResult Failure(string message)
        => new(GetAllProductsStatus.Failure, null,
            new InfrastructureError(message));
}
