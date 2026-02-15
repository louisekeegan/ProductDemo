using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace ProductsDemo.WebAPI.IntegrationTests;

using Microsoft.AspNetCore.Mvc.Testing;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    public CustomWebApplicationFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddScoped(_ => new NpgsqlConnection(_connectionString));
        });
    }
}
