using BusinessObjects;

namespace Services
{
    public interface ICategoryService
    {
        List<Category> GetCategories();
        Category GetCategoryById(short id);
        void CreateCategory(Category category);
        void UpdateCategory(Category category);
        void RemoveCategory(short id);
    }
}
