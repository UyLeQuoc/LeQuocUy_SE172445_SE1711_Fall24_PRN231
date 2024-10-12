using System.ComponentModel.DataAnnotations;

namespace BusinessObjects;

public partial class Category
{
    public short CategoryId { get; set; }

    [Required(ErrorMessage = "Category Name is required.")]
    [StringLength(100, ErrorMessage = "Category Name cannot exceed 100 characters.")]
    public string CategoryName { get; set; } = null!;
    [Required(ErrorMessage = "Category Description is required.")]
    public string CategoryDesciption { get; set; } = null!;

    public short? ParentCategoryId { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Category> InverseParentCategory { get; set; } = new List<Category>();

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();

    public virtual Category? ParentCategory { get; set; }
}
