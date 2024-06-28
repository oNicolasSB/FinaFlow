using FinaFlow.API.Common.Api;
using FinaFlow.Core.Handlers;
using FinaFlow.Core.Models;
using FinaFlow.Core.Requests.Categories;
using FinaFlow.Core.Responses;

namespace FinaFlow.Core.Endpoints.Categories;

public class CreateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("Categories: Create")
            .WithSummary("Create a new category")
            .WithDescription("Create a new category")
            .WithOrder(1)
            .Produces<Response<Category?>>(201);

    private static async Task<IResult> HandleAsync(CreateCategoryRequest request, ICategoryHandler handler)
    {
        Response<Category?> response = await handler.CreateAsync(request);

        return response.IsSuccess
            ? TypedResults.Created($"v1/categories/{response.Data?.Id}", response)
            : Results.BadRequest(response);
    }
}