using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Tag;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Post;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Tag;
using ArquiteturaCamadas.Domain.Entities;
using TestBuilders.BaseBuilders;

namespace TestBuilders
{
    public sealed class TagBuilder : BuilderBase
    {
        private string _tagName = GenerateRandomWord();
        private int _id = GenerateRandomNumber();
        private List<Post> _postList = new List<Post>();

        public static TagBuilder NewObject() =>
            new TagBuilder();

        public Tag DomainBuild() =>
            new Tag()
            {
                Id = _id,
                Posts = _postList,
                TagName = _tagName
            };

        public TagResponse ResponseBuild() =>
            new TagResponse()
            {
                Id = _id,
                TagName = _tagName
            };

        public TagSaveRequest SaveRequestBuild() =>
            new TagSaveRequest()
            {
                TagName = _tagName
            };

        public TagUpdateRequest UpdateRequestBuild() =>
            new TagUpdateRequest()
            {
                Id = _id,
                TagName = _tagName
            };

        public TagPostsResponse PostsResponseBuild() =>
            new TagPostsResponse()
            {
                Id = _id,
                Posts = new List<PostResponse>(),
                TagName = _tagName
            };

        public TagBuilder WithTagName(string tagName)
        {
            _tagName = tagName;

            return this;
        }

        public TagBuilder WithPostList(List<Post> postList)
        {
            _postList = postList;

            return this;
        }
    }
}
