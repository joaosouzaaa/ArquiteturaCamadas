using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Tag;
using ArquiteturaCamadas.Domain.Entities;
using TestBuilders.BaseBuilders;

namespace TestBuilders
{
    public sealed class TagBuilder : BuilderBase
    {
        private string _tagName = GenerateRandomWord();
        private int _id = GenerateRandomNumber();

        public static TagBuilder NewObject() =>
            new TagBuilder();

        public Tag DomainBuild() =>
            new Tag()
            {
                Id = _id,
                Posts = new List<Post>(),
                TagName = _tagName
            };

        public TagResponse ResponseBuild() =>
            new TagResponse()
            {
                Id = _id,
                TagName = _tagName
            };

        public TagBuilder WithTagName(string tagName)
        {
            _tagName = tagName;

            return this;
        }
    }
}
