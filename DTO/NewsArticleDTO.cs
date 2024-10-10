﻿namespace DTO
{
    public class NewsArticleDTO
    {
        public string NewsArticleId { get; set; } = null!;
        public string? NewsTitle { get; set; }
        public string Headline { get; set; } = null!;
        public string? NewsContent { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? NewsSource { get; set; }
        public short? CategoryId { get; set; }
        public bool? NewsStatus { get; set; }
        public short? CreatedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<int> TagIds { get; set; } = new List<int>();
    }
}

