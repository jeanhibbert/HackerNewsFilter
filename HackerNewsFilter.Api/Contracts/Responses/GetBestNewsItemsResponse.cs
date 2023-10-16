using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HackerNewsFilter.Api.Contracts.Responses;

public class GetBestNewsItemsResponse
{
    [JsonPropertyName("bestNewsItems")]
    public IEnumerable<NewsItemResult> BestNewsItems { get; set; }
}
