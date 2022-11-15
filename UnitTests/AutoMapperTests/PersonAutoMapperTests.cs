using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Person;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Person;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using ArquiteturaCamadas.Domain.Entities;
using TestBuilders;

namespace UnitTests.AutoMapperTests
{
    public sealed class PersonAutoMapperTests
    {
        public PersonAutoMapperTests()
        {
            AutoMapperSettings.Inicialize();
        }

        [Fact]
        public void PersonSaveRequest_To_Person()
        {
            var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();
            var person = personSaveRequest.MapTo<PersonSaveRequest, Person>();

            Assert.Equal(person.Name, personSaveRequest.Name);
            Assert.Equal(person.Age, personSaveRequest.Age);
            Assert.Equal((ushort)person.Gender, (ushort)personSaveRequest.Gender);
        }

        [Fact]
        public void PersonUpdateRequest_To_Person()
        {
            var personUpdateRequest = PersonBuilder.NewObject().UpdateRequestBuild();
            var person = personUpdateRequest.MapTo<PersonUpdateRequest, Person>();

            Assert.Equal(person.Id, personUpdateRequest.Id);
            Assert.Equal(person.Name, personUpdateRequest.Name);
            Assert.Equal(person.Age, personUpdateRequest.Age);
            Assert.Equal((ushort)person.Gender, (ushort)personUpdateRequest.Gender);
        }

        [Fact]
        public void Person_To_PersonResponse()
        {
            var person = PersonBuilder.NewObject().DomainBuild();
            var personResponse = person.MapTo<Person, PersonResponse>();

            Assert.Equal(personResponse.Id, person.Id);
            Assert.Equal(personResponse.Name, person.Name);
            Assert.Equal(personResponse.Age, person.Age);
            Assert.Equal(personResponse.Gender, (ushort)person.Gender);
        }

        [Fact]
        public void PersonPageList_To_PersonResponsePageList()
        {
            var personList = new List<Person>()
            {
                PersonBuilder.NewObject().DomainBuild(),
                PersonBuilder.NewObject().DomainBuild(),
                PersonBuilder.NewObject().DomainBuild()
            };
            var pageParams = PageParamsBuilder.NewObject().DomainBuild();
            var personPageList = new PageList<Person>(personList, personList.Count, pageParams);
            var personResponsePageList = personPageList.MapTo<PageList<Person>, PageList<PersonResponse>>();

            Assert.Equal(personResponsePageList.Result.Count, personPageList.Result.Count);
            Assert.Equal(personResponsePageList.CurrentPage, personPageList.CurrentPage);
            Assert.Equal(personResponsePageList.TotalPages, personPageList.TotalPages);
            Assert.Equal(personResponsePageList.PageSize, personPageList.PageSize);
            Assert.Equal(personResponsePageList.TotalCount, personPageList.TotalCount);
        }
    }
}
