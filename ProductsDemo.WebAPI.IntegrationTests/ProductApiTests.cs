using System.Net;
using ProductsDemo.WebAPI.Features.Products.Create;
using ProductsDemo.WebAPI.Features.Products.Get;
using ProductsDemo.WebAPI.Features.Products.GetAll;
using ProductsDemo.WebAPI.Features.Products.Update;
using System.Net.Http.Json;
using Xunit;

namespace ProductsDemo.WebAPI.IntegrationTests;

public class ProductsApiTests : IClassFixture<PostgresFixture>
{
    private readonly HttpClient _client;

    public ProductsApiTests(PostgresFixture fixture)
    {
        var factory = new CustomWebApplicationFactory(fixture.ConnectionString);
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateAndGetProduct_ShouldReturnProduct()
    {
        var createReq = new CreateProductRequest()
        {
            Name = "Integration Test Product",
            Price = 15,
            Description = "Test product"
        };
        
        var createResponse = await _client.PostAsJsonAsync("/products", createReq);
        createResponse.EnsureSuccessStatusCode();

        var created = await createResponse.Content.ReadFromJsonAsync<CreateProductResponse>();
        long id = created.Id;

        var getResponse = await _client.GetAsync($"/products/{id}");
        getResponse.EnsureSuccessStatusCode();

        var product = await getResponse.Content.ReadFromJsonAsync<GetProductResponse>();

        Assert.Equal(createReq.Name, product.Name);
        Assert.Equal(createReq.Price, product.Price);
    }
  
    
    [Fact]
    public async Task CreateManyAndGetAllProducts_ShouldReturnProducts()
    {
        var createReq1 = new CreateProductRequest()
        {
            Name = "Integration Test Product",
            Price = 15,
            Description = "Test product"
        };
        
        var createResponse1 = await _client.PostAsJsonAsync("/products", createReq1);
        createResponse1.EnsureSuccessStatusCode();
        var created1 = await createResponse1.Content.ReadFromJsonAsync<CreateProductResponse>();

        var createReq2 = new CreateProductRequest()
        {
            Name = "Integration Test Product 2",
            Price = 20,
            Description = "Test product 2"
        };
        
        var createResponse2 = await _client.PostAsJsonAsync("/products", createReq2);
        createResponse2.EnsureSuccessStatusCode();
        var created2 = await createResponse2.Content.ReadFromJsonAsync<CreateProductResponse>();


        var getResponse = await _client.GetAsync($"/products");
        getResponse.EnsureSuccessStatusCode();

        var products = await getResponse.Content.ReadFromJsonAsync<List<GetAllProductsResponse>>();

        Assert.True(products.Exists(p => p.Id == created1.Id));
        Assert.True(products.Exists(p => p.Id == created2.Id));
        
    }
    
    [Fact]
    public async Task CreateUpdateAndGetProduct_ShouldReturnProduct()
    {
        var createReq = new CreateProductRequest
        {
            Name = "Integration Test Product",
            Price = 15,
            Description = "Test product"
        };
        
        var createResponse = await _client.PostAsJsonAsync("/products", createReq);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<CreateProductResponse>();
        long id = created.Id;

        var updatedPrice = createReq.Price * 2;

        var updateRequest = new UpdateProductRequest("Updated product name", updatedPrice, "Updated description");
        
        var updateResponse = await _client.PutAsJsonAsync($"/products/{id}", updateRequest);
        updateResponse.EnsureSuccessStatusCode();
        var product = await updateResponse.Content.ReadFromJsonAsync<UpdateProductResponse>();

        Assert.Equal(updateRequest.Name, product.Name);
        Assert.Equal(updateRequest.Price, product.Price);
        Assert.Equal(updateRequest.Description, product.Description);
    }
    
    [Fact]
    public async Task CreateDeleteAndGetProduct_ShouldReturnNoProduct()
    {
        var createReq = new CreateProductRequest
        {
            Name = "Integration Test Product",
            Price = 15,
            Description = "Test product"
        };
        
        var createResponse = await _client.PostAsJsonAsync("/products", createReq);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<CreateProductResponse>();
        long id = created.Id;
        
        var route = $"/products/{id}";
        var deleteResponse = await _client.DeleteAsync(route);
        deleteResponse.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"/products/{id}");
        Assert.True(getResponse.StatusCode == HttpStatusCode.NotFound);
    }
}
