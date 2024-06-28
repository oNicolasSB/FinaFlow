using FinaFlow.API;
using FinaFlow.API.Common.Api;
using FinaFlow.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddDatabase();
builder.AddCors();
builder.AddDocumentation();
builder.AddServices();

var app = builder.Build();

if(app.Environment.IsDevelopment())
    app.ConfigureDevEnvironment();

app.UseCors(ApiConfiguration.CorsPolicyName);
app.MapEndpoints();
app.UseHttpsRedirection();

app.Run();
