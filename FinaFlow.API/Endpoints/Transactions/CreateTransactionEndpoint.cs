using System.Transactions;
using FinaFlow.API.Common.Api;
using FinaFlow.Core.Handlers;
using FinaFlow.Core.Requests.Transactions;
using FinaFlow.Core.Responses;

namespace FinaFlow.API.Endpoints.Transactions;
public class CreateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("Transactions: Create")
            .WithSummary("Create a transaction")
            .WithDescription("Create a transaction")
            .WithOrder(1)
            .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandleAsync(ITransactionHandler handler, CreateTransactionRequest request)
    {
        request.UserId = ApiConfiguration.UserId;
        var result = await handler.CreateAsync(request);
        return result.IsSuccess
            ? TypedResults.Created($"/{result.Data?.Id}", result)
            : TypedResults.BadRequest(result.Data);
    }
}