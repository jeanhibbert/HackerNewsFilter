namespace HackerNewsFilter.Structured.Mediator.Models;

public class NewsItem
{
    public string by { get; set; }
    public int id { get; set; }
    public List<int> kids { get; set; }
    public int parent { get; set; }
    public string text { get; set; }
    public int time { get; set; }
    public string type { get; set; }
}

