using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Enums;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Person;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Person;
using ArquiteturaCamadas.Domain.Enums;
using Bogus;
using TestBuilders.BaseBuilders;

namespace TestBuilders
{
    public sealed class PersonBuilder : BuilderBase
    {
        private int _age = 20;
        private EGender _gender = new Faker().PickRandom<EGender>();
        private int _id = GenerateRandomNumber();
        private string _name = GenerateRandomWord();
        private EGenderRequest _genderRequest = new Faker().PickRandom<EGenderRequest>();

        public static PersonBuilder NewObject() => new PersonBuilder();

        public ArquiteturaCamadas.Domain.Entities.Person DomainBuild() =>
            new ArquiteturaCamadas.Domain.Entities.Person
            {
                Age = _age,
                Gender = _gender,
                Id = _id,
                Name = _name
            };

        public PersonSaveRequest SaveRequestBuild() =>
            new PersonSaveRequest
            {
                Age = _age,
                Gender = _genderRequest,
                Name = _name
            };

        public PersonUpdateRequest UpdateRequestBuild() =>
            new PersonUpdateRequest
            {
                Age = _age,
                Gender = _genderRequest,
                Id = _id,
                Name = _name
            };

        public PersonResponse ResponseBuild() =>
            new PersonResponse
            {
                Age = _age,
                Gender = (ushort)_gender,
                Id = _id,
                Name = _name
            };

        public PersonBuilder WithName(string name)
        {
            _name = name;

            return this;
        }

        public PersonBuilder WithAge(int age)
        {
            _age = age;

            return this;
        }

        public PersonBuilder WithId(int id)
        {
            _id = id;

            return this;
        }
    }
}
