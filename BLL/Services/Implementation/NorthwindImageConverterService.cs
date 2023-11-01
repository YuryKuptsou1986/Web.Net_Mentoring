using BLL.Services.Interfaces;

namespace BLL.Services.Implementation
{
    internal class NorthwindImageConverterService : INorthwindImageConverterService
    {
        public NorthwindImageConverterService() { }

        public byte[] ConvertToNormalImage(byte[] image)
        {
            byte[] convertedImage = null;
            using (MemoryStream stream = new MemoryStream())
            {
                int offset = 78;
                stream.Write(image, offset, image.Length - offset);
                convertedImage = stream.ToArray();
            }
            return convertedImage;
        }

        public byte[] ConvertToNorthwindImage(byte[] image)
        {
            var offsetArray = new byte[78];
            return offsetArray.Concat(image).ToArray();
        }
    }
}
