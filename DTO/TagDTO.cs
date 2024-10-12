namespace DTO
{
    public partial class TagDTO
    {
        public int TagId { get; set; }

        public string? TagName { get; set; }

        public string? Note { get; set; }

        public virtual ICollection<NewsArticleResponse> NewsArticles { get; set; } = new List<NewsArticleResponse>();
    }
}
