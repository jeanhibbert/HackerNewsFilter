using DotnetDocsShow.Api.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using HackerNewsFilter.Api;
using HackerNewsFilter.Api.Services;

var builder = WebApplication.CreateBuilder();
builder.Services.AddMemoryCache();
builder.Services.AddOutputCache();
builder.Services.AddFastEndpoints()
    .AddResponseCaching()
    .SwaggerDocument(o => o.EnableJWTBearerAuth = false);

builder.Services.AddHttpClient(Constants.HackerNewsBaseUrlName, client =>
{
    client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/");
});

builder.Services.AddTransient<IHackerNewsService, HackerNewsService>();
builder.Services.AddTransient<IHackerNewsClient, HackerNewsClient>();

var app = builder.Build();

app.UseOutputCache();
app.UseResponseCaching()
    .UseFastEndpoints()
    .UseSwaggerGen();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
    app.UseReDoc(options =>
    {
        options.Path = "/redoc";
    });
}

app.Run();

public partial class Program { }