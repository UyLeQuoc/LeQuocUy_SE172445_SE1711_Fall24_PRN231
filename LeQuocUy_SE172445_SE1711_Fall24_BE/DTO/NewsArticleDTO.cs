using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class NewsArticleDTO
    {
        public string? NewsArticleId { get; set; } = null!;

        [Required(ErrorMessage = "News Title is required.")]
        [StringLength(200, ErrorMessage = "News Title cannot exceed 200 characters.")]
        public string? NewsTitle { get; set; }

        [Required(ErrorMessage = "Headline is required.")]
        [StringLength(500, ErrorMessage = "Headline cannot exceed 500 characters.")]
        public string Headline { get; set; } = null!;

        [Required(ErrorMessage = "News Content is required.")]
        [MinLength(50, ErrorMessage = "News Content must be at least 50 characters long.")]
        public string? NewsContent { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(200, ErrorMessage = "News Source cannot exceed 200 characters.")]
        public string? NewsSource { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public short? CategoryId { get; set; }

        public bool? NewsStatus { get; set; }

        public short? CreatedById { get; set; }

        public short? UpdatedById { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Required(ErrorMessage = "At least one tag is required.")]
        public List<int> TagIds { get; set; } = new List<int>();
    }
}
