using Homework.Services.Interfaces;

namespace Homework.Services.Implementation
{
    public class NorthwindImageConverterService : INorthwindImageConverterService
    {
        public NorthwindImageConverterService() { }

        public byte[] ConvertToNormalImage(byte[] image)
        {
            byte[] convertedImage = null;
            using (MemoryStream stream = new MemoryStream()) {
                int offset = 78;
                stream.Write(image, offset, image.Length - offset);
                convertedImage = stream.ToArray();
            }
            return convertedImage;
        }

        public byte[] ConvertToNorthwindImage(IFormFile imageFile)
        {
            var memoryStram = new MemoryStream();
            imageFile.CopyTo(memoryStram);

            return ConvertToNorthwindImage(memoryStram.ToArray());
        }

        public byte[] ConvertToNorthwindImage(byte[] image)
        {
            var offsetArray = new byte[78];
            return offsetArray.Concat(image).ToArray();
        }
    }
}
