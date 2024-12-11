namespace BankSystemProject.Repositories.Interface
{
    public interface IImage
    {
        Task<string> SaveImageAsync(IFormFile imageFile);
        bool DeleteImage(string imagePath);

    }
}
