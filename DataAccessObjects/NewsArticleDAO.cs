﻿using BusinessObjects;
using DTO;
using Microsoft.EntityFrameworkCore;

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
            return context.NewsArticles.Include(c => c.Category).Include(t => t.Tags).Include(r => r.CreatedBy).ToList();
        }

        public NewsArticle GetNewsArticleById(string id)
        {
            var article = context.NewsArticles.Include(c => c.Category).Include(t => t.Tags).Include(r => r.CreatedBy).FirstOrDefault(na => na.NewsArticleId == id);
            if (article != null)
            {
                return article;
            }
            return null;
        }

        public void AddNewsArticle(NewsArticleDTO newsArticleDTO)
        {
            var newArticle = new NewsArticle
            {
                NewsArticleId = newsArticleDTO.NewsArticleId,
                NewsTitle = newsArticleDTO.NewsTitle,
                Headline = newsArticleDTO.Headline,
                NewsContent = newsArticleDTO.NewsContent,
                NewsSource = newsArticleDTO.NewsSource,
                CategoryId = newsArticleDTO.CategoryId,
                NewsStatus = newsArticleDTO.NewsStatus,
                CreatedById = newsArticleDTO.CreatedById,
                CreatedDate = DateTime.Now,
                Tags = context.Tags.Where(t => newsArticleDTO.TagIds.Contains(t.TagId)).ToList()
            };
            context.NewsArticles.Add(newArticle);
            context.SaveChanges();
        }

        public void UpdateNewsArticle(NewsArticleDTO newsArticleDTO)
        {
            var existingArticle = context.NewsArticles.FirstOrDefault(na => na.NewsArticleId == newsArticleDTO.NewsArticleId);
            if (existingArticle != null)
            {
                existingArticle.NewsTitle = newsArticleDTO.NewsTitle;
                existingArticle.Headline = newsArticleDTO.Headline;
                existingArticle.NewsContent = newsArticleDTO.NewsContent;
                existingArticle.CategoryId = newsArticleDTO.CategoryId;
                existingArticle.NewsStatus = newsArticleDTO.NewsStatus;
                existingArticle.ModifiedDate = DateTime.Now;

                existingArticle.Tags = context.Tags.Where(t => newsArticleDTO.TagIds.Contains(t.TagId)).ToList();
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
