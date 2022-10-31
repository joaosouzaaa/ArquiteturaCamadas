using AutoMapper;
using System.Reflection;

namespace ArquiteturaCamadas.ApplicationService.AutoMapperSettings
{
    public static class AutoMapperSettings
    {
        public static IMapper Mapper { get; private set; }
        public static MapperConfiguration MapperConfiguration { get; private set; }

        public static void Inicialize()
        {
            MapperConfiguration = new MapperConfiguration(config =>
            {
                var profiles = Assembly.GetExecutingAssembly().GetExportedTypes().Where(p => p.IsClass && typeof(Profile).IsAssignableFrom(p));

                foreach (var profile in profiles)
                    config.AddProfile((Profile)Activator.CreateInstance(profile));
            });

            Mapper = MapperConfiguration.CreateMapper();
        }
    }
}
