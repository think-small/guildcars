using GuildCars.Models.SaleProcessServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuildCars.Services.FileUploadService
{
    public interface IFileUploadService
    {
        Task SaveAsync(FileUploadArgs args);
        Task EditAsync(IEnumerable<FileUploadArgs> args);
        void DeleteDirectoryAndFiles(string path);
        void DeleteFilesFrom(IEnumerable<string> imagePaths);
    }
}
