using FastEndpoints;

namespace ProductsDemo.WebAPI.Features.Products.Update;

public sealed class UpdateProductEndpoint
    : Endpoint<UpdateProductRequest, UpdateProductResponse>
{
    private readonly IUpdateProductService _service;

    public UpdateProductEndpoint(IUpdateProductService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Put("/products/{id}");
        AllowAnonymous();
        Summary(s => s.Summary = "Update an existing product by ID");
        Tags("Products");
    }

    public override async Task HandleAsync(UpdateProductRequest req, CancellationToken ct)
    {
        var id = Route<long>("id"); 

        var result = await _service.UpdateAsync(id, req.Name, req.Price, req.Description, ct);

        switch (result.Status)
        {
            case UpdateProductStatus.Ok:
                var p = result.Product!;
                await Send.OkAsync(new UpdateProductResponse(
                    p.Id, p.Name, p.Price, p.Description, p.CreatedAt, p.UpdatedAt.Value), cancellation: ct);
                break;

            case UpdateProductStatus.NotFound:
                await Send.NotFoundAsync(ct);
                break;

            case UpdateProductStatus.ValidationError:
                AddError(result.ValidationErrors.Field, result.ValidationErrors.Message);
                await Send.ErrorsAsync(400, ct);
                break;

            case UpdateProductStatus.Failure:
                AddError("Internal Server Error", result.Error.Message);
                await Send.ErrorsAsync(500, ct);
                break;
        }
    }
}
