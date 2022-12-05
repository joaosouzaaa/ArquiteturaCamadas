using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Post;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Post;
using IntegrationTests.Fixture;
using System.Net.Http.Headers;
using System.Text;
using TestBuilders;

namespace IntegrationTests
{
    public sealed class PostIntegrationTests : HttpClientFixture
    {
        [Fact]
        public async Task AddAsync_ReturnsTrue()
        {
            // A
            var numberOfTimesToAddTags = new Random().Next(1, 10);

            // A
            var addResult = await AddPostWithRelationsShipAsync(numberOfTimesToAddTags);

            // A
            Assert.True(addResult);
        }

        [Fact]
        public async Task UpdateUnreapeatTagsSearchAsync_ReturnsTrue()
        {
            // A
            var numberOfTimesToAddTags = new Random().Next(1, 10);

            var addResult = await AddPostWithRelationsShipAsync(numberOfTimesToAddTags);

            var tagsIds = BuildTagsIdsList(numberOfTimesToAddTags);

            var firstTag = tagsIds.First();
            tagsIds.Remove(firstTag);

            var addTagResult = await AddTagAsync();

            var lastTag = tagsIds.Last();
            tagsIds.Add(lastTag + 1);

            var postUpdateRequest = PostBuilder.NewObject().WithId(1).WithTagsIds(tagsIds).UpdateRequestBuild();

            // A
            var updateResult = await UpdatePostAsync("api/Post/update-post", postUpdateRequest);

            // A
            Assert.True(addResult);
            Assert.True(addTagResult);
            Assert.True(updateResult);
        }

