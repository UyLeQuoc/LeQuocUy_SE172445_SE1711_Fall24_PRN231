using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public partial class CategoryDTO
    {
        public short CategoryId { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        [StringLength(100, ErrorMessage = "Category Name cannot exceed 100 characters.")]
        public string CategoryName { get; set; } = null!;
        [Required(ErrorMessage = "Category Description is required.")]
        public string CategoryDesciption { get; set; } = null!;

        public short? ParentCategoryId { get; set; }

        public bool? IsActive { get; set; }

        public virtual ICollection<CategoryDTO> InverseParentCategory { get; set; } = new List<CategoryDTO>();

        public virtual ICollection<NewsArticleResponse> NewsArticles { get; set; } = new List<NewsArticleResponse>();

        public virtual CategoryDTO? ParentCategory { get; set; }
    }
}


