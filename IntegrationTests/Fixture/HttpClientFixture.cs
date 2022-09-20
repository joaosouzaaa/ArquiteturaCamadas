using ArquiteturaCamadas.Infra.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

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
                            options.UseInMemoryDatabase("DbForTests", root);
                        });
                    });
                });

            _httpClient = appFactory.CreateClient();
        }
    }
}
