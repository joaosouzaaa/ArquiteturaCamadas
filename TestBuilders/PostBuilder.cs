using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Post;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Post;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Tag;
using ArquiteturaCamadas.Domain.Entities;
using TestBuilders.BaseBuilders;
using TestBuilders.Helpers;

namespace TestBuilders
{
    public sealed class PostBuilder : BuilderBase
    {
        private string _message = GenerateRandomWord();
        private int _id = GenerateRandomNumber();
        private byte[] _imageBytes = { 0x32, 0x00, 0x1E, 0x00 };
        private List<Tag> _tagList = new List<Tag>();

        public static PostBuilder NewObject() => new PostBuilder();

        public Post DomainBuild() =>
            new Post()
            {
                Id = _id,
                ImageBytes = _imageBytes,
                Message = _message,
                Tags = _tagList
            };

        public PostSaveRequest SaveRequestBuild() =>
            new PostSaveRequest()
            {
                Image = UtilTools.BuildIFormFile(),
                Message = _message,
                TagsIds = new List<int>()
            };

        public PostUpdateRequest UpdateRequestBuild() =>
            new PostUpdateRequest()
            {
                Id = _id,
                Image = UtilTools.BuildIFormFile(),
                Message = _message,
                TagsIds = new List<int>()
            };

        public PostResponse ResponseBuild() =>
            new PostResponse()
            {
                Id = _id,
                Message = _message
            };

        public PostTagsResponse TagsResponseBuild() =>
            new PostTagsResponse()
            {
                Id = _id,
                ImageBytes = _imageBytes,
                Message = _message,
                Tags = new List<TagResponse>()
            };

        public PostBuilder WithMessage(string message)
        {
            _message = message; 

            return this;    
        }

        public PostBuilder WithTagList(List<Tag> tagList)
        {
            _tagList = tagList;

            return this;
        }
    }
}
