using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace wakeApp.Services
{
    public class UploadService : IUploadService
    {
        public string UploadImage(IFormFile formFile)
        {
            Account account = new Account("imagedpy", "882864429614789", "J0ISV-xrcX_pod7fhdyLSJ06Gl4");
            Cloudinary cloudinary = new Cloudinary(account);
            var filename = formFile.FileName;

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription($@"D:/Computador/Images/Desenhos/{filename}"),
                PublicId = filename.Replace("D:/Computador/Images/Desenhos/", "").Replace(".jpg", ""),
                UploadPreset = "h2necen7",
                Folder = "thumbnails",
                ImageMetadata = true,

            };
            var uploadResult = cloudinary.Upload(uploadParams);

            return uploadResult.SecureUrl.OriginalString;
        }

        public string UploadVideo(IFormFile formFile)
        {
            Account account = new Account("imagedpy", "882864429614789", "J0ISV-xrcX_pod7fhdyLSJ06Gl4");
            Cloudinary cloudinary = new Cloudinary(account);
            var filename = formFile.FileName;

            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription($@"D:/Computador/Images/Desenhos/{filename}"),
                PublicId = filename.Replace("D:/Computador/Images/Desenhos/", "").Replace(".mp4", ""),
                UploadPreset = "w4rctdor",
                Folder = "videos",
                ImageMetadata = true,

            };
            var uploadResult = cloudinary.Upload(uploadParams);

            return uploadResult.SecureUrl.OriginalString;
        }
    }
}