        [Fact]
        public async Task UpdateManyToManyAsync_ReturnsTrue()
        {
            // A
            var postSaveRequest = PostBuilder.NewObject().SaveRequestBuild();
            var addResult = await AddPostAsync(postSaveRequest);

            var postUpdateRequest = PostBuilder.NewObject().WithId(1).UpdateRequestBuild();

            // A
            var updateResult = await UpdatePostAsync("api/Post/update-post-many-to-many", postUpdateRequest);

            // A
            Assert.True(addResult);
            Assert.True(updateResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            // A
            var postSaveRequest = PostBuilder.NewObject().SaveRequestBuild();
            var addResult = await AddPostAsync(postSaveRequest);

            // A
            var deleteResult = await CreateDeleteAsync("api/Post/delete-post?id=1");

            // A
            Assert.True(addResult);
            Assert.True(deleteResult);
        }

        [Fact]
        public async Task FindAllEntitiesWithPaginationAsync_ReturnsEntities()
        {
            // A
            var numberOfTimesToAdd = 6;
            
            var addResult = await AddPostsInRangeAsync(numberOfTimesToAdd);

            // A
            var postTagsResponsePageList = await CreateGetAllPaginatedAsync<PostTagsResponse>("api/Post/find-all-posts-paginated?PageSize=2&PageNumber=1");

            // A
            Assert.True(addResult);
            Assert.NotEmpty(postTagsResponsePageList.Result);
            Assert.NotEqual(postTagsResponsePageList.Result.Count, numberOfTimesToAdd);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEntities()
        {
            // A
            var numberOfTimesToAdd = new Random().Next(1, 10);

            var addResult = await AddPostsInRangeAsync(numberOfTimesToAdd);

            // A
            var postTagsResponseList = await CreateGetAllAsync<PostTagsResponse>("api/Post/find-all-posts");

            // A
            Assert.True(addResult);
            Assert.Equal(postTagsResponseList.Count, numberOfTimesToAdd);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsEntity()
        {
            // A
            var postSaveRequest = PostBuilder.NewObject().SaveRequestBuild();
            var addResult = await AddPostAsync(postSaveRequest);

            // A
            var postTagsResponse = await CreateGetAsync<PostTagsResponse>("api/Post/find-post?id=1");

            // A
            Assert.True(addResult);
            Assert.NotNull(postTagsResponse);
        }

        private async Task<bool> AddPostWithRelationsShipAsync(int numberOfTimesToAddTags)
        {
            var addTagsResult = await AddTagsInRange(numberOfTimesToAddTags);

            var tagsIds = BuildTagsIdsList(numberOfTimesToAddTags);

            var postSaveRequest = PostBuilder.NewObject().WithTagsIds(tagsIds).SaveRequestBuild();

            var addPostResult = await AddPostAsync(postSaveRequest);

            if (addTagsResult is true && addPostResult is true)
                return true;

            return false;
        }

        private async Task<bool> AddTagsInRange(int range)
        {
            var addResult = false;

            for (var i = 0; i < range; i++)
            {
                addResult = await AddTagAsync();
            }

            return addResult;
        }

        private async Task<bool> AddTagAsync()
        {
            var tagSaveRequest = TagBuilder.NewObject().SaveRequestBuild();

            return await CreatePostAsJsonAsync("api/Tag/add-tag", tagSaveRequest);
        }

        private List<int> BuildTagsIdsList(int numberOfTags)
        {
            var tagsIdsList = new List<int>();

            for(var i = 0; i < numberOfTags; i++)
            {
                tagsIdsList.Add(i + 1);
            }

            return tagsIdsList;
        }

        private async Task<bool> AddPostAsync(PostSaveRequest postSaveRequest)
        {
            var addPostMultipartFormDataContent = await BuildAddPostAsyncMultipartFormDataContent(postSaveRequest);

            return await CreatePostAsync("api/Post/add-post", addPostMultipartFormDataContent);
        }

        private async Task<MultipartFormDataContent> BuildAddPostAsyncMultipartFormDataContent(PostSaveRequest postSaveRequest)
        {
            var memoryStream = new MemoryStream();
            await postSaveRequest.Image.CopyToAsync(memoryStream);
            var streamContent = new StreamContent(memoryStream);

            streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            var addPostMultipartFormDataContent = new MultipartFormDataContent()
            {
                {new StringContent(postSaveRequest.Message),  "Message"},
                {streamContent, "ImageName", "filename.jpg" }
            };

            foreach (var tagId in postSaveRequest.TagsIds)
                addPostMultipartFormDataContent.Add(new StringContent(tagId.ToString()), "TagsIds");

            return addPostMultipartFormDataContent;
        }

        private async Task<bool> UpdatePostAsync(string requestUri, PostUpdateRequest postUpdateRequest)
        {
            var updatePostMultipartFormDataContent = await BuildUpdatePostAsyncMultipartFormDataContent(postUpdateRequest);
            
            return await CreatePutAsync(requestUri, updatePostMultipartFormDataContent);
        }

        private async Task<MultipartFormDataContent> BuildUpdatePostAsyncMultipartFormDataContent(PostUpdateRequest postUpdateRequest)
        {
            var memoryStream = new MemoryStream();
            await postUpdateRequest.Image.CopyToAsync(memoryStream);
            var streamContent = new StreamContent(memoryStream);

            streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            var updatePostMultipartFormDataContent = new MultipartFormDataContent()
            {
                {new StringContent(postUpdateRequest.Id.ToString()),  "Id"},
                {new StringContent(postUpdateRequest.Message),  "Message"},
                {streamContent, "ImageName", "filename.jpg" }
            };

            foreach (var tagId in postUpdateRequest.TagsIds)
                updatePostMultipartFormDataContent.Add(new StringContent(tagId.ToString()), "TagsIds");

            return updatePostMultipartFormDataContent;
        }

        private async Task<bool> AddPostsInRangeAsync(int range)
        {
            var addResult = false;

            for(var i = 0; i < range; i++)
            {
                var postSaveRequest = PostBuilder.NewObject().SaveRequestBuild();
                
                addResult = await AddPostAsync(postSaveRequest);
            }

            return addResult;
        }
    }
}
