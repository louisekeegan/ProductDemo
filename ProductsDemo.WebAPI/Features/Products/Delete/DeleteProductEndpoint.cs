namespace ProductsDemo.WebAPI.Features.Products.Delete;

using FastEndpoints;

public sealed class DeleteProductEndpoint : Endpoint<DeleteProductRequest>
{
    private readonly IDeleteProductService _service;

    public DeleteProductEndpoint(IDeleteProductService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Delete("/products/{id}");
        AllowAnonymous();
        Summary(s => s.Summary = "Delete a product by ID");
        Tags("Products");
    }

    public override async Task HandleAsync(DeleteProductRequest req, CancellationToken ct)
    {
        var result = await _service.DeleteAsync(req.Id, ct);

        switch (result.Status)
        {
            case DeleteProductStatus.Ok:
                await Send.OkAsync($"Product {req.Id} was deleted", ct);
                break;

            case DeleteProductStatus.NotFound:
                await Send.NotFoundAsync(ct);
                break;

            case DeleteProductStatus.Failure:
                AddError("Internal Server Error", result.Error.Message);
                await Send.ErrorsAsync(500, ct);
                break;
        }
    }
}
