using BusinessObjects;
using DataAccessObjects;
using DTO;

namespace Repositories
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        public List<NewsArticle> GetNewsArticles()
        {
            return NewsArticleDAO.Instance.GetNewsArticles();
        }

        public NewsArticle GetNewsArticleById(string id)
        {
            return NewsArticleDAO.Instance.GetNewsArticleById(id);
        }

        public void AddNewsArticle(NewsArticle newsArticle)
        {
            NewsArticleDAO.Instance.AddNewsArticle(newsArticle);
        }

        public void UpdateNewsArticle(NewsArticleDTO newsArticle)
        {
            NewsArticleDAO.Instance.UpdateNewsArticle(newsArticle);
        }

        public void DeleteNewsArticle(string id)
        {
            NewsArticleDAO.Instance.DeleteNewsArticle(id);
        }
    }
}
