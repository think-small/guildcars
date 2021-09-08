using GuildCars.Models.Exceptions;
using GuildCars.Models.SaleProcessServiceModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Services.FileUploadService
{
    internal class ImageToDiskService : IFileUploadService
    {
        private string[] _acceptedFileTypes = new string[] { ".jpg", ".jpeg", ".png" };
        private const int imageSizeLimit = 1000000;

        public async Task SaveAsync(FileUploadArgs file)
        {
            if (IsValid(file))
            {
                var filePath = GetPathFor(file);

                using (var fileStream = File.Create(filePath))
                {
                    await file.Data.CopyToAsync(fileStream);
                }
            }
        }

        private string GetPathFor(FileUploadArgs file)
        {
            int counter = 1;
            string filePath = Path.Combine(file.DirectoryPath, file.FileName + file.Extension);
            while (File.Exists(filePath))
            {
                filePath = Path.Combine(file.DirectoryPath, file.FileName + counter.ToString() + file.Extension);
                counter++;
            }

            return filePath;
        }

        private bool IsValid(FileUploadArgs file)
        {
            if (_acceptedFileTypes.Contains(file.Extension))
                throw new UnsupportedFileTypeException("Invalid image file type. Only .jpg or .png are allowed");
            if (file.ByteCount < 0 || file.ByteCount > imageSizeLimit)
                throw new InvalidFileException("File size exceeds limits.");

            return true;
        }

        public Task EditAsync(IEnumerable<FileUploadArgs> args)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectoryAndFiles(string path)
        {
            throw new NotImplementedException();
        }

        public void DeleteFilesFrom(IEnumerable<string> imagePaths)
        {
            throw new NotImplementedException();
        }
    }
}
