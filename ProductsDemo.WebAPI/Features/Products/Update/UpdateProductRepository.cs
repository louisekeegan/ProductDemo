namespace ProductsDemo.WebAPI.Features.Products.Update;

using Dapper;
using Npgsql;

public interface IUpdateProductRepository
{
    Task<ProductEntity?> UpdateAsync(long id, string name, decimal price, string? description, CancellationToken ct);
}


public sealed class UpdateProductRepository : IUpdateProductRepository
{
    private readonly NpgsqlConnection _connection;

    public UpdateProductRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<ProductEntity?> UpdateAsync(long id, string name, decimal price, string? description, CancellationToken ct)
    {
        const string sql = @"
            UPDATE products
            SET name = @Name,
                price = @Price,
                description = @Description,
                updated_at = NOW()
            WHERE id = @Id
            RETURNING id, name, price, description, created_at AS CreatedAt, updated_at AS UpdatedAt;";

        var updated = await _connection.QueryFirstOrDefaultAsync<ProductEntity>(
            new CommandDefinition(sql, new { Id = id, Name = name, Price = price, Description = description }, cancellationToken: ct));

        return updated;
    }
}
