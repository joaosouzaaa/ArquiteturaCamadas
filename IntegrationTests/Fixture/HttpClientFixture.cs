using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using ArquiteturaCamadas.Infra.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace IntegrationTests.Fixture
{
    public abstract class HttpClientFixture
    {
        protected readonly HttpClient _httpClient;
        
        protected HttpClientFixture()
        {
            var root = new InMemoryDatabaseRoot();

            var appFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                typeof(DbContextOptions<ArquiteturaCamadasDbContext>));

                        services.Remove(descriptor);

                        services.AddDbContext<ArquiteturaCamadasDbContext>(options =>
                        {
                            options.ConfigureWarnings(o => o.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                            options.UseInMemoryDatabase("DbForTests", root);
                        });
                    });
                });

            _httpClient = appFactory.CreateClient();
        }

        protected async Task<bool> CreatePostAsync(string requestUri, HttpContent httpContent)
        {
            var saveHttpResponseMessage = await _httpClient.PostAsync(requestUri, httpContent);

            return saveHttpResponseMessage.IsSuccessStatusCode;
        }

        protected async Task<bool> CreatePutAsync(string requestUri, HttpContent httpContent)
        {
            var updateHttpResponseMessage = await _httpClient.PutAsync(requestUri, httpContent);

            return updateHttpResponseMessage.IsSuccessStatusCode;
        }

        protected async Task<bool> CreatePostAsJsonAsync<TSave>(string requestUri, TSave save)
            where TSave : class
        {
            var saveHttpResponseMessage = await _httpClient.PostAsJsonAsync(requestUri, save);

            return saveHttpResponseMessage.IsSuccessStatusCode;
        }

        protected async Task<bool> CreatePutAsJsonAsync<TUpdate>(string requestUri, TUpdate update)
            where TUpdate : class
        {
            var updateHttpResponseMessage = await _httpClient.PutAsJsonAsync(requestUri, update);

            return updateHttpResponseMessage.IsSuccessStatusCode;
        }

        protected async Task<bool> CreateDeleteAsync(string requestUri)
        {
            var deleteHttpResponseMessage = await _httpClient.DeleteAsync(requestUri);

            return deleteHttpResponseMessage.IsSuccessStatusCode;
        }

        protected async Task<TResponse> CreateGetAsync<TResponse>(string requestUri)
            where TResponse : class
        {
            var getHttpResponseMessage = await _httpClient.GetAsync(requestUri);

            return await getHttpResponseMessage.Content.ReadFromJsonAsync<TResponse>();
        }

        protected async Task<List<TResponse>> CreateGetAllAsync<TResponse>(string requesUri)
            where TResponse : class
            =>
            await _httpClient.GetFromJsonAsync<List<TResponse>>(requesUri);

        protected async Task<PageList<TResponse>> CreateGetAllPaginatedAsync<TResponse>(string requesUri)
            where TResponse : class
            =>
            await _httpClient.GetFromJsonAsync<PageList<TResponse>>(requesUri);
    }
}
