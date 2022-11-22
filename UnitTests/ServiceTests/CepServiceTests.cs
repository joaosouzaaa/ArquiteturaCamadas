using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.Services;
using ArquiteturaCamadas.Business.Interfaces.Notification;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using TestBuilders;
using TestBuilders.Helpers;

namespace UnitTests.ServiceTests
{
    public sealed class CepServiceTests
    {
        private readonly Mock<INotificationHandler> _notificationHandlerMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly CepService _cepService;

        public CepServiceTests()
        {
            _notificationHandlerMock = new Mock<INotificationHandler>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _cepService = new CepService(_notificationHandlerMock.Object, _httpClientFactoryMock.Object);

            AutoMapperSettings.Inicialize();
        }

        [Fact]
        public async Task GetAddressFromCepAsync_ReturnsAddress()
        {
            // A
            var cep = "82145154";
            var viaCepAddressResponse = ViaCepBuilder.NewObject().AddressResponseBuild();
            var viaCepAddressResponseJsonString = JsonSerializer.Serialize(viaCepAddressResponse);
            var httpResponseMessage = new HttpResponseMessage()
            {
                Content = new StringContent(viaCepAddressResponseJsonString),
                StatusCode = HttpStatusCode.OK
            };
            
            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(h => h.RequestUri == new Uri($"https://viacep.com.br/ws/{cep}/json/")),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);
            
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://viacep.com.br/ws/")
            };

            _httpClientFactoryMock.Setup(h => h.CreateClient("ViaCepHttpClient"))
                .Returns(httpClient);

            // A
            var serviceResult = await _cepService.GetAddressFromCepAsync(cep);

            // A
            VerifyGetAddressFromCepAsyncMocks(httpClientMockTimes: 1, notificationMockTimes: 0);
            Assert.NotNull(serviceResult);
        }

        [Fact]
        public async Task GetAddressFromCepAsync_AddressLengthLesserThan8Chars_ReturnsNull()
        {
            // A
            var invalidCep = "kakak";
            _notificationHandlerMock.Setup(n => n.AddDomainNotification(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // A
            var serviceResult = await _cepService.GetAddressFromCepAsync(invalidCep);

            // A
            VerifyGetAddressFromCepAsyncMocks(httpClientMockTimes: 0, notificationMockTimes: 1);
            Assert.Null(serviceResult);
        }

        [Fact]
        public async Task GetAddressFromCepAsync_CepDoesNotExist_ReturnsNull()
        {
            // A
            var cep = "82145154";
            var httpResponseMessage = new HttpResponseMessage()
            {
                Content = new StringContent("{}"),
                StatusCode = HttpStatusCode.OK
            };

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(h => h.RequestUri == new Uri($"https://viacep.com.br/ws/{cep}/json/")),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://viacep.com.br/ws/")
            };

            _httpClientFactoryMock.Setup(h => h.CreateClient("ViaCepHttpClient"))
                .Returns(httpClient);

            // A
            var serviceResult = await _cepService.GetAddressFromCepAsync(cep);

            // A
            VerifyGetAddressFromCepAsyncMocks(httpClientMockTimes: 1, notificationMockTimes: 1);
            Assert.Null(serviceResult);
        }

        private void VerifyGetAddressFromCepAsyncMocks(int httpClientMockTimes, int notificationMockTimes)
        {
            _httpClientFactoryMock.Verify(h => h.CreateClient("ViaCepHttpClient"), Times.Exactly(httpClientMockTimes));
            _notificationHandlerMock.Verify(n => n.AddDomainNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(notificationMockTimes));
        }
    }
}
