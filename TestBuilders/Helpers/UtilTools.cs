using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Text;
using System.Text.Json;

namespace TestBuilders.Helpers
{
    public static class UtilTools
    {
        public static IFormFile BuildIFormFile()
        {
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            
            return new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "image.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg",
                ContentDisposition = "form-data; name=\"Image\"; filename=\"image.jpg\""
            };
        }

        public static IFormFile BuildInvalidIFormFileImageFormat()
        {
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");

            return new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "image.txt")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg",
                ContentDisposition = "form-data; name=\"Image\"; filename=\"image.txt\""
            };
        }

        public static Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> MockIIncludableQuery<TEntity>()
            where TEntity : class
            =>
            It.IsAny<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>();
    }
}
