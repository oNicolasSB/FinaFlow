using FinaFlow.API.Data;
using FinaFlow.API.Handlers;
using FinaFlow.Core;
using FinaFlow.Core.Handlers;
using Microsoft.EntityFrameworkCore;

namespace FinaFlow.API.Common.Api;

public static class BuilderExtensions
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
        Configuration.FrontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? string.Empty;
    }

    public static void AddDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(s =>
        {
            //Category -> FinaFlow.Core.Models.Category
            s.CustomSchemaIds(x => x.FullName);
        });
    }

    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
    }

    //CORS -> Cross-Origin Resource Sharing
    public static void AddCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options => options.AddPolicy(ApiConfiguration.CorsPolicyName,
            policy => policy
                .WithOrigins([
                    Configuration.FrontendUrl,
                    Configuration.BackendUrl
                ])
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                ));
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
        builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
    }
}