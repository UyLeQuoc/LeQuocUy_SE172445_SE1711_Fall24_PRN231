using BusinessObjects;

namespace Repositories
{
    public interface ITagRepository
    {
        List<Tag> GetTags();
        Tag GetTagById(int id);
        void AddTag(Tag tag);
        void UpdateTag(Tag tag);
        void DeleteTag(int id);
    }
}
