using ArquiteturaCamadas.ApplicationService.Requests.Enums;
using ArquiteturaCamadas.ApplicationService.Requests.Person;
using ArquiteturaCamadas.ApplicationService.Responses;
using ArquiteturaCamadas.Domain.Enums;
using Bogus;

namespace TestBuilders
{
    public class PersonBuilder
    {
        private int _age = 20;
        private EGender _gender = new Faker().PickRandom<EGender>();
        private int _id = new Faker().Random.Int(1, 100);
        private string _name = "random name";
        private EGenderRequest _genderRequest = new Faker().PickRandom<EGenderRequest>();

        public static PersonBuilder NewObject()
        {
            return new PersonBuilder();
        }

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
    }
}
