using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Post;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Post;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Tag;
using ArquiteturaCamadas.Domain.Entities;
using Microsoft.AspNetCore.Http;
using TestBuilders.Helpers;

namespace TestBuilders
{
    public sealed class PostBuilder
    {
        private string _message = "carlos";
        private int _id = new Random().Next(1, 10000);
        private byte[] _imageBytes = { 0x32, 0x00, 0x1E, 0x00 };
        private List<Tag> _tagList = new List<Tag>();
        private IFormFile _image = UtilTools.BuildIFormFile();
        private List<int> _tagsIds = new List<int>();

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
                Image = _image,
                Message = _message,
                TagsIds = _tagsIds
            };

        public PostUpdateRequest UpdateRequestBuild() =>
            new PostUpdateRequest()
            {
                Id = _id,
                Image = _image,
                Message = _message,
                TagsIds = _tagsIds
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

        public PostBuilder WithTagsIds(List<int> tagsIds)
        {
            _tagsIds = tagsIds;

            return this;
        }

        public PostBuilder WithImage(IFormFile image)
        {
            _image = image;

            return this;
        }

        public PostBuilder WithId(int id)
        {
            _id = id;

            return this;
        }
    }
}
