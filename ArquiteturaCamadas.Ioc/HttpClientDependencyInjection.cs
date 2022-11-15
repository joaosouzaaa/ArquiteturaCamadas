using Microsoft.Extensions.DependencyInjection;

namespace ArquiteturaCamadas.Ioc
{
    public static class HttpClientDependencyInjection
    {
        public static void AddHttpClientDependencyInjection(this IServiceCollection services)
        {
            services.AddHttpClient("ViaCepHttpClient", h => h.BaseAddress = new Uri("https://viacep.com.br/ws/"));
        }
    }
}
