namespace ArquiteturaCamadas.ApplicationService.AutoMapperSettings
{
    public static class AutoMapperExtension
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
            where TSource : class
            where TDestination : class
            =>
            AutoMapperSettings.Mapper.Map<TSource, TDestination>(source);
    }
}
