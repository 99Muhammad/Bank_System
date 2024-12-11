using BankSystemProject.Repositories.Interface;

namespace BankSystemProject.Repositories.Service
{
    public class ImageService : IImage
    {
        public bool DeleteImage(string imagePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveImageAsync(IFormFile imageFile)
        {
            throw new NotImplementedException();
        }
    }
}
