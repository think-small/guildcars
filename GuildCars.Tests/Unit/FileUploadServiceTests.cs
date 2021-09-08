using FluentAssertions;
using GuildCars.Models.SaleProcessServiceModels;
using GuildCars.Services.FileUploadService;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GuildCars.Tests.Unit
{
    [SetUpFixture]
    public class FileUploadServiceTestsSetup
    {
        readonly string testDirectory = @"C:\Users\Barry\Desktop\Repos\online-net-think-small\Summatives\mastery-car\GuildCars\GuildCars.Tests\DummyData";
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            using (var writer = File.CreateText(testDirectory + @"\TEST-SeedTestFile.txt"))
            {
                writer.WriteLine("File to be used for FileUploadService.");
                writer.WriteLine("This file is to be saved to a stream and then re-written to disk.");
            }

            using (var writer = File.CreateText(testDirectory + @"\TEST-DuplicateTestFile.txt"))
            {
                writer.WriteLine("File for use with FileUploadServiceTests.");
                writer.WriteLine("Intended to test service's handling of file with duplicate name");
            }
        }

        [OneTimeTearDown]
        public void GlobalTearDown()
        {
            foreach (var file in new DirectoryInfo(testDirectory).GetFiles())
            {
                if (file.Name.Contains("TEST-"))
                {
                    file.Delete();
                }
            }
        }
    }

    [TestFixture]
    public class FileUploadServiceTests
    {
        string seedFile = @"C:\Users\Barry\Desktop\Repos\online-net-think-small\Summatives\mastery-car\GuildCars\GuildCars.Tests\DummyData\TEST-SeedTestFile.txt";
        string dirPath = @"C:\Users\Barry\Desktop\Repos\online-net-think-small\Summatives\mastery-car\GuildCars\GuildCars.Tests\DummyData";
        [Test]
        [Category("UploadToDiskService")]
        public async Task CanCreateNewFileOnDisk()
        {
            var uploadService = new UploadToDiskService();

            var file = new FileUploadArgs
            {
                DirectoryPath = dirPath,
                FileName = "TEST-Rewritten Test File",
                Extension = ".txt",
                Data = new MemoryStream()
            };

            await uploadService.SaveAsync(file);

            GetFileNamesFrom(file.DirectoryPath)
                .Should().NotBeNullOrEmpty()
                .And.Contain(file.FileName + file.Extension);
        }

        [Test]
        [Category("UploadToDiskService")]
        public async Task AppendsIncrementingNumberToFileNameIfItAlreadyExists()
        {
            var uploadService = new UploadToDiskService();

            var file = new FileUploadArgs
            {
                DirectoryPath = dirPath,
                FileName = "TEST-DuplicateTestFile",
                Extension = ".txt",
                Data = new MemoryStream()
            };

            await uploadService.SaveAsync(file);
            
            GetFileNamesFrom(file.DirectoryPath)
                .Should().NotBeNullOrEmpty()
                .And.Contain("TEST-DuplicateTestFile1.txt");
        }

        private List<string> GetFileNamesFrom(string directory)
        {
            var files = new DirectoryInfo(directory).GetFiles();
            return files.Select(f => f.Name).ToList();
        }

        [Test]
        [Category("UploadToDiskService")]
        public async Task Can_Delete_And_Replace_All_Files_In_Directory()
        {
            var uploadService = new UploadToDiskService();

            var file = new FileUploadArgs
            {
                DirectoryPath = dirPath,
                FileName = "TEST-EditedFile",
                Extension = ".txt",
                Data = new MemoryStream()
            };

            await uploadService.EditAsync(new List<FileUploadArgs> { file });

            GetFileNamesFrom(file.DirectoryPath).
                Should().NotBeNullOrEmpty()
                .And.HaveCount(1)
                .And.Contain("TEST-EditedFile.txt");
        }
    }
}
