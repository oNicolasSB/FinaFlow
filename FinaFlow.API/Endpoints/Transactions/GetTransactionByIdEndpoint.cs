using System.Transactions;
using FinaFlow.API.Common.Api;
using FinaFlow.Core.Handlers;
using FinaFlow.Core.Requests.Transactions;
using FinaFlow.Core.Responses;

namespace FinaFlow.API.Endpoints.Transactions;
public class GetTransactionByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", HandleAsync)
            .WithName("Transactions: Get By Id")
            .WithSummary("Get a transaction by id")
            .WithDescription("Get a transaction by id")
            .WithOrder(4)
            .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandleAsync(
        ITransactionHandler handler,
        long id)
    {
        var request = new GetTransactionByIdRequest
        {
            UserId = ApiConfiguration.UserId,
            Id = id
        };

        var result = await handler.GetByIdAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}