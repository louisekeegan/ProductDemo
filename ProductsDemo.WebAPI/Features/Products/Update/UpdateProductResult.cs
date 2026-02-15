namespace ProductsDemo.WebAPI.Features.Products.Update;

public enum UpdateProductStatus
{
    Ok,
    NotFound,
    ValidationError,
    Failure
}

public sealed record UpdateProductResult(
    UpdateProductStatus Status,
    ProductEntity? Product,
    ValidationError? ValidationErrors,
    InfrastructureError? Error)
{
    public static UpdateProductResult Ok(ProductEntity product)
        => new(UpdateProductStatus.Ok, product, null, null);

    public static UpdateProductResult NotFound()
        => new(UpdateProductStatus.NotFound, null, null, null);

    public static UpdateProductResult ValidationError(ValidationError error)
        => new(UpdateProductStatus.ValidationError, null, error, null);

    public static UpdateProductResult Failure(string message)
        => new(UpdateProductStatus.Failure, null, null, new InfrastructureError(message));
}
