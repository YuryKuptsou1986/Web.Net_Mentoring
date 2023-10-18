using Homework.Entities.Configuration;
using Microsoft.Extensions.Options;
using System.IO;

namespace Homework.MIddleware
{
    public class ImageCacheMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<ImageCacheSettings> _settings;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ImageCacheMiddleware> _logger;
        private readonly object _lock = new object();
        private const string ImageIdQueryParam = "image_id";
        private const string ImageExtension = ".bmp";
        private const string ImageContentType = "image/bmp";

        public ImageCacheMiddleware(RequestDelegate next, IOptions<ImageCacheSettings> settings,
            IWebHostEnvironment webHostEnvironment,
            ILogger<ImageCacheMiddleware> logger)
        {
            _next = next;
            _settings = settings;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;

            lock (_lock) {
                string cacheFolderPath = GetCacheFolderPath();

                if (!Directory.Exists(cacheFolderPath)) {
                    Directory.CreateDirectory(cacheFolderPath);
                } else {
                    DeleteOldImages(cacheFolderPath);
                }
            }

            if (await TryToAddIntoResponseCachePicture(request, response)) {
                return;
            }

            // responce Body is not readable, so change it with memory stream.
            var originalBodyStream = context.Response.Body;
            await using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            // execute
            await _next(context);

            await TryToSavePictureIntoCache(request, memoryStream);

            // put original stream into body.
            response.Body = originalBodyStream;
            // write all info from temporary stram into original
            await response.Body.WriteAsync(memoryStream.ToArray());
        }

        private async Task TryToSavePictureIntoCache(HttpRequest request, MemoryStream imageStream)
        {
            if (request.Method == HttpMethod.Get.ToString()) {

                if (request.Query.ContainsKey(ImageIdQueryParam)) {
                    var imageName = request.Query[ImageIdQueryParam] + ImageExtension;

                    imageStream.Seek(0, SeekOrigin.Begin);

                    lock (_lock) {
                        ClearCasheIfExceedImageCount(GetCacheFolderPath());
                        SaveImage(imageStream, imageName);
                    }
                }
            }
        }

        private async Task<bool> TryToAddIntoResponseCachePicture(HttpRequest request, HttpResponse response)
        {
            if (request.Method == HttpMethod.Get.ToString()) {

                if (request.Query.ContainsKey(ImageIdQueryParam)) {
                    var imageName = request.Query[ImageIdQueryParam] + ImageExtension;

                    var filePath = BuildServerPath(imageName);
                    byte[] image = null;
                    lock (_lock) {
                        FileInfo fileInfo = new FileInfo(filePath);
                        if (fileInfo.Exists) {
                            if (fileInfo.CreationTimeUtc.AddSeconds(_settings.Value.ExpirationTimeInSecons) > DateTime.UtcNow) {
                                // cache is not expired, return image from cache
                                image = GetImage(filePath);
                            }
                        }
                    }
                    if (image != null) {
                        response.ContentType = ImageContentType;
                        await response.Body.WriteAsync(image);
                        return true;
                    }
                }
            }
            return false;
        }

        private void DeleteFiles(IEnumerable<string> files)
        {
            if (!files.Any()) {
                return;
            }
            foreach (var item in files) {
                File.Delete(item);
            }
        }

        private void ClearCasheIfExceedImageCount(string folderPath)
        {
            var files = Directory.GetFiles(folderPath)
                .Select(x => new FileInfo(x))
                .OrderBy(x => x.CreationTimeUtc)
                .Select(x => x.FullName)
                .SkipLast(_settings.Value.MaxImages - 1); //we wil save new file, so delete more files.

            DeleteFiles(files);
        }

        private void DeleteOldImages(string folderPath)
        {
            var expiredFiles = Directory
                .GetFiles(folderPath)
                .Where(x => {
                    var fileInfo = new FileInfo(x);
                    if (fileInfo.CreationTimeUtc.AddSeconds(_settings.Value.ExpirationTimeInSecons) > DateTime.UtcNow) {
                        return false; // not expired
                    } else {
                        return true;// expired
                    }
                });

            DeleteFiles(expiredFiles);
        }

        private string BuildServerPath(string name)
        {
            return Path.Combine(GetCacheFolderPath(), name);
        }

        private string GetCacheFolderPath()
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, _settings.Value.CachePath);
        }

        private void SaveImage(Stream image, string name)
        {
            var filePath = BuildServerPath(name);
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }

            var imageBytesArray = ConvertStreamToByteArray(image);
            using (var writer = new BinaryWriter(File.OpenWrite(filePath))) {
                writer.Write(imageBytesArray);
            }
        }

        public byte[] ConvertStreamToByteArray(Stream stream)
        {
            byte[] bytes;
            using (var binaryReader = new BinaryReader(stream)) {
                bytes = binaryReader.ReadBytes((int)stream.Length);
            }
            return bytes;
        }

        private byte[] GetImage(string name)
        {
            string filePath = BuildServerPath(name);
            if (File.Exists(filePath)) {                
                return ConvertStreamToByteArray(File.OpenRead(filePath));
            } else {
                return null;
            }
        }
    }
}