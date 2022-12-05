using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Person;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Person;
using IntegrationTests.Fixture;
using TestBuilders;

namespace IntegrationTests
{
    public sealed class PersonIntegrationTests : HttpClientFixture
    {
        [Fact]
        public async Task AddAsync_ReturnsSuccess()
        {
            // A A
            var saveResult = await AddPersonAsync();

            // A
            Assert.True(saveResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsSuccess()
        {
            // A
            var saveResult = await AddPersonAsync();

            var personUpdateRequest = PersonBuilder.NewObject().WithId(1).UpdateRequestBuild();

            var updateAsyncRequestUri = "api/Person/update-person";
            
            // A
            var updateResult = await CreatePutAsJsonAsync(updateAsyncRequestUri, personUpdateRequest);

            // A
            Assert.True(saveResult);
            Assert.True(updateResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsSuccess()
        {
            // A
            var saveResult = await AddPersonAsync();

            var deleteAsyncRequestUri = "api/Person/delete-person?id=1";

            // A
            var deleteResult = await CreateDeleteAsync(deleteAsyncRequestUri);
            
            // A
            Assert.True(saveResult);
            Assert.True(deleteResult);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsEntity()
        {
            // A
            var saveResult = await AddPersonAsync();

            var findByidAsyncRequestUri = "api/Person/find-person?id=1";

            // A
            var getResult = await CreateGetAsync<PersonResponse>(findByidAsyncRequestUri);

            // A
            Assert.True(saveResult);
            Assert.NotNull(getResult);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEntities()
        {
            // A
            var numberOfTimesToAdd = new Random().Next(1, 10);
            var saveResult = await AddPersonsInRangeAsync(numberOfTimesToAdd);

            var findAllEntitiesAsyncRequestUri = "api/Person/find-all-persons";

            // A
            var getAllResult = await CreateGetAllAsync<PersonResponse>(findAllEntitiesAsyncRequestUri);

            // A
            Assert.True(saveResult);
            Assert.Equal(getAllResult.Count, numberOfTimesToAdd);
        }

        [Fact]
        public async Task FindAllEntitiesWithPaginationAsync_ReturnsEntities()
        {
            // A
            var numberOfTimesToAdd = 4;
            var saveResult = await AddPersonsInRangeAsync(numberOfTimesToAdd);

            var findAllEntitiesWithPaginationAsyncRequestUri = "api/Person/find-all-persons-paginated?PageSize=1&PageNumber=1";

            // A
            var getAllPaginatedResult = await CreateGetAllPaginatedAsync<PersonResponse>(findAllEntitiesWithPaginationAsyncRequestUri);

            // A
            Assert.True(saveResult);
            Assert.NotEmpty(getAllPaginatedResult.Result);
            Assert.NotEqual(getAllPaginatedResult.Result.Count, numberOfTimesToAdd);
        }

        private async Task<bool> AddPersonAsync()
        {
            var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();
            
            return await CreatePostAsJsonAsync(requestUri: "api/Person/add-person", personSaveRequest);
        }

        private async Task<bool> AddPersonsInRangeAsync(int range)
        {
            var saveResult = true;

            for(var i = 0; i < range; i++)
            {
                saveResult = await AddPersonAsync();
            }

            return saveResult;
        }
    }
}
