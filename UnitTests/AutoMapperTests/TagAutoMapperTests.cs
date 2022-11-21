using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Tag;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Tag;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using ArquiteturaCamadas.Domain.Entities;
using TestBuilders;

namespace UnitTests.AutoMapperTests
{
    public sealed class TagAutoMapperTests
    {
        public TagAutoMapperTests()
        {
            AutoMapperSettings.Inicialize();
        }

        [Fact]
        public void TagSaveRequest_To_Tag()
        {
            var tagSaveRequest = TagBuilder.NewObject().SaveRequestBuild();
            var tag = tagSaveRequest.MapTo<TagSaveRequest, Tag>();

            Assert.Equal(tag.TagName, tagSaveRequest.TagName);
        }

        [Fact]
        public void TagUpdateRequest_To_Tag()
        {
            var tagUpdateRequest = TagBuilder.NewObject().UpdateRequestBuild();
            var tag = tagUpdateRequest.MapTo<TagUpdateRequest, Tag>();

            Assert.Equal(tag.Id, tagUpdateRequest.Id);
            Assert.Equal(tag.TagName, tagUpdateRequest.TagName);
        }

        [Fact]
        public void Tag_To_TagResponse()
        {
            var tag = TagBuilder.NewObject().DomainBuild();
            var tagResponse = tag.MapTo<Tag, TagResponse>();

            Assert.Equal(tagResponse.Id, tag.Id);
            Assert.Equal(tagResponse.TagName, tag.TagName);
        }

        [Fact]
        public void Tag_To_TagPostsResponse()
        {
            var postList = new List<Post>()
            {
                PostBuilder.NewObject().DomainBuild(),
                PostBuilder.NewObject().DomainBuild(),
                PostBuilder.NewObject().DomainBuild(),
                PostBuilder.NewObject().DomainBuild()
            };
            var tag = TagBuilder.NewObject().WithPostList(postList).DomainBuild();
            var tagPostsResponse = tag.MapTo<Tag, TagPostsResponse>();

            Assert.Equal(tagPostsResponse.Id, tagPostsResponse.Id);
            Assert.Equal(tagPostsResponse.TagName, tagPostsResponse.TagName);
            Assert.Equal(tagPostsResponse.Posts.Count, tagPostsResponse.Posts.Count);
        }

        [Fact]
        public void TagPageList_To_TagPostsResponsePageList()
        {
            var tagList = new List<Tag>()
            {
                TagBuilder.NewObject().DomainBuild()
            };
            var pageParams = PageParamsBuilder.NewObject().DomainBuild();
            var tagPageList = new PageList<Tag>(tagList, tagList.Count, pageParams);
            var tagPostsResponsePageList = tagPageList.MapTo<PageList<Tag>, PageList<TagPostsResponse>>();

            Assert.Equal(tagPostsResponsePageList.Result.Count, tagPageList.Result.Count);
            Assert.Equal(tagPostsResponsePageList.TotalCount, tagPageList.TotalCount);
            Assert.Equal(tagPostsResponsePageList.CurrentPage, tagPageList.CurrentPage);
            Assert.Equal(tagPostsResponsePageList.PageSize, tagPageList.PageSize);
            Assert.Equal(tagPostsResponsePageList.TotalPages, tagPageList.TotalPages);
        }
    }
}
