namespace Homework.Services.Interfaces
{
    public interface IFormFileToStreamConverter
    {
        byte[] ConvertToStream(IFormFile imageFile);
    }
}
