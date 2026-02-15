using ProductsDemo.UI.Models;

namespace ProductsDemo.UI.Services;

using System.Net.Http.Json;

public class ProductService
{
    private readonly HttpClient _http;

    public ProductService(HttpClient http)
    {
        _http = http;
    }

    public Task<List<ProductModel>> GetAllAsync() =>
        _http.GetFromJsonAsync<List<ProductModel>>("/products")!;

    public Task<ProductModel?> GetByIdAsync(long id) =>
        _http.GetFromJsonAsync<ProductModel>($"/products/{id}");

    public Task<ProductModel?> CreateAsync(ProductModel product) =>
        _http.PostAsJsonAsync("/products", product)
            .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<ProductModel>()).Unwrap();

    public Task<ProductModel?> UpdateAsync(ProductModel product) =>
        _http.PutAsJsonAsync($"/products/{product.Id}", product)
            .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<ProductModel>()).Unwrap();

    public Task<bool> DeleteAsync(long id) =>
        _http.DeleteAsync($"/products/{id}")
            .ContinueWith(t => t.Result.IsSuccessStatusCode);
}
