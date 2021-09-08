using FluentAssertions;
using GuildCars.Data;
using GuildCars.Models;
using GuildCars.Models.Contexts;
using GuildCars.Models.Exceptions;
using GuildCars.Models.QueryParams;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuildCars.Tests.Integration
{    
    [TestFixture]
    public class VehicleRepositoryTests
    {
        [SetUp]
        public async Task Init()
        {
            var context = new GCContext();
            await context.Database.ExecuteSqlCommandAsync("GCEFTestReset");
        }

        [Test]
        [Category("Add")]
        public async Task Add_InsertsNewRecordIntoVehiclesTable()
        {
            var owner = "00000000-0000-0000-0000-00000000";
            var expected = new Vehicle
            {
                ModelId = 2,
                Model = new Model { Id = 2, MakeId = 1, Make = new Make { Id = 1, Name = "Mazda" }, Name = "CX-5" },
                IsNew = true,
                BodyStyleId = 8,
                BodyStyle = new BodyStyle { Id = 8, Name = "Subcompact" },
                Engine = "There is one",
                Year = 2021,
                TransmissionTypeId = 1,
                TransmissionType = new TransmissionType { Id = 1, Name = "Automatic" },
                Color = "Blue",
                Interior = "Gray",
                Mileage = 75.1M,
                VIN = "WDCGG5HB4FG358810",
                MSRP = 35399.99M,
                SalePrice = 35399.99M,
                Description = "New Carbon Edition Mazda CX-5 is a powerful four-cylinder, front-wheel drive, six-speed automatic daily driver. This vehicle has a luxury aura without the luxury price.",
                ImagePaths = new List<ImagePath>(),
                OwnerId = owner
            };
            var sut = new VehicleRepository();

            await sut.Add(expected);
            var actual = await sut.GetVehiclesOwnedBy(owner);

            actual.Should().NotBeNullOrEmpty()
                .And.HaveCount(4)
                .And.ContainEquivalentOf(expected);
        }

        [Test]
        [Category("GetAll")]
        public async Task GetAll_ReturnsEveryVehicle()
        {
            var sut = new VehicleRepository();

            var actual = await sut.GetAll();

            actual.Should().NotBeNullOrEmpty()
                .And.HaveCount(5);
        }

        [Test]
        [Category("GetById")]
        public async Task GetById_ReturnsSingleCarWithExpectedId()
        {
            var sut = new VehicleRepository();

            var actual = await sut.GetById(1);
            var expected = new Vehicle
            {
                Id = 1,
                ModelId = 1,
                Model = await sut.GetMakeAndModel(1),
                IsNew = false,
                BodyStyleId = 8,
                BodyStyle = new BodyStyle { Id = 8, Name = "Subcompact" },
                Year = 2020,
                TransmissionTypeId = 1,
                TransmissionType = new TransmissionType {  Id = 1, Name = "Automatic" },
                Color = "Blue",
                Interior = "Gray",
                Mileage = 75.10M,
                VIN = "WDCGG5HB4FG358810",
                MSRP = 35399.99M,
                SalePrice = 35399.99M,
                Description = "New Carbon Edition Mazda CX-3 is a powerful four-cylinder, front-wheel drive, six speed automatic daily driver. This vehicle has the aura of luxury without the price tag.",
                OwnerId = "00000000-0000-0000-0000-00000000",
                ImagePaths = new List<ImagePath>(),
                HighwayMpg = 34,
                CityMpg = 29,
                Engine = "I-4 2.0 L/122",
                Details = new List<Detail>
                {
                    new Detail { Id = 1, Type = DetailType.Exterior, Name = "Brakelights", Description = "LED Brakelights", IsKeyFeature = false },
                    new Detail { Id = 2, Type = DetailType.Exterior, Name = "Spoiler", Description = "Lip Spoiler", IsKeyFeature = false },
                    new Detail { Id = 3, Type = DetailType.Exterior, Name = "Wipers", Description = "Rain Detecting Variable Intermittent Wipers", IsKeyFeature = false },
                    new Detail { Id = 4, Type = DetailType.Exterior, Name = "Spare Wheel", Description = "Steel Spare Wheel", IsKeyFeature = false },
                    new Detail { Id = 5, Type = DetailType.Exterior, Name = "Tires", Description = "215/60R16 AS", IsKeyFeature = false },
                    new Detail { Id = 6, Type = DetailType.Exterior, Name = "Wheels", Description = "16x6 Aluminum Alloy Wheels with Silver Accents", IsKeyFeature = false },
                    new Detail { Id = 7, Type = DetailType.Exterior, Name = "Paint", Description = "Clearcoat Paint", IsKeyFeature = false },
                    new Detail { Id = 8, Type = DetailType.Exterior, Name = "Rear Window", Description = "Fixed Rear Window with Fixed Interval Wiper and Defroster", IsKeyFeature = false },
                    new Detail { Id = 9, Type = DetailType.Interior, Name = "Outlet", Description = "1 12V DC Outlet", IsKeyFeature = false },
                    new Detail { Id = 10, Type = DetailType.Interior, Name = "Seatback Storage", Description = "2 Steaback Storage Pocket", IsKeyFeature = false },
                    new Detail { Id = 11, Type = DetailType.Interior, Name = "Seat Recline", Description = "4-Way Passenger Seat Manual Recline", IsKeyFeature = false },
                    new Detail { Id = 12, Type = DetailType.Interior, Name = "Air", Description = "Air Filtration", IsKeyFeature = false },
                    new Detail { Id = 13, Type = DetailType.Interior, Name = "Air", Description = "Automatic Air Conditioning", IsKeyFeature = false },
                    new Detail { Id = 14, Type = DetailType.Interior, Name = "Lights", Description = "Cargo Space Lights", IsKeyFeature = false },
                    new Detail { Id = 15, Type = DetailType.Interior, Name = "Mirror", Description = "Day/Night Rearview Mirror", IsKeyFeature = false },
                    new Detail { Id = 16, Type = DetailType.Interior, Name = "HUD", Description = "Heads Up Display", IsKeyFeature = true },
                    new Detail { Id = 17, Type = DetailType.Interior, Name = "Keyless", Description = "Remote Keyless Entry", IsKeyFeature = false },
                    new Detail { Id = 18, Type = DetailType.Interior, Name = "Keyless", Description = "Keyless Ignition", IsKeyFeature = false },
                    new Detail { Id = 19, Type = DetailType.Interior, Name = "Tech", Description = "SMS Text Message Audio Delivery and Reply", IsKeyFeature = false },
                    new Detail { Id = 20, Type = DetailType.Entertainment, Name = "Monitors", Description = "Two LCD Monitors in the Front", IsKeyFeature = false },
                    new Detail { Id = 21, Type = DetailType.Entertainment, Name = "Radio", Description = "AM/FM Radio", IsKeyFeature = false },
                    new Detail { Id = 22, Type = DetailType.Entertainment, Name = "Infotainment", Description = "Infotainment System Voice Command", IsKeyFeature = true },
                    new Detail { Id = 23, Type = DetailType.Entertainment, Name = "Radio", Description = "Pandora Internet Radio", IsKeyFeature = true },
                    new Detail { Id = 24, Type = DetailType.Entertainment, Name = "Bluetooth", Description = "Bluetooth 5.1", IsKeyFeature = true },
                    new Detail { Id = 25, Type = DetailType.Mechanical, Name = "Alternator", Description = "100 Amp Alternator", IsKeyFeature = false },
                    new Detail { Id = 26, Type = DetailType.Mechanical, Name = "Fuel Tank", Description = "12.7 Gallon Fuel Tank", IsKeyFeature = false },
                    new Detail { Id = 27, Type = DetailType.Mechanical, Name = "Axle", Description = "4.325 Axle Ratio", IsKeyFeature = false },
                    new Detail { Id = 28, Type = DetailType.Mechanical, Name = "FWD", Description = "Front Wheel Drive", IsKeyFeature = false },
                    new Detail { Id = 29, Type = DetailType.Safety, Name = "ABS", Description = "ABS and Driveline Traction Control", IsKeyFeature = false },
                    new Detail { Id = 30, Type = DetailType.Safety, Name = "Airbag", Description = "Airbag Occupancy Sensor", IsKeyFeature = false },
                    new Detail { Id = 31, Type = DetailType.Safety, Name = "Backup Camera", Description = "Back-up Camera", IsKeyFeature = true },
                    new Detail { Id = 32, Type = DetailType.Safety, Name = "BSM", Description = "Blind Spot Monitoring", IsKeyFeature = true },
                    new Detail { Id = 33, Type = DetailType.Safety, Name = "Child Lock", Description = "Rear Child Safety Locks", IsKeyFeature = false }
                }
            };

            actual.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Test]
        [Category("GetVehiclesOwnedBy")]
        public async Task GetVehiclesOwnedBy_RetrieveAllCarsOwnedByGivenUserId()
        {
            var sut = new VehicleRepository();

            var actual = await sut.GetVehiclesOwnedBy("00000000-0000-0000-0000-00000000");

            actual.Should().NotBeNullOrEmpty()
                .And.HaveCount(3);
        }
        [Test]
        [Category("GetVehiclesOwnedBy")]
        public async Task GetVehiclesOwnedBy_ReturnEmptyListIfUserHasNoCars()
        {
            var sut = new VehicleRepository();

            var actual = await sut.GetVehiclesOwnedBy("Invalid UserId Should Return Empty List");

            actual.Should().NotBeNull().And.BeEmpty();
        }
        [Test]
        [Category("GetVehiclesOwnedBy")]
        public async Task GetVehiclesOwnedBy_PassingNullShouldGiveIEnumerableOfVehiclesWithoutUserIdListed()
        {
            var sut = new VehicleRepository();

            var vehiclesOwnedByNull = await sut.GetVehiclesOwnedBy(null);
            var actual = vehiclesOwnedByNull.ToList()[0];

            vehiclesOwnedByNull.Should().NotBeNullOrEmpty()
                .And.HaveCount(1);
            actual.OwnerId.Should().BeNull();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(5)]
        [Category("Delete")]
        public void Cannot_Delete_Vehicle_If_It_Still_Has_Records_Of_Sale(int id)
        {
            var sut = new VehicleRepository();
            Func<Task> actual = async () => await sut.Delete(id);

            actual.Should().Throw<CascadeDeleteException>();
        }
        [Test]
        [Category("Delete")]
        public async Task Can_Delete_Vehicle_If_It_Has_No_Records_Of_Sale()
        {
            var sut = new VehicleRepository();
            
            await sut.Delete(3);
            var actual = await sut.GetById(3);

            actual.Should().BeNull();
        }
        [Test]
        [Category("Delete")]
        public async Task Delete_RecordsRemainUnchangedIfInvalidId()
        {
            var sut = new VehicleRepository();
            await sut.Delete(100);

            var actual = await sut.GetAll();

            actual.Should().NotBeNullOrEmpty().And.HaveCount(5);
        }

        [Test]
        [Category("Edit")]
        public async Task Edit_UpdatesVehicleProperties()
        {
            var sut = new VehicleRepository();
            var expected = new Vehicle
            {
                Id = 1,
                ModelId = 4,
                Model = await sut.GetMakeAndModel(4),
                IsNew = true,
                BodyStyleId = 3,
                BodyStyle = new BodyStyle
                {
                    Id = 3,
                    Name = "Sports Car"
                },
                Engine = "There is one",
                Year = 2000,
                TransmissionTypeId = 2,
                TransmissionType = new TransmissionType
                {
                    Id = 2,
                    Name = "Manual"
                },
                Color = "Blue",
                Interior = "Gray",
                Mileage = 1000M,
                VIN = "WDCGG5HB4FG358820",
                MSRP = 10999M,
                SalePrice = 10999M,
                Description = "Updated car",
                OwnerId = "11111111-1111-1111-1111-11111111",
                Details = new List<Detail>
                {
                    new Detail
                    {
                        Id = 1,
                        Type = DetailType.Exterior,
                        Name = "Brakelights",
                        Description = "LED Brakelights",
                        IsKeyFeature = false,
                        Vehicles = null
                    }
                },
                ImagePaths = new List<ImagePath>
                {
                    new ImagePath { Id = 100, Path = "this is a path to an image file", VehicleId = 1 }
                }
            };

            var id = await sut.Edit(expected);
            var actual = await sut.GetById(id);

            actual.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Test]
        [Category("Edit")]
        public void Edit_RecordsLeftUnChangedIfInvalidId()
        {
            var sut = new VehicleRepository();
            var expected = new Vehicle
            {
                Id = 100,
                ModelId = 5000,
                IsNew = true,
                BodyStyleId = 3,
                Year = 2000,
                TransmissionTypeId = 2,
                Color = "Blue",
                Interior = "Gray",
                Mileage = 1000M,
                VIN = "WDCGG5HB4FG358820",
                MSRP = 10999M,
                SalePrice = 10999M,
                Description = "Updated car",
                OwnerId = "11111111-1111-1111-1111-11111111"
            };

            Func<Task> actual = async () => await sut.Edit(expected);
            actual.Should()
                .Throw<VehicleNotFoundException>()
                .WithMessage("Invalid Id (100). Unable to update vehicle.");
        }

        [Test]
        [Category("FilterBy")]
        public async Task Can_Retrieve_Vehicles_When_All_Filter_Parameters_Are_Used()
        {
            var sut = new VehicleRepository();
            var queryArgs = new VehicleFilter
            {
                VehicleConditions = new string[] { "new" },
                MinPrice = 10000M,
                MaxPrice = 20000M,
                BodyStyles = new int[] { 1, 2, 3 },
                Models = new int[] { 1, 2, 3, 4, 5 },
                Makes = new int[] { 2, 3 },
                Transmissions = new int[] { 1 },
                IsSearchingForAvailableCars = true
            };

            var actual = await sut.FilterBy(queryArgs);
            var expected = new List<Vehicle>
            {
                new Vehicle
                {
                    Id = 5,
                    Year = 2021,
                    Color = "Orange",
                    Interior = "Black",
                    Mileage = 55.90M,
                    VIN = "WDCGG5HB4FG358815",
                    MSRP = 18999.00M,
                    SalePrice = 18999.00M,
                    Description = "Please buy this...",
                    OwnerId = null,
                    ModelId = 5,
                    BodyStyleId = 1,
                    TransmissionTypeId = 1,
                    IsNew = true,
                    HighwayMpg = 40,
                    CityMpg = 33,
                    Engine = "I-4 1.5L/91",
                    Model = new Model
                    {
                        Id = 5,
                        Name = "Fit",
                        MakeId = 2,
                        Make = new Make
                        {
                            Id = 2,
                            Name = "Honda"
                        }
                    },
                    ImagePaths = new List<ImagePath>
                    {
                        new ImagePath
                        {
                            Path = "Honda/Fit/fit-1.jpg",
                            Id = 6,
                            VehicleId = 5
                        }
                    }
                }
            };

            actual.Should().NotBeNullOrEmpty()
                  .And.HaveCount(1)
                  .And.BeEquivalentTo(expected);
        }
        [Test]
        [Category("FilterBy")]
        public async Task Can_Get_All_Used_Vehicles()
        {
            var sut = new VehicleRepository();
            var queryArgs = new VehicleFilter
            {
                VehicleConditions = new string[] { "used" },
                MinPrice = 0M,
                MaxPrice = 100000M,
                BodyStyles = new int[0],
                Models = new int[0],
                Makes = new int[0],
                Transmissions = new int[0],
                IsSearchingForAvailableCars = false
            };
            var expected = new List<Vehicle>
            {
                new Vehicle
                {
                    Id = 3,
                    Year = 2019,
                    Color = "Titanium",
                    Interior = "Gray",
                    Mileage = 2150.20M,
                    VIN = "WDCGGHB4FG358812",
                    MSRP = 27999.00M,
                    SalePrice = 27999.00M,
                    Description = "It still works, what more do you want?",
                    OwnerId = "00000000-0000-0000-0000-00000000",
                    ModelId = 3,
                    BodyStyleId = 1,
                    TransmissionTypeId = 1,
                    IsNew = false,
                    HighwayMpg = 38,
                    CityMpg = 30,
                    Engine = "I-4 1.5L/91",
                    Model = new Model
                    {
                        Id = 3,
                        Name = "Accord",
                        MakeId = 2,
                        Make = new Make
                        {
                            Id = 2,
                            Name = "Honda"
                        }
                    },
                    ImagePaths = new List<ImagePath>()
                },
                new Vehicle
                {
                    Id = 4,
                    Year = 2021,
                    Color = "Red",
                    Interior = "White",
                    Mileage = 1121.70M,
                    VIN = "WDCGGHB4FG358814",
                    MSRP = 29899.00M,
                    SalePrice = 29899.00M,
                    Description = "It is popular, you should probably won one too",
                    OwnerId = "00000000-0000-0000-0000-00000000",
                    ModelId = 4,
                    BodyStyleId = 1,
                    TransmissionTypeId = 1,
                    IsNew = false,
                    HighwayMpg = 37,
                    CityMpg = 29,
                    Engine = "I-4 2.0L/122",
                    Model = new Model
                    {
                        Id = 4,
                        Name = "Civic",
                        MakeId = 2,
                        Make = new Make
                        {
                            Id = 2,
                            Name = "Honda"
                        }
                    },
                    ImagePaths = new List<ImagePath>
                    {
                        new ImagePath
                        {
                            Id = 3,
                            Path = "Honda/Civic/civic-1.jpg",
                            VehicleId = 4
                        },
                        new ImagePath
                        {
                            Id = 4,
                            Path = "Honda/Civic/civic-2.jpg",
                            VehicleId = 4
                        },
                        new ImagePath
                        {
                            Id = 5,
                            Path = "Honda/Civic/civic-3.jpg",
                            VehicleId = 4
                        }
                    }
                },
                new Vehicle
                {
                    Id = 2,
                    Year = 2021,
                    Color = "Green",
                    Interior = "Gray",
                    Mileage = 75.10M,
                    VIN = "WDCGGHB4FG358811",
                    MSRP = 45399.99M,
                    SalePrice = 45399.99M,
                    Description = "New Carbon Edition Mazda CX-5 is a powerful four-cylinder, front wheel drive, six speed automatic daily driver. This vehicle has the aura of luxury without the price tag.",
                    OwnerId = "11111111-1111-1111-1111-11111111",
                    ModelId = 2,
                    BodyStyleId = 8,
                    TransmissionTypeId = 1,
                    IsNew = false,
                    HighwayMpg = 30,
                    CityMpg = 24,
                    Engine = "I-4 2.5 L/152",
                    Model = new Model
                    {
                        Id = 2,
                        Name = "CX-5",
                        MakeId = 1,
                        Make = new Make
                        {
                            Id = 1,
                            Name = "Mazda"
                        }
                    },
                    ImagePaths = new List<ImagePath>
                    {
                        new ImagePath
                        {
                            Id = 1,
                            Path = "Mazda/CX5/cx5-1.jpg",
                            VehicleId = 2
                        },
                        new ImagePath
                        {
                            Id = 2,
                            Path = "Mazda/CX5/cx5-2.jpg",
                            VehicleId = 2
                        }
                    }
                },
                new Vehicle
                {
                    Id = 1,
                    Year = 2020,
                    Color = "Blue",
                    Interior = "Gray",
                    Mileage = 75.10M,
                    VIN = "WDCGG5HB4FG358810",
                    MSRP = 35399.99M,
                    SalePrice = 35399.99M,
                    Description = "New Carbon Edition Mazda CX-3 is a powerful four-cylinder, front-wheel drive, six speed automatic daily driver. This vehicle has the aura of luxury without the price tag.",
                    OwnerId = "00000000-0000-0000-0000-00000000",
                    ModelId = 1,
                    BodyStyleId = 8,
                    TransmissionTypeId = 1,
                    IsNew = false,
                    HighwayMpg = 34,
                    CityMpg = 29,
                    Engine = "I-4 2.0 L/122",
                    Model = new Model
                    {
                        Id = 1,
                        Name = "CX-3",
                        MakeId = 1,
                        Make = new Make
                        {
                            Id = 1,
                            Name = "Mazda"
                        }
                    },
                    ImagePaths = new List<ImagePath>()
                },
            };

            var actual = await sut.FilterBy(queryArgs);

            actual.Should().NotBeNullOrEmpty()
                  .And.BeEquivalentTo(expected);
        }
        [Test]
        [Category("FilterBy")]
        public async Task Get_Empty_List_If_No_Vehicles_Match_Price_Range()
        {
            var sut = new VehicleRepository();
            var queryArgs = new VehicleFilter
            {
                MinPrice = 19000M,
                MaxPrice = 10000M,
                BodyStyles = new int[0],
                Models = new int[0],
                Makes = new int[0],
                Transmissions = new int[0],
                VehicleConditions = new string[0],
                IsSearchingForAvailableCars = false
            };

            var actual = await sut.FilterBy(queryArgs);

            actual.Should().NotBeNull().And.BeEmpty();
        }

        [Test]
        [Category("FilterBy")]
        public async Task Get_Empty_List_If_No_Vehicles_Match_BodyStyles()
        {
            var sut = new VehicleRepository();
            var queryArgs = new VehicleFilter
            {
                MinPrice = 10000M,
                MaxPrice = 20000M,
                BodyStyles = new int[] { 8 },
                Models = new int[0],
                Makes = new int[0],
                Transmissions = new int[0],
                VehicleConditions = new string[0]
            };

            var actual = await sut.FilterBy(queryArgs);

            actual.Should().NotBeNull().And.BeEmpty();
        }

        [Test]
        [Category("FilterBy")]
        public async Task Get_Empty_List_If_No_Vehicles_Match_Models()
        {
            var sut = new VehicleRepository();
            var queryArgs = new VehicleFilter
            {
                MinPrice = 10000M,
                MaxPrice = 20000M,
                BodyStyles = new int[0],
                Models = new int[] { 1 },
                Makes = new int[0],
                Transmissions = new int[0],
                VehicleConditions = new string[0]
            };

            var actual = await sut.FilterBy(queryArgs);

            actual.Should().NotBeNull().And.BeEmpty();
        }
        [Test]
        [Category("FilterBy")]
        public async Task Get_Empty_List_If_No_Vehicles_Match_Makes()
        {
            var sut = new VehicleRepository();
            var queryArgs = new VehicleFilter
            {
                MinPrice = 10000M,
                MaxPrice = 20000M,
                BodyStyles = new int[0],
                Models = new int[0],
                Makes = new int[] { 1 },
                Transmissions = new int[0],
                VehicleConditions = new string[0]
            };

            var actual = await sut.FilterBy(queryArgs);

            actual.Should().NotBeNull().And.BeEmpty();
        }

        [Test]
        [Category("FilterBy")]
        public async Task Get_Empty_List_If_No_Vehicles_Match_Transmission()
        {
            var sut = new VehicleRepository();
            var queryArgs = new VehicleFilter
            {
                MinPrice = 10000M,
                MaxPrice = 20000M,
                BodyStyles = new int[0],
                Models = new int[0],
                Makes = new int[0],
                Transmissions = new int[] { 2 },
                VehicleConditions = new string[0]
            };

            var actual = await sut.FilterBy(queryArgs);

            actual.Should().NotBeNull().And.BeEmpty();
        }
        
        [Test]
        [Category("GetAllMakesAndModels")]
        public async Task Retrieve_All_Models_With_Their_Make()
        {
            var sut = new VehicleRepository();
            var makes = new List<Make>
            {
                new Make { Id = 1, Name = "Mazda" },
                new Make { Id = 2, Name = "Honda" }
            };

            var actual = await sut.GetAllMakesAndModels();
            var expected = new List<Model>
            {
                new Model { Id = 1, Name = "CX-3", MakeId = makes[0].Id, Make = makes[0] },
                new Model { Id = 2, Name = "CX-5", MakeId = makes[0].Id, Make = makes[0] },
                new Model { Id = 3, Name = "Accord", MakeId = makes[1].Id, Make = makes[1] },
                new Model { Id = 4, Name = "Civic", MakeId = makes[1].Id, Make = makes[1] },
                new Model { Id = 5, Name = "Fit", MakeId = makes[1].Id, Make = makes[1] }
            };

            actual.Should().NotBeNullOrEmpty()
                  .And.HaveCount(5)
                  .And.BeEquivalentTo(expected);
        }

        [Test]
        [Category("GetAllBodyStyles")]
        public async Task Retrieve_All_BodyStyles()
        {
            var sut = new VehicleRepository();

            var actual = await sut.GetAllBodyStyles();
            var expected = new List<BodyStyle>
            {
                new BodyStyle { Id = 1, Name = "Sedan" },
                new BodyStyle { Id = 2, Name = "Coupe" },
                new BodyStyle { Id = 3, Name = "Sports Car" },
                new BodyStyle { Id = 4, Name = "Station Wagon" },
                new BodyStyle { Id = 5, Name = "Hatchback" },
                new BodyStyle { Id = 6, Name = "Convertible" },
                new BodyStyle { Id = 7, Name = "SUV" },
                new BodyStyle { Id = 8, Name = "Subcompact" },
                new BodyStyle { Id = 9, Name = "Minivan" },
                new BodyStyle { Id = 10, Name = "Van" },
                new BodyStyle { Id = 11, Name = "Pickup" }
            };

            actual.Should().NotBeNullOrEmpty()
                  .And.HaveCount(11)
                  .And.BeEquivalentTo(expected);
        }

        [Test]
        [Category("GetAllTransmissionTypes")]
        public async Task Retrieve_All_TransmissionTypes()
        {
            var sut = new VehicleRepository();
            var expected = new List<TransmissionType>
            {
                new TransmissionType { Id = 1, Name = "Automatic" },
                new TransmissionType { Id = 2, Name = "Manual" },
                new TransmissionType { Id = 3, Name = "Automated Manual" },
                new TransmissionType { Id = 4, Name = "Continuous Variable" }
            };

            var actual = await sut.GetAllTransmissionTypes();

            actual.Should().NotBeNullOrEmpty()
                  .And.HaveCount(4)
                  .And.BeEquivalentTo(expected);
        }
    }
}
