namespace Homework.Services.Interfaces
{
    public interface INorthwindImageConverterService
    {
        byte[] ConvertToNormalImage(byte[] image);

        byte[] ConvertToNorthwindImage(byte[] image);
        byte[] ConvertToNorthwindImage(IFormFile imageFile);
    }
}
