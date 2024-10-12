using BusinessObjects;
using Repositories;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository repository)
        {
            this.categoryRepository = repository;
        }

        public List<Category> GetCategories()
        {
            return categoryRepository.GetCategories();
        }

        public Category GetCategoryById(short id)
        {
            return categoryRepository.GetCategoryById(id);
        }

        public void CreateCategory(Category category)
        {
            categoryRepository.AddCategory(category);
        }

        public void UpdateCategory(Category category)
        {
            categoryRepository.UpdateCategory(category);
        }

        public void RemoveCategory(short id)
        {
            categoryRepository.DeleteCategory(id);
        }
    }
}
