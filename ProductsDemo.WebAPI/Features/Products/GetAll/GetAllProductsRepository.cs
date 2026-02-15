using Dapper;
using Npgsql;

namespace ProductsDemo.WebAPI.Features.Products.GetAll;

public interface IGetAllProductsRepository
{
    Task<IReadOnlyCollection<ProductEntity>> GetAllAsync(CancellationToken ct);
}

public sealed class GetAllProductsRepository : IGetAllProductsRepository
{
    private readonly NpgsqlConnection _connection;

    public GetAllProductsRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<IReadOnlyCollection<ProductEntity>> GetAllAsync(CancellationToken ct)
    {
        const string sql = @"
            SELECT id, name, price, description, created_at AS CreatedAt
            FROM products
            ORDER BY created_at DESC;";

        var products = await _connection.QueryAsync<ProductEntity>(
            new CommandDefinition(sql, cancellationToken: ct));

        return products.ToList();
    }
}
