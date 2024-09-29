using BusinessObjects;
using DataAccessObjects;

namespace Repositories
{
    public class TagRepository : ITagRepository
    {
        public List<Tag> GetTags()
        {
            return TagDAO.Instance.GetTags();
        }

        public Tag GetTagById(int id)
        {
            return TagDAO.Instance.GetTagById(id);
        }

        public void AddTag(Tag tag)
        {
            TagDAO.Instance.AddTag(tag);
        }

        public void UpdateTag(Tag tag)
        {
            TagDAO.Instance.UpdateTag(tag);
        }

        public void DeleteTag(int id)
        {
            TagDAO.Instance.DeleteTag(id);
        }
    }
}
