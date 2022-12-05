using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.ViaCep;

namespace TestBuilders
{
    public sealed class ViaCepBuilder
    {
        public static ViaCepBuilder NewObject() => new ViaCepBuilder();

        public ViaCepAddressResponse AddressResponseBuild() =>
            new ViaCepAddressResponse()
            {
                Bairro = "bairro aqui",
                Cep = "821828289",
                Complemento = "",
                Localidade = "copmplemento randiom",
                Logradouro = "rua das ruas",
                Uf = "cwb"
            };
    }
}
