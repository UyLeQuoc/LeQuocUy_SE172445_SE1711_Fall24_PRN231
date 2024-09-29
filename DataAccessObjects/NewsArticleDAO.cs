using BusinessObjects;

namespace DataAccessObjects
{
    public class NewsArticleDAO
    {
        private static NewsArticleDAO instance = null;
        private readonly FunewsManagementFall2024Context context;

        private NewsArticleDAO()
        {
            context = new FunewsManagementFall2024Context();
        }

        public static NewsArticleDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NewsArticleDAO();
                }
                return instance;
            }
        }

        public List<NewsArticle> GetNewsArticles()
        {
            return context.NewsArticles.Where(article => article.NewsStatus == true).ToList();
        }

        public NewsArticle GetNewsArticleById(string id)
        {
            return context.NewsArticles.FirstOrDefault(article => article.NewsArticleId == id);
        }

        public void AddNewsArticle(NewsArticle newsArticle)
        {
            context.NewsArticles.Add(newsArticle);
            context.SaveChanges();
        }

        public void UpdateNewsArticle(NewsArticle newsArticle)
        {
            var existingArticle = context.NewsArticles.FirstOrDefault(na => na.NewsArticleId == newsArticle.NewsArticleId);
            if (existingArticle != null)
            {
                existingArticle.NewsTitle = newsArticle.NewsTitle;
                existingArticle.Headline = newsArticle.Headline;
                existingArticle.NewsContent = newsArticle.NewsContent;
                existingArticle.CategoryId = newsArticle.CategoryId;
                existingArticle.NewsStatus = newsArticle.NewsStatus;
                context.SaveChanges();
            }
        }

        public void DeleteNewsArticle(string id)
        {
            var newsArticle = context.NewsArticles.FirstOrDefault(na => na.NewsArticleId == id);
            if (newsArticle != null)
            {
                context.NewsArticles.Remove(newsArticle);
                context.SaveChanges();
            }
        }
    }
}
