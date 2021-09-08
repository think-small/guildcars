using System.IO;

namespace GuildCars.Models.SaleProcessServiceModels
{
    public class FileUploadArgs
    {
        public string FileName { get; set; }
        public string DirectoryPath { get; set; }
        public string Extension { get; set; }
        public Stream Data { get; set; }
        public int ByteCount { get; set; }
    }
}
