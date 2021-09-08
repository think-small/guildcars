using FluentAssertions;
using GuildCars.Data;
using GuildCars.Models;
using GuildCars.Models.Contexts;
using GuildCars.Models.Exceptions;
using GuildCars.Models.SaleProcessServiceModels;
using GuildCars.Services;
using GuildCars.Services.FileUploadService;
using GuildCars.Services.ReceiptGeneratorService;
using GuildCars.Services.SaleProcessorService;
using GuildCars.Services.SaleProcessorService.SaleTypes;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GuildCars.Tests.Integration
{
    [TestFixture]
    public class SaleProcessServiceTests
    {
        readonly string testDirectory = @"C:\Users\Barry\Desktop\Repos\online-net-think-small\Summatives\mastery-car\GuildCars\GuildCars.Tests\DummyData";

        [SetUp]
        public async Task Init()
        {
            using (var context = new GCContext())
            {
                await context.Database.ExecuteSqlCommandAsync("GCEFTestReset");
            }
        }

        [OneTimeTearDown]
        public async Task GlobalTearDown()
        {
            foreach (var file in new DirectoryInfo(testDirectory).GetFiles())
            {
                if (file.Name.Contains("TEST-"))
                {
                    file.Delete();
                }
            }

            using (var context = new GCContext())
            {
                await context.Database.ExecuteSqlCommandAsync("GCEFTestReset");
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [Category("SaleProcessService")]
        public async Task Processed_Sale_Without_Trade_In_Will_Transfer_Ownership_To_Customer(int saleType)
        {
            var vehicleRepo = new VehicleRepository();
            var vehiclesOwnedByDealer = await vehicleRepo.GetVehiclesOwnedBy(null);
            var soldVehicle = vehiclesOwnedByDealer.First();
            var sut = new SaleProcessorService(new SaleFactory(new LoanCalculatorService(), new UploadToDiskService(), new IronPdfReceiptService()), new VehicleRepository(), new SaleRepository());
            var sale = new SaleProcessServiceArgs
            {
                Vehicle = soldVehicle,
                PurchasePrice = soldVehicle.SalePrice,
                PurchaseTypeId = saleType,
                CustomerId = "11111111-1111-1111-1111-11111111",
                DownPayment = soldVehicle.SalePrice,
                LoanLength = 5,
                InterestRate = 0.05M,
                ApprovalAmount = soldVehicle.SalePrice,
                ApprovalLetter = new FileUploadArgs
                {
                    FileName = "TEST-Test approval letter",
                    DirectoryPath = testDirectory,
                    Extension = ".txt",
                    Data = new MemoryStream()
                }
            };

            await sut.Process(sale);
            var actual = await vehicleRepo.GetById(soldVehicle.Id);

            actual.OwnerId.Should().Be(sale.CustomerId);
        }

        [TestCase(2)]
        [TestCase(3)]
        [Category("SaleProcessService")]
        public async Task Processed_Finance_Sale_With_Trade_In_Transfers_Ownership_Of_Both_Cars_Correctly(int saleType)
        {
            var vehicleRepo = new VehicleRepository();
            var vehiclesOwnedByDealer = await vehicleRepo.GetVehiclesOwnedBy(null);
            var soldVehicle = vehiclesOwnedByDealer.First();
            var vehiclesOwnedByCustomer = await vehicleRepo.GetVehiclesOwnedBy("11111111-1111-1111-1111-11111111");
            var tradeIn = vehiclesOwnedByCustomer.First();
            var sut = new SaleProcessorService(new SaleFactory(new LoanCalculatorService(), new UploadToDiskService(), new IronPdfReceiptService()), new VehicleRepository(), new SaleRepository());
            var sale = new SaleProcessServiceArgs
            {
                Vehicle = soldVehicle,
                PurchasePrice = soldVehicle.SalePrice,
                PurchaseTypeId = saleType,
                CustomerId = "11111111-1111-1111-1111-11111111",
                DownPayment = 2500M,
                LoanLength = 3,
                InterestRate = 0.07M,
                TradeIn = tradeIn,
                ApprovalAmount = soldVehicle.SalePrice,
                ApprovalLetter = new FileUploadArgs
                {
                    FileName = "TEST-Test approval letter",
                    DirectoryPath = testDirectory,
                    Extension = ".txt",
                    Data = new MemoryStream()
                }
            };

            await sut.Process(sale);
            var actualSoldVehicle = await vehicleRepo.GetById(soldVehicle.Id);
            var actualTradeIn = await vehicleRepo.GetById(tradeIn.Id);

            actualSoldVehicle.OwnerId.Should().Be(sale.CustomerId);
            actualTradeIn.OwnerId.Should().BeNull();
        }

        [TestCase(1)]
        [TestCase(2)]
        [Category("SaleProcessService")]
        public async Task Throw_Error_When_Customer_Does_Not_Pay_PurchasePrice(int saleType)
        {
            var vehicleRepo = new VehicleRepository();
            var vehiclesOwnedByDealer = await vehicleRepo.GetVehiclesOwnedBy(null);
            var soldVehicle = vehiclesOwnedByDealer.First();
            var sut = new SaleProcessorService(new SaleFactory(new LoanCalculatorService(), new UploadToDiskService(), new IronPdfReceiptService()), new VehicleRepository(), new SaleRepository());
            var sale = new SaleProcessServiceArgs
            {
                PurchaseTypeId = saleType,
                Vehicle = soldVehicle,
                PurchasePrice = soldVehicle.SalePrice,
                CustomerId = "11111111-1111-1111-1111-11111111",
                DownPayment = 0M,
                LoanLength = 1,
                InterestRate = 0.1M,
                ApprovalAmount = soldVehicle.SalePrice - 5000
            };

            Func<Task> act = async () => await sut.Process(sale);

            act.Should().Throw<InsufficientFundsException>();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [Category("SaleProcessService")]
        public async Task Throw_Error_If_Purchase_Information_Is_Missing(int purchaseType)
        {
            var vehicleRepo = new VehicleRepository();
            var sut = new SaleProcessorService(new SaleFactory(new LoanCalculatorService(), new UploadToDiskService(), new IronPdfReceiptService()), new VehicleRepository(), new SaleRepository());
            var sale = new SaleProcessServiceArgs
            {
                PurchaseTypeId = purchaseType,
                Vehicle = await vehicleRepo.GetById(1)
            };

            Func<Task> act = async () => await sut.Process(sale);

            act.Should().Throw<SaleInformationMissingException>();
        }

        [Test]
        [Category("SaleProcessService")]
        public async Task Adds_Correct_Sale_Record_To_Database_For_Valid_Sale_Without_Trade_In()
        {
            var saleRepo = new SaleRepository();
            var vehicleRepo = new VehicleRepository();
            var sut = new SaleProcessorService(new SaleFactory(new LoanCalculatorService(), new UploadToDiskService(), new IronPdfReceiptService()), new VehicleRepository(), new SaleRepository());
            var vehicle = await vehicleRepo.GetById(5);
            var expected = new SaleRecord
            {
                Id = 4,
                CustomerId = "11111111-1111-1111-1111-11111111",
                EmployeeId = "12345678-1234-1234-1234-12345678",
                VehicleId = 5,
                TradeInId = null,
                PurchasePrice = 10000,
                ExpectedSalePrice = 18999,
                PurchaseTypeId = 1,
                Date = DateTime.Today.Date
            };
            var sale = new SaleProcessServiceArgs
            {               
                Vehicle = vehicle,
                PurchasePrice = 10000,
                TradeIn = null,
                PurchaseTypeId = 1,
                CustomerId = "11111111-1111-1111-1111-11111111",
                EmployeeId = "12345678-1234-1234-1234-12345678",
                DownPayment = 10000
            };

            await sut.Process(sale);
            var actual = await saleRepo.GetAll();

            actual.Should().NotBeNullOrEmpty().And.HaveCount(4);
            actual.OrderByDescending(s => s.Date).First().Should().BeEquivalentTo(expected);
        }

        [Test]
        [Category("SaleProcessService")]
        public async Task Add_Correct_Sale_To_Database_For_Valid_Sale_With_A_Trade_In()
        {
            var vehicleRepo = new VehicleRepository();
            var saleRepo = new SaleRepository();
            var sut = new SaleProcessorService(new SaleFactory(new LoanCalculatorService(), new UploadToDiskService(), new IronPdfReceiptService()), new VehicleRepository(), new SaleRepository());
            var customer = "11111111-1111-1111-1111-11111111";
            var tradeIn = new Vehicle
            {
                ModelId = 1,
                IsNew = false,
                BodyStyleId = 1,
                Engine = "There is one",
                Year = 2000,
                TransmissionTypeId = 1,
                Color = "Blue",
                Interior = "Blue",
                Mileage = 125983M,
                VIN = "SLK209ELKD02L",
                MSRP = 4000M,
                SalePrice = 4000M,
                Description = "Piece of junk...",
                ImagePaths = null,
                OwnerId = customer
            };
            await vehicleRepo.Add(tradeIn);
            tradeIn = await vehicleRepo.GetById(6);
            var sale = new SaleProcessServiceArgs
            {
                Vehicle = await vehicleRepo.GetById(5),
                PurchasePrice = 10000,
                TradeIn = tradeIn,
                PurchaseTypeId = 1,
                CustomerId = customer,
                EmployeeId = "12345678-1234-1234-1234-12345678",
                DownPayment = 10000
            };
            var expected = new SaleRecord
            {
                Id = 4,
                CustomerId = customer,
                EmployeeId = "12345678-1234-1234-1234-12345678",
                VehicleId = 5,
                TradeInId = tradeIn.Id,
                PurchasePrice = 10000,
                ExpectedSalePrice = 18999,
                PurchaseTypeId = 1,
                Date = DateTime.Today.Date
            };

            await sut.Process(sale);
            var actual = await saleRepo.GetAll();

            actual.Should().NotBeNullOrEmpty().And.HaveCount(4);
            actual.OrderByDescending(s => s.Date).First().Should().BeEquivalentTo(expected);
        }
    }
}
