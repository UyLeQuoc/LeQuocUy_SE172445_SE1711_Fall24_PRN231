using BusinessObjects;
using DTO;

namespace Repositories
{
    public interface INewsArticleRepository
    {
        List<NewsArticle> GetNewsArticles();
        NewsArticle GetNewsArticleById(string id);
        void AddNewsArticle(NewsArticle newsArticle);
        void UpdateNewsArticle(NewsArticleDTO newsArticle);
        void DeleteNewsArticle(string id);
    }
}
