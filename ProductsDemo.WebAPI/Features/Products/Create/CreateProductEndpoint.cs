using FastEndpoints;

namespace ProductsDemo.WebAPI.Features.Products.Create;

public class CreateProductEndpoint: Endpoint<CreateProductRequest, CreateProductResponse>
{
    private ICreateProductService? _service;
    
    public CreateProductEndpoint(ICreateProductService service)
    {
        ArgumentNullException.ThrowIfNull(service);
        _service = service;
    }
    
    public override void Configure()
    {
        Post("/products");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
    {
        var result = await _service.CreateProductAsync(req.Name, req.Price, req.Description, ct);

        switch (result.Status)
        {
            case CreateProductStatus.Ok:
                var response = AsResponse(result);
                await Send.OkAsync(response, ct);
                break;

            case CreateProductStatus.ValidationError:
                AddError(result.ValidationErrors.Field, result.ValidationErrors.Message);
                await Send.ErrorsAsync(400, ct);
                break;

            case CreateProductStatus.Failure:
                AddError("Internal Server Error", result.InfrastructureError.Message);
                await Send.ErrorsAsync(500, ct);
                break;
        }
    }

    private CreateProductResponse AsResponse(CreateProductResult result)
    {
        return new CreateProductResponse
        {
            Id = result.Product.Id,
            Name = result.Product.Name,
            Price = result.Product.Price,
            Description = result.Product.Description,
        };
    }
}