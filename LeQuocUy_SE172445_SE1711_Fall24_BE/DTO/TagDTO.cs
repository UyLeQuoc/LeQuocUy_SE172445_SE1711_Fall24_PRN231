using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public partial class TagDTO
    {
        public int TagId { get; set; }

        [Required(ErrorMessage = "Tag Name is required.")]
        [StringLength(100, ErrorMessage = "Tag Name cannot exceed 100 characters.")]
        public string TagName { get; set; }

        [Required(ErrorMessage = "Tag Note is required.")]
        [StringLength(200, ErrorMessage = "Note cannot exceed 200 characters.")]
        public string Note { get; set; }

        public virtual ICollection<NewsArticleResponse>? NewsArticles { get; set; } = new List<NewsArticleResponse>();
    }
}
