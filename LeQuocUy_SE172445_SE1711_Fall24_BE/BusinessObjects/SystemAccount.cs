using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BusinessObjects;

public partial class SystemAccount
{
    [Key]
    public short AccountId { get; set; }

    public string? AccountName { get; set; }

    public string? AccountEmail { get; set; }

    public int? AccountRole { get; set; }
    [JsonIgnore]
    public string? AccountPassword { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
