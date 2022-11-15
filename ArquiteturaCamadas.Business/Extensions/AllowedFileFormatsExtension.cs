using static System.Net.Mime.MediaTypeNames;

namespace ArquiteturaCamadas.Business.Extensions
{
    public static class AllowedFileFormatsExtension
    {
        public static bool ValidateFileFormat(this string fileName)
        {
            var extensionList = new List<string>()
            {
                ".JPEG",
                ".JPG",
                ".PNG",
                ".JFIF",
                ".PDF"
            };

            var imageExtension = Path.GetExtension(fileName.ToUpper());

            if (!extensionList.Contains(imageExtension))
                return false;

            return true;
        }
    }
}
