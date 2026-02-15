namespace ProductsDemo.WebAPI.Features.Products.Get;

using FastEndpoints;

public sealed class GetProductByIdEndpoint : Endpoint<GetProductRequest, GetProductResponse>
{
    private readonly IGetProductService _service;

    public GetProductByIdEndpoint(IGetProductService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Get("/products/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetProductRequest req, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(req.Id, ct);

        switch (result.Status)
        {
            case GetProductByIdStatus.Ok:
                var p = result.Product!;
                await Send.OkAsync(new GetProductResponse(
                        p.Id, p.Name, p.Price, p.Description, p.CreatedAt, p.UpdatedAt),
                    cancellation: ct);
                break;

            case GetProductByIdStatus.NotFound:
                await Send.NotFoundAsync(ct);
                break;

            case GetProductByIdStatus.Failure:
                AddError("Internal Server Error", result.Error.Message);
                await Send.ErrorsAsync(500, ct);
                break;
        }
    }
}
