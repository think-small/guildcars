using FluentAssertions;
using GuildCars.Data;
using GuildCars.Models;
using GuildCars.Models.Exceptions;
using GuildCars.Models.QueryParams;
using GuildCars.Models.QueryResults;
using GuildCars.Services.FileUploadService;
using GuildCars.Services.InventoryService;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuildCars.Tests.Integration
{
    internal class InvalidVehicleFilters : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new VehicleFilter { Models = new int[0], Transmissions = new int[0], BodyStyles = new int[0], MinPrice = 10000M, MaxPrice = 20000M };
            yield return new VehicleFilter { Makes = new int[0], Transmissions = new int[0], BodyStyles = new int[0], MinPrice = 10000M, MaxPrice = 20000M };
            yield return new VehicleFilter { Makes = new int[0], Models = new int[0], BodyStyles = new int[0], MinPrice = 10000M, MaxPrice = 20000M };
            yield return new VehicleFilter { Makes = new int[0], Models = new int[0], Transmissions = new int[0], MinPrice = 10000M, MaxPrice = 20000M };
            yield return new VehicleFilter { Makes = new int[0], Models = new int[0], Transmissions = new int[0], BodyStyles = new int[0], MinPrice = 20000M, MaxPrice = 19000M };
            yield return new VehicleFilter { Makes = new int[0], Models = new int[0], Transmissions = new int[0], BodyStyles = new int[0], MinPrice = -1M, MaxPrice = 20000M };
            yield return new VehicleFilter { Makes = new int[0], Models = new int[0], Transmissions = new int[0], BodyStyles = new int[0], MinPrice = 0M, MaxPrice = 101000M };
        }
    }
    [TestFixture]
    public class InventoryServiceTests
    {
        [TestCaseSource(typeof(InvalidVehicleFilters))]
        [Category("GetVehiclesFilteredBy")]
        public void Throw_If_Invalid_Or_Missing_Filter_Information_Is_Given(VehicleFilter filter)
        {
            var sut = new InventoryService(new VehicleRepository(), new UploadToDiskService());
            Func<Task> actual = async () => await sut.GetVehiclesFilteredBy(filter);

            actual.Should().Throw<InvalidInventoryFilterException>();
        }

        [Test]
        [Category("GetAllVehicleOptions")]
        public async Task Retrieve_All_Makes_Models_Transmission_BodyStyles()
        {
            var sut = new InventoryService(new VehicleRepository(), new UploadToDiskService());

            var actual = await sut.GetAllVehicleOptions();
            var expected = new VehicleOptions
            {
                Transmissions = new List<Tuple<int, string>>
                                {
                                    new Tuple<int, string>(1, "Automatic"),
                                    new Tuple<int, string>(2, "Manual" ),
                                    new Tuple<int, string>(3, "Automated Manual"),
                                    new Tuple<int, string>(4, "Continuous Variable")
                                },
                BodyStyles = new List<Tuple<int,string>>
                            {
                                new Tuple<int, string>(1, "Sedan"),
                                new Tuple<int, string>(2, "Coupe"),
                                new Tuple<int, string>(3, "Sports Car"),
                                new Tuple<int, string>(4, "Station Wagon"),
                                new Tuple<int, string>(5, "Hatchback"),
                                new Tuple<int, string>(6, "Convertible"),
                                new Tuple<int, string>(7, "SUV"),
                                new Tuple<int, string>(8, "Subcompact"),
                                new Tuple<int, string>(9, "Minivan"),
                                new Tuple<int, string>(10,"Van"),
                                new Tuple<int, string>(11,"Pickup")
                            },
                Makes = new List<Tuple<int, string>>
                        {
                            new Tuple<int, string>(1, "Mazda"),
                            new Tuple<int, string>(2, "Honda")
                        },
                Models = new List<Tuple<int, string>>
                        {
                            new Tuple<int, string>(1, "CX-3"),
                            new Tuple<int, string>(2, "CX-5"),
                            new Tuple<int, string>(3, "Accord"),
                            new Tuple<int, string>(4, "Civic"),
                            new Tuple<int, string>(5, "Fit")
                        },
                MakesAndModels = new Dictionary<Make, IList<Model>>
                {
                    { new Make { Id = 1, Name = "Mazda" }, new List<Model> { new Model { Id = 1, Name = "CX-3" }, new Model { Id = 2, Name = "CX-5"} } },
                    { new Make { Id = 2, Name = "Honda" }, new List<Model> { new Model { Id = 3, Name = "Accord" }, new Model { Id = 4, Name = "Civic" }, new Model { Id = 5, Name = "Fit" } } }
                },
                Details = new List<Tuple<int, string>>
                {
                    new Tuple<int, string>(1, "LED Brakelights"),
                    new Tuple<int, string>(2, "Lip Spoiler"),
                    new Tuple<int, string>(3, "Rain Detecting Variable Intermittent Wipers"),
                    new Tuple<int, string>(4, "Steel Spare Wheel"),
                    new Tuple<int, string>(5, "215/60R16 AS"),
                    new Tuple<int, string>(6, "16x6 Aluminum Alloy Wheels with Silver Accents"),
                    new Tuple<int, string>(7, "Clearcoat Paint"),
                    new Tuple<int, string>(8, "Fixed Rear Window with Fixed Interval Wiper and Defroster"),
                    new Tuple<int, string>(9, "1 12V DC Outlet"),
                    new Tuple<int, string>(10, "2 Steaback Storage Pocket"),
                    new Tuple<int, string>(11, "4-Way Passenger Seat Manual Recline"),
                    new Tuple<int, string>(12, "Air Filtration"),
                    new Tuple<int, string>(13, "Automatic Air Conditioning"),
                    new Tuple<int, string>(14, "Cargo Space Lights"),
                    new Tuple<int, string>(15, "Day/Night Rearview Mirror"),
                    new Tuple<int, string>(16, "Heads Up Display"),
                    new Tuple<int, string>(17, "Remote Keyless Entry"),
                    new Tuple<int, string>(18, "Keyless Ignition"),
                    new Tuple<int, string>(19, "SMS Text Message Audio Delivery and Reply"),
                    new Tuple<int, string>(20, "Two LCD Monitors in the Front"),
                    new Tuple<int, string>(21, "AM/FM Radio"),
                    new Tuple<int, string>(22, "Infotainment System Voice Command"),
                    new Tuple<int, string>(23, "Pandora Internet Radio"),
                    new Tuple<int, string>(24, "Bluetooth 5.1"),
                    new Tuple<int, string>(25, "100 Amp Alternator"),
                    new Tuple<int, string>(26, "12.7 Gallon Fuel Tank"),
                    new Tuple<int, string>(27, "4.325 Axle Ratio"),
                    new Tuple<int, string>(28, "Front Wheel Drive"),
                    new Tuple<int, string>(29, "ABS and Driveline Traction Control"),
                    new Tuple<int, string>(30, "Airbag Occupancy Sensor"),
                    new Tuple<int, string>(31, "Back-up Camera"),
                    new Tuple<int, string>(32, "Blind Spot Monitoring"),
                    new Tuple<int, string>(33, "Rear Child Safety Locks"),
                    new Tuple<int, string>(34, "17 Silver-Painted Alloy"),
                    new Tuple<int, string>(35, "225/50R17 AS"),
                    new Tuple<int, string>(36, "14.8 Gallon Fuel Tank"),
                    new Tuple<int, string>(37, "3.24 Axle Ratio"),
                    new Tuple<int, string>(38, "Lane Departure Warning"),
                    new Tuple<int, string>(39, "Side Impact Beams"),
                    new Tuple<int, string>(40, "2 USB Ports"),
                    new Tuple<int, string>(41, "15x5.5 Steel Wheels"),
                    new Tuple<int, string>(42, "185/60R15 84T Tires"),
                    new Tuple<int, string>(43, "10.6 Gallon Fuel Tank")
                }
            };

            actual.Should().NotBeNull()
                  .And.BeEquivalentTo(expected);
        }
    }
}
