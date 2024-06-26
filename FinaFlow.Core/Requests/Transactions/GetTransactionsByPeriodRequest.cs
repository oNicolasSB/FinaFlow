namespace FinaFlow.Core.Requests.Transactions;

public class GetTransactionsByPeriodRequest : PagedRequest
{
    public DateTime? InitialDate { get; set; }
    public DateTime? FinalDate { get; set; }
}