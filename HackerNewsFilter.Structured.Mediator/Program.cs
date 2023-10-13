using DotnetDocsShow.Structured.Mediator.Handlers;
using DotnetDocsShow.Structured.Mediator.Services;
using HackerNewsFilter.Structured.Mediator.Models;
using HackerNewsFilter.Structured.Mediator.Services;
using MediatR;

var builder = WebApplication.CreateBuilder();

builder.Services.AddMediatR(typeof(NewsItem));
builder.Services.AddHttpClient("HackerNews", client =>
{
    client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/");
});

builder.Services.AddTransient<IHackerNewsService, HackerNewsService>();
builder.Services.AddTransient<IHackerNewsClient, HackerNewsClient>();

var app = builder.Build();

app.MapGet("news", async (IMediator mediator) => await mediator.Send(new GetBestItemsRequest()));
app.MapGet("/news/{id}", async (IMediator mediator, long id) => await mediator.Send(new GetNewsItemByIdRequest(id)));

app.Run();

public partial class Program { }