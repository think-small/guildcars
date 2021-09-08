using GuildCars.Models.SaleProcessServiceModels;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using GuildCars.Models.Exceptions;
using System;

namespace GuildCars.Services.FileUploadService
{
    internal class UploadToDiskService : IFileUploadService
    {
        /// <summary>
        /// Deletes all files in given directory, and writes new files to disk.
        /// Each FileUploadArgs.DirectoryPath must point to the same directory, or a DirectoryMismatchException is thrown.
        /// </summary>
        public async Task EditAsync(IEnumerable<FileUploadArgs> args)
        {
            if (args.Count() == 0)
                return;
            if (args.Select(a => a.DirectoryPath).Distinct().Count() > 1)
                throw new DirectoryMismatchException();

            var directory = new DirectoryInfo(args.First().DirectoryPath);
            //DeleteFilesIn(directory);

            await SaveAllAsync(args);
        }

        private void DeleteFilesIn(DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles())
            {
                file.Delete();
            }
        }

        private async Task SaveAllAsync(IEnumerable<FileUploadArgs> args)
        {
            var saveTasks = new List<Task>();
            foreach (var file in args)
            {
                saveTasks.Add(SaveAsync(file));
            }
            await Task.WhenAll(saveTasks);
        }

        public async Task SaveAsync(FileUploadArgs args)
        {
            var filePath = GetUniqueFilePath(args);

            using (var fileStream = File.Create(filePath))
            {
                await args.Data.CopyToAsync(fileStream);
            }
        }

        private string GetUniqueFilePath(FileUploadArgs args)
        {
            int counter = 1;
            string filePath = Path.Combine(args.DirectoryPath, args.FileName + args.Extension);
            while (File.Exists(filePath))
            {
                filePath = Path.Combine(args.DirectoryPath, args.FileName + counter.ToString() + args.Extension);
                counter++;
            }

            return filePath;
        }

        public void DeleteDirectoryAndFiles(string path)
        {
            Directory.Delete(path, true);
        }

        public void DeleteFilesFrom(IEnumerable<string> imagePaths)
        {
            foreach (var imagePath in imagePaths)
            {
                File.Delete(imagePath);
            }
        }
    }
}
