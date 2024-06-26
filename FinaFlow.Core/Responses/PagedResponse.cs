using System.Text.Json.Serialization;

namespace FinaFlow.Core.Responses;

public abstract class PagedResponse<TData> : Response<TData>
{
    [JsonConstructor]
    public PagedResponse(TData? data, int totalCount, int currentPage = 1, int pageSize = Configuration.DefaultPageSize, string? message = null) : base(data)
    {
        Data = data;
        TotalCount = totalCount;
        CurrentPage = currentPage;
        PageSize = pageSize;
        Message = message;
    }
    public PagedResponse(TData data, int code = Configuration.DefaultStatusCode, string? message = null) : base(data, code, message) { }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; } = Configuration.DefaultPageSize;
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public int TotalCount { get; set; }
}