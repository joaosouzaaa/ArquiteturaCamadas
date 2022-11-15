using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using TestBuilders.BaseBuilders;

namespace TestBuilders
{
    public sealed class PageParamsBuilder : BuilderBase
    {
        public static PageParamsBuilder NewObject() => new PageParamsBuilder();
        
        public PageParams DomainBuild() =>
            new PageParams()
            {
                PageNumber = GenerateRandomNumber(),
                PageSize = GenerateRandomNumber()
            };
    }
}
