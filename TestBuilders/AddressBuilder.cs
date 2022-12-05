using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Address;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Address;
using ArquiteturaCamadas.Domain.Entities;

namespace TestBuilders
{
    public sealed class AddressBuilder 
    {
        private string _city = "palavra alkeaotiria";
        private string _zipCode = "78557102";
        private string _street = "joao";
        private string _state = "pr";
        private string _number = "8181";
        private string _complement = "random";
        private string _district = "mock";

        public static AddressBuilder NewObject() => new AddressBuilder();

        public Address DomainBuild() =>
            new Address()
            {
                City = _city,
                Complement = _complement,
                District = _district,
                Id = 1,
                Number = _number,
                State = _state,
                Street = _street,
                ZipCode = _zipCode
            };

        public AddressRequest RequestBuild() =>
            new AddressRequest()
            {
                Complement = _complement,
                Number = _number,
                ZipCode = _zipCode
            };

        public AddressResponse ResponseBuild() =>
            new AddressResponse()
            {
                City = _city,
                Complement = _complement,
                District = _district,
                Id = 1,
                Number = _number,
                State = _state,
                Street = _street,
                ZipCode = _zipCode
            };

        public AddressBuilder WithZipCode(string zipCode)
        {
            _zipCode = zipCode;

            return this;
        }

        public AddressBuilder WithCity(string city)
        {
            _city = city;

            return this;
        }

        public AddressBuilder WithStreet(string street)
        {
            _street = street;

            return this;
        }

        public AddressBuilder WithState(string state)
        {
            _state = state;

            return this;
        }

        public AddressBuilder WithNumber(string number)
        {
            _number = number;

            return this;
        }

        public AddressBuilder WithDistrict(string district)
        {
            _district = district;

            return this;
        }

        public AddressBuilder WithComplement(string complement)
        {
            _complement = complement;

            return this;
        }
    }
}
