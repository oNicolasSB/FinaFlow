using System.Transactions;
using FinaFlow.Core;
using FinaFlow.API.Common.Api;
using FinaFlow.Core.Handlers;
using FinaFlow.Core.Requests.Transactions;
using FinaFlow.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FinaFlow.API.Endpoints.Transactions;
public class GetTransactionsByPeriodEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithName("Transactions: Get All")
            .WithSummary("Get all transactions")
            .WithDescription("Get all transactions")
            .WithOrder(5)
            .Produces<PagedResponse<List<Transaction>?>>();

    private static async Task<IResult> HandleAsync(
        ITransactionHandler handler,
        [FromQuery] DateTime? initialDate = null,
        [FromQuery] DateTime? finalDate = null,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetTransactionsByPeriodRequest
        {
            UserId = ApiConfiguration.UserId,
            PageNumber = pageNumber,
            PageSize = pageSize,
            InitialDate = initialDate,
            FinalDate = finalDate
        };

        var result = await handler.GetByPeriodAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}