namespace ArquiteturaCamadas.ApplicationService.AutoMapperSettings
{
    public static class AutoMapperExtension
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
            where TSource : class
            where TDestination : class
            =>
            AutoMapperConfigurations.Mapper.Map<TSource, TDestination>(source);

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
            where TSource : class
            where TDestination : class
            =>
            AutoMapperConfigurations.Mapper.Map(source, destination);
    }
}
