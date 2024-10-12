using BusinessObjects;

namespace Services
{
    public interface ITagService
    {
        List<Tag> GetTags();
        Tag GetTagById(int id);
        void CreateTag(Tag tag);
        void UpdateTag(Tag tag);
        void RemoveTag(int id);
    }
}
