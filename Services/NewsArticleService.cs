using DTO;
using Repositories;

namespace Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository newsArticleRepository;

        public NewsArticleService(INewsArticleRepository repository)
        {
            this.newsArticleRepository = repository;
        }

        public List<NewsArticleDTO> GetNewsArticles()
        {
            return newsArticleRepository.GetNewsArticles();
        }

        public NewsArticleDTO GetNewsArticleById(string id)
        {
            return newsArticleRepository.GetNewsArticleById(id);
        }

        public void CreateNewsArticle(NewsArticleDTO newsArticle)
        {
            newsArticleRepository.AddNewsArticle(newsArticle);
        }

        public void UpdateNewsArticle(NewsArticleDTO newsArticle)
        {
            newsArticleRepository.UpdateNewsArticle(newsArticle);
        }

        public void RemoveNewsArticle(string id)
        {
            newsArticleRepository.DeleteNewsArticle(id);
        }
    }
}
