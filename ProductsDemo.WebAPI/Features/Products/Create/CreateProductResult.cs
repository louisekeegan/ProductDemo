namespace ProductsDemo.WebAPI.Features.Products.Create;

public enum CreateProductStatus
{
    Ok,
    ValidationError,
    Failure
}

public sealed record CreateProductResult
{
    public CreateProductStatus Status { get; }
    public ProductEntity? Product { get; }
    public ValidationError? ValidationErrors { get; }
    public InfrastructureError? InfrastructureError { get; }

    private CreateProductResult(
        CreateProductStatus status,
        ProductEntity? product,
        ValidationError? valError,
        InfrastructureError? infrastructureError)
    {
        Status = status;
        Product = product;
        ValidationErrors = valError;
        InfrastructureError = infrastructureError;
    }

    public static CreateProductResult Ok(ProductEntity product)
        => new(CreateProductStatus.Ok, product, null, null);

    public static CreateProductResult ValidationError(ValidationError error)
        => new(CreateProductStatus.ValidationError, null, error, null);

    public static CreateProductResult Failure(string message)
        => new(CreateProductStatus.Failure, null, null, new InfrastructureError(message));
}



