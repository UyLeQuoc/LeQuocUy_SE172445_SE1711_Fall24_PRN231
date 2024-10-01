using BusinessObjects;

namespace Services
{
    public interface INewsArticleService
    {
        List<NewsArticle> GetNewsArticles();
        NewsArticle GetNewsArticleById(string id);
        void CreateNewsArticle(NewsArticle newsArticle);
        void UpdateNewsArticle(NewsArticle newsArticle);
        void RemoveNewsArticle(string id);
    }
}
