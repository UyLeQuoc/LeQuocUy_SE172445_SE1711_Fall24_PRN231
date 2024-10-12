﻿using BusinessObjects;

namespace Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetCategories();
        Category GetCategoryById(short id);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(short id);
    }
}
