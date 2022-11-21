using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Post;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Post;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using ArquiteturaCamadas.Domain.Entities;
using TestBuilders;

namespace UnitTests.AutoMapperTests
{
    public sealed class PostAutoMapperTests
    {
        public PostAutoMapperTests()
        {
            AutoMapperSettings.Inicialize();
        }

        [Fact]
        public void PostSaveRequest_To_Post()
        {
            var postSaveRequest = PostBuilder.NewObject().SaveRequestBuild();
            var post = postSaveRequest.MapTo<PostSaveRequest, Post>();

            byte[] imageBytes;
            using (var stream = new MemoryStream())
            {
                postSaveRequest.Image.CopyTo(stream);

                imageBytes = stream.ToArray();
            }

            Assert.Equal(post.Message, postSaveRequest.Message);
            Assert.Equal(post.ImageBytes, imageBytes);
        }

        [Fact]
        public void Post_To_PostResponse()
        {
            var post = PostBuilder.NewObject().DomainBuild();
            var postResponse = post.MapTo<Post, PostResponse>();

            Assert.Equal(postResponse.Id, post.Id);
            Assert.Equal(postResponse.Message, post.Message);
        }

        [Fact]
        public void Post_To_PostTagsResponse()
        {
            var tagList = new List<Tag>()
            {
                TagBuilder.NewObject().DomainBuild()
            };
            var post = PostBuilder.NewObject().WithTagList(tagList).DomainBuild();
            var postTagsResponse = post.MapTo<Post, PostTagsResponse>();

            Assert.Equal(postTagsResponse.Id, post.Id);
            Assert.Equal(postTagsResponse.Message, post.Message);
            Assert.Equal(postTagsResponse.ImageBytes, post.ImageBytes);
            Assert.Equal(postTagsResponse.Tags.Count, post.Tags.Count);
        }

        [Fact]
        public void PostPageList_To_PostTagsResponsePageList()
        {
            var postList = new List<Post>()
            {
                PostBuilder.NewObject().DomainBuild(),
                PostBuilder.NewObject().DomainBuild(),
                PostBuilder.NewObject().DomainBuild()
            };
            var pageParams = PageParamsBuilder.NewObject().DomainBuild();
            var postPageList = new PageList<Post>(postList, postList.Count, pageParams);
            var postTagsResponsePageList = postPageList.MapTo<PageList<Post>, PageList<PostTagsResponse>>();

            Assert.Equal(postTagsResponsePageList.Result.Count, postPageList.Result.Count);
            Assert.Equal(postTagsResponsePageList.CurrentPage, postPageList.CurrentPage);
            Assert.Equal(postTagsResponsePageList.TotalPages, postPageList.TotalPages);
            Assert.Equal(postTagsResponsePageList.PageSize, postPageList.PageSize);
            Assert.Equal(postTagsResponsePageList.TotalCount, postPageList.TotalCount);
        }
    }
}
