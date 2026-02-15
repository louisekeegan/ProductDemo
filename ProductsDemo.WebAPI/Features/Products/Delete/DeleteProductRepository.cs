using Dapper;
using Npgsql;

namespace ProductsDemo.WebAPI.Features.Products.Delete;

public interface IDeleteProductRepository
{
    /// <summary>
    /// Deletes a product by id. Returns true if a row was deleted, false if not found.
    /// </summary>
    Task<bool> DeleteAsync(long id, CancellationToken ct);
}

public sealed class DeleteProductRepository : IDeleteProductRepository
{
    private readonly NpgsqlConnection _connection;

    public DeleteProductRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken ct)
    {
        const string sql = "DELETE FROM products WHERE id = @Id;";
        var affectedRows = await _connection.ExecuteAsync(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
        return affectedRows > 0;
    }
}
