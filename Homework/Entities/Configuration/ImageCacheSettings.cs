namespace Homework.Entities.Configuration
{
    public class ImageCacheSettings
    {
        public int MaxImages { get; set; }
        public int ExpirationTimeInSecons { get; set; }
        public string CachePath { get; set; }
    }
}
