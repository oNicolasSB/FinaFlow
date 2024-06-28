using FinaFlow.API.Data;
using FinaFlow.Core.Common;
using FinaFlow.Core.Enums;
using FinaFlow.Core.Handlers;
using FinaFlow.Core.Models;
using FinaFlow.Core.Requests.Transactions;
using FinaFlow.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace FinaFlow.API.Handlers;

public class TransactionHandler : ITransactionHandler
{
    private readonly AppDbContext _context;
    public TransactionHandler(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        try
        {
            if (request is { Type: ETransactionType.Withdraw, Amount: > 0 })
                request.Amount *= -1;

            Transaction transaction = new()
            {
                Title = request.Title,
                Type = request.Type,
                Amount = request.Amount,
                CreatedAt = DateTime.Now,
                PaidOrReceivedAt = request.PaidOrReceivedAt,
                UserId = request.UserId,
                CategoryId = request.CategoryId
            };
            _ = await _context.Transactions.AddAsync(transaction);
            _ = await _context.SaveChangesAsync();
            return new Response<Transaction?>(transaction, 201, "Transaction created successfully");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Something went wrong while creating the transaction");
        }
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            var transaction = await _context
                .Transactions
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (transaction is null)
                return new Response<Transaction?>(null, 404, "Transaction not found");

            _ = _context.Transactions.Remove(transaction);
            _ = await _context.SaveChangesAsync();

            return new Response<Transaction?>(transaction);
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Something went wrong while deleting the transaction");
        }
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            Transaction? transaction = await _context.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

            return transaction == null
                ? new Response<Transaction?>(null, 404, "Transaction not found")
                : new Response<Transaction?>(transaction);
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Something went wrong while getting the transaction");
        }
    }

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
    {
        try
        {
            request.InitialDate ??= DateTime.Now.GetFirstDayOfMonth();
            request.FinalDate ??= DateTime.Now.GetLastDayOfMonth();
        }
        catch
        {
            return new PagedResponse<List<Transaction>?>(null, 500, "Something went wrong while getting the dates");
        }
        try
        {
            IOrderedQueryable<Transaction> query = _context
                .Transactions
                .AsNoTracking()
                .Where(x =>
                    x.UserId == request.UserId &&
                    x.CreatedAt >= request.InitialDate &&
                    x.CreatedAt <= request.FinalDate)
                .OrderByDescending(x => x.CreatedAt);

            List<Transaction> transactions = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            int count = await query.CountAsync();

            return new PagedResponse<List<Transaction>?>(transactions, count, request.PageNumber, request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Transaction>?>(null, 500, "Something went wrong while getting the transactions");
        }
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        try
        {
            if (request is { Type: ETransactionType.Withdraw, Amount: > 0 })
                request.Amount *= -1;
            var transaction = await _context
                .Transactions
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (transaction is null)
                return new Response<Transaction?>(null, 404, "Transaction not found");

            transaction.CategoryId = request.CategoryId;
            transaction.Amount = request.Amount;
            transaction.Title = request.Title;
            transaction.Type = request.Type;
            transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;

            _ = _context.Transactions.Update(transaction);
            _ = await _context.SaveChangesAsync();

            return new Response<Transaction?>(transaction);
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Something went wrong while updating the transaction");
        }
    }
}