using ArquiteturaCamadas.Business.Settings.PaginationSettings;

namespace TestBuilders
{
    public sealed class PageParamsBuilder 
    {
        public static PageParamsBuilder NewObject() => new PageParamsBuilder();
        
        public PageParams DomainBuild() =>
            new PageParams()
            {
                PageNumber = GenerateRandomNumber(),
                PageSize = GenerateRandomNumber()
            };

        private static int GenerateRandomNumber() =>
            new Random().Next();
    }
}
