using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ArquiteturaCamadas.ApplicationService.AutoMapperSettings.Converters
{
    public sealed class FormFileToBytes : IValueConverter<IFormFile, byte[]>
    {
        public byte[] Convert(IFormFile file, ResolutionContext context)
        {
            if (file is not null)
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);

                    return stream.ToArray();
                }
            }

            return null;
        }
    }
}
