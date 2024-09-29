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
            context.Tags.Add(tag);
            context.SaveChanges();
        }

        public void UpdateTag(Tag tag)
        {
            var existingTag = context.Tags.FirstOrDefault(t => t.TagId == tag.TagId);
            if (existingTag != null)
            {
                existingTag.TagName = tag.TagName;
                existingTag.Note = tag.Note;
                context.SaveChanges();
            }
        }

        public void DeleteTag(int id)
        {
            var tag = context.Tags.FirstOrDefault(t => t.TagId == id);
            if (tag != null)
            {
                context.Tags.Remove(tag);
                context.SaveChanges();
            }
        }
    }
}
