using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.ViaCep;
using TestBuilders.BaseBuilders;

namespace TestBuilders
{
    public sealed class ViaCepBuilder : BuilderBase
    {
        public static ViaCepBuilder NewObject() => new ViaCepBuilder();

        public ViaCepAddressResponse AddressResponseBuild() =>
            new ViaCepAddressResponse()
            {
                Bairro = GenerateRandomWord(),
                Cep = GenerateRandomWord(),
                Complemento = GenerateRandomWord(),
                Localidade = GenerateRandomWord(),
                Logradouro = GenerateRandomWord(),
                Uf = GenerateRandomWord()
            };
    }
}
