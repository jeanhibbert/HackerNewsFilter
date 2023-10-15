using FastEndpoints;
using FluentValidation;
using HackerNewsFilter.Api.Contracts.Requests;

namespace HackerNewsFilter.Api.Validators;

public class GetBestNewsItemsValidator : Validator<GetBestNewsItemsRequest>
{
    public GetBestNewsItemsValidator()
    {
        RuleFor(x => x.FetchCount)
            .GreaterThan(0)
            .WithMessage($"{nameof(GetBestNewsItemsRequest.FetchCount)} must be greater than zero");
    }
}