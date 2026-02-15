using FastEndpoints;

namespace ProductsDemo.WebAPI.Features.Products.GetAll;

public sealed class GetAllProductsEndpoint : EndpointWithoutRequest<List<GetAllProductsResponse>>
{
    private readonly IGetAllProductsService _service;

    public GetAllProductsEndpoint(IGetAllProductsService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Get("/products");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _service.GetAllAsync(ct);

        if (result.Status == GetAllProductsStatus.Failure)
        {
            AddError("Internal Server Error", result.Error.Message);
            await Send.ErrorsAsync(500, ct);
        }

        var response = result.Products!
            .Select(p => new GetAllProductsResponse(
                p.Id,
                p.Name,
                p.Price,
                p.Description,
                p.CreatedAt,
                p.UpdatedAt))
            .ToList();

        await Send.OkAsync(response, cancellation: ct);
    }
}