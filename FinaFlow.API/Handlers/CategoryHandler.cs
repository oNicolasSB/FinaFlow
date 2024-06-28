using FinaFlow.API.Data;
using FinaFlow.Core.Handlers;
using FinaFlow.Core.Models;
using FinaFlow.Core.Requests.Categories;
using FinaFlow.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace FinaFlow.API.Handlers;

public class CategoryHandler : ICategoryHandler
{
    private readonly AppDbContext _context;
    public CategoryHandler(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        try
        {
            Category category = new()
            {
                UserId = request.UserId,
                Title = request.Title,
                Description = request.Description
            };

            _ = await _context.Categories.AddAsync(category);
            _ = await _context.SaveChangesAsync();
            return new Response<Category?>(category, 201, "Category created successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Response<Category?>(null, 500, "Something went wrong while creating the category");
        }

    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            Category? category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

            if (category == null)
                return new Response<Category?>(null, 404, "Category not found");

            _ = _context.Categories.Remove(category);
            _ = await _context.SaveChangesAsync();

            return new Response<Category?>(category, 200, "Category deleted successfully");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Something went wrong while deleting the category");
        }
    }

    public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request)
    {
        try
        {
            IOrderedQueryable<Category> query = _context
                .Categories
                .AsNoTracking()
                .Where(c => c.UserId == request.UserId)
                .OrderBy(c => c.Title);

            List<Category> categories = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            int count = await query.CountAsync();

            return new PagedResponse<List<Category>?>(categories, count, request.PageNumber, request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Category>?>(null, 500, "Something went wrong while getting the categories");
        }
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            Category? category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

            return category == null
                ? new Response<Category?>(null, 404, "Category not found")
                : new Response<Category?>(category);
        }
        catch
        {
            return new Response<Category?>(null, 500, "Something went wrong while getting the category");
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            Category? category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

            if (category == null)
                return new Response<Category?>(null, 404, "Category not found");

            category.Title = request.Title;
            category.Description = request.Description;
            _ = await _context.SaveChangesAsync();

            return new Response<Category?>(category, 200, "Category updated successfully");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Something went wrong while updating the category");
        }
    }
}