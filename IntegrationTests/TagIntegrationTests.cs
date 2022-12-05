using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Tag;
using IntegrationTests.Fixture;
using TestBuilders;

namespace IntegrationTests
{
    public sealed class TagIntegrationTests : HttpClientFixture
    {
        [Fact]
        public async Task AddAsync_ReturnsTrue()
        {
            // A A
            var addResult = await AddTagAsync();

            // A
            Assert.True(addResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue()
        {
            // A
            var addResult = await AddTagAsync();

            var tagUpdateRequest = TagBuilder.NewObject().WithId(1).UpdateRequestBuild();

            var updateAsyncRequestUri = "api/Tag/update-tag";

            // A
            var updateResult = await CreatePutAsJsonAsync(updateAsyncRequestUri, tagUpdateRequest);

            // A
            Assert.True(addResult);
            Assert.True(updateResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            // A
            var addResult = await AddTagAsync();

            var deleteRequestUri = "api/Tag/delete-tag?id=1";

            // A
            var deleteResult = await CreateDeleteAsync(deleteRequestUri);

            // A
            Assert.True(addResult);
            Assert.True(deleteResult);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsEntity()
        {
            // A
            var addResult = await AddTagAsync();

            var findByIdRequestUri = "api/Tag/find-tag?id=1";

            // A
            var findByIdResult = await CreateGetAsync<TagResponse>(findByIdRequestUri);

            // A
            Assert.True(addResult);
            Assert.NotNull(findByIdResult);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEntities()
        {
            // A
            var numberOfTimesToAdd = new Random().Next(1, 10);
            var addResult = await AddTagsInRange(numberOfTimesToAdd);

            var findAllRequestUri = "api/Tag/find-all-tags";

            // A
            var findAllResult = await CreateGetAllAsync<TagPostsResponse>(findAllRequestUri);

            // A
            Assert.True(addResult);
            Assert.Equal(findAllResult.Count, numberOfTimesToAdd);
        }

        [Fact]
        public async Task FindAllEntitiesWithPaginationAsync_ReturnsEntities()
        {
            // A
            var numberOfTimesToAdd = 5;
            var addResult = await AddTagsInRange(numberOfTimesToAdd);

            var findAllEntitiesWithPaginationRequestUri = "api/Tag/find-all-tags-paginated?PageSize=3&PageNumber=1";

            // A
            var findAllWithPaginationResult = await CreateGetAllPaginatedAsync<TagPostsResponse>(findAllEntitiesWithPaginationRequestUri);

            // A
            Assert.True(addResult);
            Assert.NotEmpty(findAllWithPaginationResult.Result);
            Assert.NotEqual(findAllWithPaginationResult.Result.Count, numberOfTimesToAdd);
        }

        private async Task<bool> AddTagAsync()
        {
            var tagSaveRequest = TagBuilder.NewObject().SaveRequestBuild();

            return await CreatePostAsJsonAsync("api/Tag/add-tag", tagSaveRequest);
        }

        private async Task<bool> AddTagsInRange(int range)
        {
            var addResult = false;

            for(var i = 0; i < range; i++)
            {
                addResult = await AddTagAsync();
            }

            return addResult;
        }
    }
}
