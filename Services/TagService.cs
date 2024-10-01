using BusinessObjects;
using Repositories;

namespace Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository tagRepository;

        public TagService(ITagRepository repository)
        {
            this.tagRepository = repository;
        }

        public List<Tag> GetTags()
        {
            return tagRepository.GetTags();
        }

        public Tag GetTagById(int id)
        {
            return tagRepository.GetTagById(id);
        }

        public void CreateTag(Tag tag)
        {
            tagRepository.AddTag(tag);
        }

        public void UpdateTag(Tag tag)
        {
            tagRepository.UpdateTag(tag);
        }

        public void RemoveTag(int id)
        {
            tagRepository.DeleteTag(id);
        }
    }
}
