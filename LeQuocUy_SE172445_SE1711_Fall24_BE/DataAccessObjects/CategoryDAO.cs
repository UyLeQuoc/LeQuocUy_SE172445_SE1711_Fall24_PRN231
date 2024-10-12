using BusinessObjects;

namespace DataAccessObjects
{
    public class CategoryDAO
    {
        private static CategoryDAO instance = null;
        private readonly FunewsManagementFall2024Context context;

        private CategoryDAO()
        {
            context = new FunewsManagementFall2024Context();
        }

        public static CategoryDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CategoryDAO();
                }
                return instance;
            }
        }

        public List<Category> GetCategories()
        {
            return context.Categories.ToList();
        }

        public Category GetCategoryById(short id)
        {
            return context.Categories.FirstOrDefault(category => category.CategoryId == id);
        }

        public void AddCategory(Category category)
        {
            try
            {
                var categories = GetCategories();

                if (categories.Any(c => c.CategoryName == category.CategoryName))
                {
                    throw new InvalidOperationException("Category name already exists.");
                }

                context.Categories.Add(category);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to add category: {e.Message}", e);
            }
        }

        public void UpdateCategory(Category category)
        {
            var existingCategory = context.Categories.FirstOrDefault(c => c.CategoryId == category.CategoryId);
            if (existingCategory != null)
            {
                existingCategory.CategoryName = category.CategoryName;
                existingCategory.CategoryDesciption = category.CategoryDesciption;
                existingCategory.IsActive = category.IsActive;
                existingCategory.ParentCategoryId = category.ParentCategoryId;
                context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Category not found.");
            }
        }

        public void DeleteCategory(short id)
        {
            var category = context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                throw new InvalidOperationException("Category not found.");
            }

            bool isCategoryUsedInArticles = context.NewsArticles.Any(na => na.CategoryId == id);

            if (isCategoryUsedInArticles)
            {
                throw new InvalidOperationException("Cannot delete category associated with news articles.");
            }

            context.Categories.Remove(category);
            context.SaveChanges();
        }
    }
}
