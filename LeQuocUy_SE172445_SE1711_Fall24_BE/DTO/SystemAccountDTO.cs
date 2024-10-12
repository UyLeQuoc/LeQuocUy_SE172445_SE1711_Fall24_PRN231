using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public partial class SystemAccountDTO
    {
        [Key]
        public short AccountId { get; set; }
        [Required]
        public string? AccountName { get; set; }
        [Required]
        public string? AccountEmail { get; set; }
        [Required]
        public int? AccountRole { get; set; }
        [Required]
        public string? AccountPassword { get; set; }

        public virtual ICollection<NewsArticleResponse> NewsArticles { get; set; } = new List<NewsArticleResponse>();
    }
}

