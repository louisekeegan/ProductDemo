using ProductsDemo.WebAPI.Database;
using Npgsql;
using Testcontainers.PostgreSql;

namespace ProductsDemo.WebAPI.IntegrationTests;

public class PostgresFixture : IAsyncLifetime
{
    public PostgreSqlContainer PostgresContainer { get; private set; } = 
        new PostgreSqlBuilder()
            .WithDatabase("testdb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    public string ConnectionString => PostgresContainer.GetConnectionString();

    public async Task InitializeAsync()
    {
        await PostgresContainer.StartAsync();

        using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();
        Migrator.RunMigrations(ConnectionString);
    }

    public async Task DisposeAsync()
    {
        await PostgresContainer.StopAsync();
    }
}
