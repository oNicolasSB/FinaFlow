using System.Transactions;
using FinaFlow.API.Common.Api;
using FinaFlow.Core.Handlers;
using FinaFlow.Core.Requests.Transactions;
using FinaFlow.Core.Responses;

namespace FinaFlow.API.Endpoints.Transactions;
public class DeleteTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id}", HandleAsync)
            .WithName("Transactions: Delete")
            .WithSummary("Delete a transaction")
            .WithDescription("Delete a transaction")
            .WithOrder(3)
            .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandleAsync(
        ITransactionHandler handler,
        long id)
    {
        var request = new DeleteTransactionRequest
        {
            UserId = ApiConfiguration.UserId,
            Id = id
        };

        var result = await handler.DeleteAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}