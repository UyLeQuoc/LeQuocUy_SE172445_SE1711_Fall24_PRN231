using BusinessObjects;

namespace DataAccessObjects
{
    public class TagDAO
    {
        private static TagDAO instance = null;
        private readonly FunewsManagementFall2024Context context;

        private TagDAO()
        {
            context = new FunewsManagementFall2024Context();
        }

        public static TagDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TagDAO();
                }
                return instance;
            }
        }

        public List<Tag> GetTags()
        {
            return context.Tags.ToList();
        }

        public Tag GetTagById(int id)
        {
            return context.Tags.FirstOrDefault(tag => tag.TagId == id);
        }
        public void AddTag(Tag tag)
        {
            try
            {
                if (context.Tags.Any(t => t.TagName == tag.TagName))
                {
                    throw new InvalidOperationException("Tag already exists.");
                }

                context.Tags.Add(tag);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding tag: {ex.Message}", ex);
            }
        }

        public void UpdateTag(Tag updatedTag)
        {
            try
            {
                var existingTag = context.Tags.FirstOrDefault(t => t.TagId == updatedTag.TagId);
                if (existingTag == null)
                {
                    throw new InvalidOperationException("Tag not found.");
                }

                existingTag.TagName = updatedTag.TagName;
                existingTag.Note = updatedTag.Note;

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating tag: {ex.Message}", ex);
            }
        }
        public void DeleteTag(int id)
        {
            try
            {
                var tag = context.Tags.FirstOrDefault(t => t.TagId == id);
                if (tag == null)
                {
                    throw new InvalidOperationException("Tag not found.");
                }

                if (context.NewsArticles.Any(na => na.Tags.Any(t => t.TagId == id)))
                {
                    throw new InvalidOperationException("Cannot delete tag associated with news articles.");
                }

                context.Tags.Remove(tag);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting tag: {ex.Message}", ex);
            }
        }
    }
}
