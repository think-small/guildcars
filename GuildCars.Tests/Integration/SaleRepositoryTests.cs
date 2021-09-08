using FluentAssertions;
using GuildCars.Data;
using GuildCars.Models;
using GuildCars.Models.Contexts;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GuildCars.Tests.Integration
{
    [TestFixture]
    public class SaleRepositoryTests
    {
        [SetUp]
        public async Task Init()
        {
            var context = new GCContext();
            await context.Database.ExecuteSqlCommandAsync("GCEFTestReset");
        }

        [Test]
        [Category("SaleRepository")]
        public async Task Able_To_Retrieve_All_Sales()
        {
            var sut = new SaleRepository();
            var actual = await sut.GetAll();
            actual.Should().NotBeNullOrEmpty().And.HaveCount(3);
        }

        [Test]
        [Category("SaleRepository")]
        public async Task Can_Add_Sale_To_Database()
        {
            var sut = new SaleRepository();
            var today = DateTime.Today;
            var expected = new SaleRecord
            {
                CustomerId = "00000000-0000-0000-0000-00000000",
                EmployeeId = "12345678-1234-1234-1234-12345678",
                VehicleId = 3,
                PurchasePrice = 10000,
                ExpectedSalePrice = 20000,
                Date = today,
                TradeInId = null,
                PurchaseTypeId = 1
            };

            await sut.Add(expected);
            var actual = await sut.GetAll();

            actual.OrderByDescending(s => s.Id)
                  .First()
                  .Should().BeEquivalentTo(expected, config => config.Excluding(p => p.Id));
            actual.Should().NotBeNullOrEmpty().And.HaveCount(4);
        }

        [TestCase("12345678-1234-1234-1234-12345678", 3)]
        [TestCase("Non-existent employee id", 0)]
        [Category("SaleRepository")]
        public async Task Can_Get_Sales_By_Employee_Id(string id, int expectedNumberOfSales)
        {
            var sut = new SaleRepository();
            var actual = await sut.GetByEmployee(id);

            actual.Should().HaveCount(expectedNumberOfSales);
        }

        [TestCase("00000000-0000-0000-0000-00000000", 2)]
        [TestCase("11111111-1111-1111-1111-11111111", 1)]
        [TestCase("Non-existent customer id", 0)]
        [Category("SaleRepository")]
        public async Task Can_Get_Sales_By_Customer_Id(string id, int expectedNumberOfSales)
        {
            var sut = new SaleRepository();
            var actual = await sut.GetByCustomer(id);
            actual.Should().HaveCount(expectedNumberOfSales);
        }

        [TestCase(2016, 12, 1, 2016, 12, 30, 1)]
        [TestCase(2014, 1, 1, 2014, 12, 30, 2)]
        [TestCase(2014, 12, 30, 2014, 1, 1, 0)]
        [TestCase(2013, 1, 1, 2013, 12, 30, 0)]
        [TestCase(2014, 9, 2, 2014, 9, 2, 1)]
        [Category("SaleRepository")]
        public async Task Can_Get_Sales_Between_Dates(int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay, int numberOfSales)
        {
            var sut = new SaleRepository();
            var startDate = new DateTime(startYear, startMonth, startDay);
            var endDate = new DateTime(endYear, endMonth, endDay);

            var actual = await sut.GetByDateRange(startDate, endDate);

            actual.Should().HaveCount(numberOfSales);
        }

        [Test]
        [Category("SaleRepository")]
        public async Task Can_Get_Sale_Record_By_Id()
        {
            var sut = new SaleRepository();
            var expected = new SaleRecord
            {
                Id = 3,
                CustomerId = "00000000-0000-0000-0000-00000000",
                EmployeeId = "12345678-1234-1234-1234-12345678",
                VehicleId = 4,
                PurchasePrice = 29000M,
                ExpectedSalePrice = 29000M,
                Date = new DateTime(2014, 9, 2).Date,
                TradeInId = 2,
                PurchaseTypeId = 2
            };

            var actual = await sut.GetById(3);
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        [Category("SaleRepository")]
        public async Task Can_Edit_Details_Of_A_Sale()
        {
            var sut = new SaleRepository();
            var expected = new SaleRecord
            {
                Id = 2,
                CustomerId = "00000000-0000-0000-0000-00000000",
                EmployeeId = "99999999-9999-9999-9999-99999999",
                VehicleId = 5,
                PurchasePrice = 1,
                ExpectedSalePrice = 1000000,
                Date = DateTime.Today,
                TradeInId = null,
                PurchaseTypeId = 3
            };

            await sut.Edit(expected);
            var actual = await sut.GetById(2);

            actual.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Test]
        [Category("SaleRepository")]
        public async Task Can_Delete_Sale_Record()
        {
            var sut = new SaleRepository();
            var deletedSale = new SaleRecord
            {
                Id = 2,
                CustomerId = "11111111-1111-1111-1111-11111111",
                EmployeeId = "12345678- 1234-1234-1234-12345678",
                VehicleId = 2,
                PurchasePrice = 40000M,
                ExpectedSalePrice = 45399.99M,
                Date = new DateTime(2016, 12, 2).Date,
                TradeInId = null,
                PurchaseTypeId = 1
            };

            await sut.Delete(2);
            var allSales = await sut.GetAll();

            allSales.Should().NotContain(deletedSale);
        }

        [Test]
        [Category("SaleRepository")]
        public async Task Records_Unchanged_If_Trying_To_Delete_Invalid_Id()
        {
            var sut = new SaleRepository();

            await sut.Delete(1000);
            var allSales = await sut.GetAll();

            allSales.Should().NotBeNullOrEmpty().And.HaveCount(3);
        }

        [TestCase(1, "cash")]
        [TestCase(2, "bank finance")]
        [TestCase(3, "dealer finance")]
        [Category("SaleRepository")]
        public async Task Can_Retrieve_Correct_PurchaseType_By_Id(int id, string expected)
        {
            var sut = new SaleRepository();

            var actual = await sut.GetPurchaseType(id);

            actual.Name.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [TestCase(1, 2)]
        [TestCase(2, 1)]
        [Category("SaleRepository")]
        public async Task Can_Retrieve_SaleRecords_For_A_Make_Of_Car(int makeId, int expectedSales)
        {
            var sut = new SaleRepository();

            var actual = await sut.GetSalesForMake(makeId);

            actual.Should().NotBeNullOrEmpty()
                  .And.HaveCount(expectedSales)
                  .And.OnlyContain(s => s.Vehicle.Model.Make != null);
        }

        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(4, 1)]
        [Category("SaleRepository")]
        public async Task Can_Retrieve_SaleRecords_For_A_Model_Of_Car(int modelId, int expectedSales)
        {
            var sut = new SaleRepository();

            var actual = await sut.GetSalesForModel(modelId);

            actual.Should().NotBeNull()
                  .And.HaveCount(expectedSales)
                  .And.OnlyContain(s => s.Vehicle.Model != null);
        }

        [TestCase(3, 0)]
        [TestCase(1000, 0)]
        [Category("SaleRepository")]
        public async Task Get_Empty_Collection_If_Model_Was_Not_Sold_Or_Does_Not_Exist(int modelId, int expectedSales)
        {
            var sut = new SaleRepository();

            var actual = await sut.GetSalesForModel(modelId);

            actual.Should().NotBeNull().And.BeEmpty();
        }
    }
}
