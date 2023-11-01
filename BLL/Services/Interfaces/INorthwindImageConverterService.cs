namespace BLL.Services.Interfaces
{
    internal interface INorthwindImageConverterService
    {
        byte[] ConvertToNormalImage(byte[] image);

        byte[] ConvertToNorthwindImage(byte[] image);
    }
}
