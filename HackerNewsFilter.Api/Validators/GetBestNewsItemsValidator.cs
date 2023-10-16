using FastEndpoints;
using FluentValidation;
using HackerNewsFilter.Api.Contracts.Requests;

namespace HackerNewsFilter.Api.Validators;

public class GetBestNewsItemsValidator : Validator<GetBestNewsItemsRequest>
{
    public GetBestNewsItemsValidator()
    {
        RuleFor(x => x.limit)
            .GreaterThan(0)
            .WithMessage($"{nameof(GetBestNewsItemsRequest.limit)} must be greater than zero");
    }
}