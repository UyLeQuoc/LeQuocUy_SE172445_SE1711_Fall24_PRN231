using BusinessObjects;
using DTO;

namespace Services
{
    public interface INewsArticleService
    {
        List<NewsArticle> GetNewsArticles();
        NewsArticle GetNewsArticleById(string id);
        void CreateNewsArticle(NewsArticleDTO newsArticle);
        void UpdateNewsArticle(NewsArticleDTO newsArticle);
        void RemoveNewsArticle(string id);
    }
}
