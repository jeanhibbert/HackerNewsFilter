﻿namespace HackerNewsFilter.Api.Models;

public class NewsItem
{
    public string by { get; set; }
    public int id { get; set; }
    public string title { get; set; }
    public string url { get; set; }
    public int score { get; set; }
    public int time { get; set; }
    public int descendants { get; set; }
}

