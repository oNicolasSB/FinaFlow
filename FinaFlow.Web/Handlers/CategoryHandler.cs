using System.IO.Pipelines;
using System.Net.Http.Json;
using FinaFlow.Core.Handlers;
using FinaFlow.Core.Models;
using FinaFlow.Core.Requests.Categories;
using FinaFlow.Core.Responses;

namespace FinaFlow.Web.Handlers;

public class CategoryHandler : ICategoryHandler
{
    private readonly HttpClient _httpClient;
    public CategoryHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(WebConfiguration.HttpClientName);
    }
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        HttpResponseMessage result = await _httpClient.PostAsJsonAsync("v1/categories", request);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>() ?? new Response<Category?>(null, 400, "Something went wrong while creating the category");
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        HttpResponseMessage result = await _httpClient.DeleteAsync($"v1/categories/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Category?>>() ?? new Response<Category?>(null, 400, "Something went wrong while deleting the category");
    }

    public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request)
        => await _httpClient.GetFromJsonAsync<PagedResponse<List<Category>?>>("v1/categories") ?? new PagedResponse<List<Category>?>(null, 400, "Something went wrong while getting the categories");

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
        => await _httpClient.GetFromJsonAsync<Response<Category?>>($"v1/categories/{request.Id}") ?? new Response<Category?>(null, 400, "Something went wrong while getting the category");

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        HttpResponseMessage result = await _httpClient.PutAsJsonAsync($"v1/categories/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>() ?? new Response<Category?>(null, 400, "Something went wrong while updating the category");
    }
}