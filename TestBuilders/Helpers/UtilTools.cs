using Microsoft.AspNetCore.Http;
using System.Text;

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
    }
}
