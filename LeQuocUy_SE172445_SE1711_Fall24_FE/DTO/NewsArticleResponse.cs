namespace DTO
{
    public partial class NewsArticleResponse
    {
        public string? NewsArticleId { get; set; } = null!;

        public string? NewsTitle { get; set; }

        public string Headline { get; set; } = null!;

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public string? NewsContent { get; set; }

        public string? NewsSource { get; set; }

        public short? CategoryId { get; set; }

        public bool? NewsStatus { get; set; }

        public short? CreatedById { get; set; }

        public short? UpdatedById { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual CategoryDTO? Category { get; set; }

        public virtual SystemAccountDTO? CreatedBy { get; set; }

        public virtual ICollection<TagDTO> Tags { get; set; } = new List<TagDTO>();
    }

}
