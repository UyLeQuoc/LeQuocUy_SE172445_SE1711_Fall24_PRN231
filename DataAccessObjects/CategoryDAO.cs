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
            return context.Categories.Where(category => category.IsActive == true).ToList();
        }

        public Category GetCategoryById(short id)
        {
            return context.Categories.FirstOrDefault(category => category.CategoryId == id);
        }

        public void AddCategory(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
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
        }

        public void DeleteCategory(short id)
        {
            var category = context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category != null && !context.NewsArticles.Any(na => na.CategoryId == id))
            {
                context.Categories.Remove(category);
                context.SaveChanges();
            }
        }
    }
}
