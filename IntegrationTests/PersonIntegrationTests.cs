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
            var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();

            var saveResult = await AddPersonAsync(personSaveRequest);

            Assert.True(saveResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsSuccess()
        {
            var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();
            var saveResult = await AddPersonAsync(personSaveRequest);
            var personUpdateRequest = PersonBuilder.NewObject().WithId(1).UpdateRequestBuild();
            var requestUri = "api/Person/update-person";

            var updateResult = await CreatePutAsJsonAsync(requestUri, personUpdateRequest);

            Assert.True(saveResult);
            Assert.True(updateResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsSuccess()
        {
            var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();
            var saveResult = await AddPersonAsync(personSaveRequest);
            var requestUri = "api/Person/delete-person?id=1";

            var deleteResult = await CreateDeleteAsync(requestUri);
            
            Assert.True(saveResult);
            Assert.True(deleteResult);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsEntity()
        {
            var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();
            var saveResult = await AddPersonAsync(personSaveRequest);
            var requestUri = "api/Person/find-person?id=1";

            var getResult = await CreateGetAsync<PersonResponse>(requestUri);

            Assert.True(saveResult);
            Assert.NotNull(getResult);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEntities()
        {
            var numberOfTimesToAdd = new Random().Next(1, 10);
            var saveResult = await AddPersonsInRangeAsync(numberOfTimesToAdd);
            var requestUri = "api/Person/find-all-persons";

            var getAllResult = await CreateGetAllAsync<PersonResponse>(requestUri);

            Assert.True(saveResult);
            Assert.Equal(getAllResult.Count, numberOfTimesToAdd);
        }

        [Fact]
        public async Task FindAllEntitiesWithPaginationAsync_ReturnsEntities()
        {
            var numberOfTimesToAdd = 4;
            var saveResult = await AddPersonsInRangeAsync(numberOfTimesToAdd);
            var requestUri = "api/Person/find-all-persons-paginated?PageSize=1&PageNumber=1";

            var getAllPaginatedResult = await CreateGetAllPaginatedAsync<PersonResponse>(requestUri);

            Assert.True(saveResult);
            Assert.NotEmpty(getAllPaginatedResult.Result);
            Assert.NotEqual(getAllPaginatedResult.Result.Count, numberOfTimesToAdd);
        }

        private async Task<bool> AddPersonAsync(PersonSaveRequest personSaveRequest) =>
            await CreatePostAsJsonAsync(requestUri: "api/Person/add-person", personSaveRequest);

        private async Task<bool> AddPersonsInRangeAsync(int range)
        {
            var saveResult = true;

            for(var i = 0; i < range; i++)
            {
                var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();

                saveResult = await AddPersonAsync(personSaveRequest);
            }

            return saveResult;
        }
    }
}
