namespace wakeApp.Services
{
    public interface IUploadService
    {
        string UploadImage(IFormFile formFile);
        string UploadVideo(IFormFile formFile); 
    }
}
