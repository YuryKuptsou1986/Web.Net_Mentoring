using Homework.Services.Interfaces;

namespace Homework.Services.Implementation
{
    public class FormFileToStreamConverter : IFormFileToStreamConverter
    {
        public FormFileToStreamConverter() { }

        public byte[] ConvertToStream(IFormFile imageFile)
        {
            var memoryStram = new MemoryStream();
            imageFile.CopyTo(memoryStram);

            return memoryStram.ToArray();
        }
    }
}
