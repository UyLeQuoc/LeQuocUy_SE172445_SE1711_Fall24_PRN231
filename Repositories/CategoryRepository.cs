using BusinessObjects;
using DataAccessObjects;

namespace Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public List<Category> GetCategories()
        {
            return CategoryDAO.Instance.GetCategories();
        }

        public Category GetCategoryById(short id)
        {
            return CategoryDAO.Instance.GetCategoryById(id);
        }

        public void AddCategory(Category category)
        {
            CategoryDAO.Instance.AddCategory(category);
        }

        public void UpdateCategory(Category category)
        {
            CategoryDAO.Instance.UpdateCategory(category);
        }

        public void DeleteCategory(short id)
        {
            CategoryDAO.Instance.DeleteCategory(id);
        }
    }
}
