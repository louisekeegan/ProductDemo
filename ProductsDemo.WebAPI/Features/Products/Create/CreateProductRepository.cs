using Dapper;
using Npgsql;

namespace ProductsDemo.WebAPI.Features.Products.Create;

public interface ICreateProductRepository
{
    Task<long> CreateProductAsync(string name, decimal price, string? description, CancellationToken token);
}

public class CreateProductRepository: ICreateProductRepository
{
    private NpgsqlConnection _connection;

    public CreateProductRepository(NpgsqlConnection connection)
    {
        ArgumentNullException.ThrowIfNull(connection);
        _connection = connection;
    }
    
    public async Task<long> CreateProductAsync(string name, decimal price, string? description, CancellationToken token)
    {
        return await CreateAsync(name, price, description);
    }

    private async Task<long> CreateAsync(string name, decimal price, string? description)
    {
        // TODO consider retries
        var created = DateTime.UtcNow;
        
        const string sql = @"
            INSERT INTO products (name, price, description, created_at)
            VALUES (@Name, @Price, @Description, @Created)
            RETURNING id;";


        return await _connection.ExecuteScalarAsync<long>(sql,
            new { Name = name, Price = price, Description = description, Created = created });
        
    }
}