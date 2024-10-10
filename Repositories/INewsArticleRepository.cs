using DTO;

namespace Repositories
{
    public interface INewsArticleRepository
    {
        List<NewsArticleDTO> GetNewsArticles();
        NewsArticleDTO GetNewsArticleById(string id);
        void AddNewsArticle(NewsArticleDTO newsArticle);
        void UpdateNewsArticle(NewsArticleDTO newsArticle);
        void DeleteNewsArticle(string id);
    }
}
