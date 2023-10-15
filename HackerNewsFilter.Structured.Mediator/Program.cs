using DotnetDocsShow.Structured.Mediator.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using HackerNewsFilter.Structured.Mediator.Middleware;
using HackerNewsFilter.Structured.Mediator.Services;

var builder = WebApplication.CreateBuilder();

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();
builder.Services.AddMemoryCache();

builder.Services.AddHttpClient(HackerNewsFilter.Structured.Mediator.Config.HackerNewsBaseUrlName, client =>
{
    client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/");
});

builder.Services.AddTransient<IHackerNewsService, HackerNewsService>();
builder.Services.AddTransient<IHackerNewsClient, HackerNewsClient>();

var app = builder.Build();

app.UseMiddleware<ValidationExceptionMiddleware>();
app.UseFastEndpoints(x =>
{
    //x.ErrorResponseBuilder = (failures, _) =>
    //{
    //    return new ValidationFailureResponse
    //    {
    //        Errors = failures.Select(y => y.ErrorMessage).ToList()
    //    };
    //};
});

app.UseOpenApi();
app.UseSwaggerUi3(s => s.ConfigureDefaults());

app.Run();

public partial class Program { }