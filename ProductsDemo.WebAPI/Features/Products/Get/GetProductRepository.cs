using Dapper;
using Npgsql;

namespace ProductsDemo.WebAPI.Features.Products.Get;

public interface IGetProductRepository
{
    Task<ProductEntity?> GetByIdAsync(long id, CancellationToken ct);
}


public sealed class GetProductRepository : IGetProductRepository
{
    private readonly NpgsqlConnection _connection;

    public GetProductRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<ProductEntity?> GetByIdAsync(long id, CancellationToken ct)
    {
        const string sql = @"
            SELECT id, name, price, description, created_at AS CreatedAt
            FROM products
            WHERE id = @Id;";

        return await _connection.QueryFirstOrDefaultAsync<ProductEntity>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
    }
}
