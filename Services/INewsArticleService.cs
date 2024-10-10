using DTO;

namespace Services
{
    public interface INewsArticleService
    {
        List<NewsArticleDTO> GetNewsArticles();
        NewsArticleDTO GetNewsArticleById(string id);
        void CreateNewsArticle(NewsArticleDTO newsArticle);
        void UpdateNewsArticle(NewsArticleDTO newsArticle);
        void RemoveNewsArticle(string id);
    }
}
