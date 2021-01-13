namespace Gradebook.Web.Services.Interfaces
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public interface IFileManagementService
    {
        void DeleteImage(string imageFolderName, string imageName);

        Task SaveImageAsync(string imageFolderName, string uniqueFileName, IFormFile image);
    }
}