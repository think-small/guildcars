namespace GuildCars.Models.Migrations
{
    using GuildCars.UI.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GuildCars.Models.Contexts.GCContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GuildCars.Models.Contexts.GCContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.           
            
            //  Uncomment the following code block to debug Seed method
            //if (!System.Diagnostics.Debugger.IsAttached)
            //    System.Diagnostics.Debugger.Launch();

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            if (!context.Roles.Any(r => r.Name == "Customer"))
            {
                var role = new IdentityRole { Name = "Customer" };
                roleManager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "Sales"))
            {
                var role = new IdentityRole { Name = "Sales" };
                roleManager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var role = new IdentityRole { Name = "Admin" };
                roleManager.Create(role);
            }

            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);

            if (!context.Users.Any(u => u.UserName == "TestCustomer2"))
            {
                var user = new ApplicationUser
                {
                    Email = "test.customer2@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "555-123-4567",
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    UserName = "TestCustomer2",
                    FirstName = "Test",
                    LastName = "Customer2",
                    Address1 = "123 TestCustomer Dr",
                    City = "St Louis Park",
                    State = "Minnesota",
                    ZipCode = "55426"
                };

                manager.Create(user, "password");
                manager.AddToRole(user.Id, "Customer");
            }
            if (!context.Users.Any(u => u.UserName == "TestSales1"))
            {
                var user = new ApplicationUser
                {
                    Email = "test.sales1@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "555-123-4567",
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    UserName = "TestSales1",
                    FirstName = "Test",
                    LastName = "Sales1",
                    Address1 = "123 TestSales St",
                    City = "St Paul",
                    State = "Minnesota",
                    ZipCode = "55444"
                };

                manager.Create(user, "password");
                manager.AddToRole(user.Id, "Sales");
            }

            if (!context.Users.Any(u => u.UserName == "TestAdmin1"))
            {
                var user = new ApplicationUser
                {
                    Email = "test.admin1@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "555-987-6543",
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    UserName = "TestAdmin1",
                    FirstName = "Test",
                    LastName = "Admin1",
                    Address1 = "987 Admin Dr",
                    City = "Roseville",
                    State = "Minnesota",
                    ZipCode = "55382"
                };

                manager.Create(user, "password");
                manager.AddToRole(user.Id, "Admin");
            }

            //context.PurchaseTypes.AddOrUpdate(new PurchaseType { Name = "Cash" });
            //context.PurchaseTypes.AddOrUpdate(new PurchaseType { Name = "Bank Finance" });
            //context.PurchaseTypes.AddOrUpdate(new PurchaseType { Name = "Dealer Finance" });

            //context.Makes.AddOrUpdate(new Make { Name = "Mazda" });
            //context.Makes.AddOrUpdate(new Make { Name = "Honda" });
            //context.Makes.AddOrUpdate(new Make { Name = "Ford" });

            //context.Models.AddOrUpdate(new Model { Name = "CX-3", MakeId = 1 });
            //context.Models.AddOrUpdate(new Model { Name = "CX-5", MakeId = 1 });
            //context.Models.AddOrUpdate(new Model { Name = "Accord", MakeId = 2 });
            //context.Models.AddOrUpdate(new Model { Name = "Civic", MakeId = 2 });
            //context.Models.AddOrUpdate(new Model { Name = "Fit", MakeId = 2 });

            //context.BodyStyles.AddOrUpdate(new BodyStyle { Name = "Sedan" });
            //context.BodyStyles.AddOrUpdate(new BodyStyle { Name = "Coupe" });
            //context.BodyStyles.AddOrUpdate(new BodyStyle { Name = "Sports Car" });
            //context.BodyStyles.AddOrUpdate(new BodyStyle { Name = "Station Wagon" });
            //context.BodyStyles.AddOrUpdate(new BodyStyle { Name = "Hatchback" });
            //context.BodyStyles.AddOrUpdate(new BodyStyle { Name = "Convertible" });
            //context.BodyStyles.AddOrUpdate(new BodyStyle { Name = "SUV" });
            //context.BodyStyles.AddOrUpdate(new BodyStyle { Name = "Subcompact" });
            //context.BodyStyles.AddOrUpdate(new BodyStyle { Name = "Minivan" });
            //context.BodyStyles.AddOrUpdate(new BodyStyle { Name = "Van" });
            //context.BodyStyles.AddOrUpdate(new BodyStyle { Name = "Pickup" });

            //context.TransmissionTypes.AddOrUpdate(new TransmissionType { Name = "Automatic" });
            //context.TransmissionTypes.AddOrUpdate(new TransmissionType { Name = "Manual" });
            //context.TransmissionTypes.AddOrUpdate(new TransmissionType { Name = "Automated Manual" });
            //context.TransmissionTypes.AddOrUpdate(new TransmissionType { Name = "Continuously Variable" });


            //var cars = context.Vehicles.ToList();
            //context.Vehicles.RemoveRange(cars);
            //context.SaveChanges();

            //context.Vehicles.AddOrUpdate(new Vehicle
            //{
            //    ModelId = 1,
            //    Type = "New",
            //    BodyStyleId = 8,
            //    Year = 2020,
            //    TransmissionTypeId = 1,
            //    Color = "Blue",
            //    Interior = "Gray",
            //    Mileage = 75.1M,
            //    VIN = "WDCGG5HB4FG358810",
            //    MSRP = 35399.99M,
            //    SalePrice = 35399.99M,
            //    Description = "New Carbon Edition Mazda CX-3 is a powerful four-cylinder, front-wheel drive, six-speed automatic daily driver. This vehicle has a luxury aura without the luxury price.",
            //    ImagePaths = new List<ImagePath>(),
            //});
            //context.Vehicles.AddOrUpdate(new Vehicle
            //{
            //    ModelId = 2,
            //    Type = "New",
            //    BodyStyleId = 8,
            //    Year = 2021,
            //    TransmissionTypeId = 1,
            //    Color = "Green",
            //    Interior = "Gray",
            //    Mileage = 75.1M,
            //    VIN = "WDCGG5HB4FG358811",
            //    MSRP = 45399.99M,
            //    SalePrice = 45399.99M,
            //    Description = "New Carbon Edition Mazda CX-5 is a powerful four-cylinder, front-wheel drive, six-speed automatic daily driver. This vehicle has a luxury aura without the luxury price.",
            //    ImagePaths = new List<ImagePath>(),
            //});
            //context.Vehicles.AddOrUpdate(new Vehicle
            //{
            //    ModelId = 3,
            //    Type = "Used",
            //    BodyStyleId = 1,
            //    Year = 2019,
            //    TransmissionTypeId = 1,
            //    Color = "Titanium",
            //    Interior = "Gray",
            //    Mileage = 2150.2M,
            //    VIN = "WDCGG5HB4FG358812",
            //    MSRP = 27999M,
            //    SalePrice = 27999M,
            //    Description = "It still works, what more do you want?",
            //    ImagePaths = new List<ImagePath>(),
            //});
            //context.Vehicles.AddOrUpdate(new Vehicle
            //{
            //    ModelId = 4,
            //    Type = "New",
            //    BodyStyleId = 1,
            //    Year = 2021,
            //    TransmissionTypeId = 1,
            //    Color = "Red",
            //    Interior = "White",
            //    Mileage = 1121.7M,
            //    VIN = "WDCGG5HB4FG358814",
            //    MSRP = 29899M,
            //    SalePrice = 29899M,
            //    Description = "It's popular, you should probably own one too.",
            //    ImagePaths = new List<ImagePath>()
            //});
            //context.Vehicles.AddOrUpdate(new Vehicle
            //{
            //    ModelId = 5,
            //    Type = "New",
            //    BodyStyleId = 8,
            //    Year = 2021,
            //    TransmissionTypeId = 1,
            //    Color = "Orange",
            //    Interior = "Black",
            //    Mileage = 55.9M,
            //    VIN = "WDCGG5HB4FG358815",
            //    MSRP = 18999M,
            //    SalePrice = 16999M,
            //    Description = "Please buy this...",
            //    ImagePaths = new List<ImagePath>()
            //});

            context.SaveChanges();
        }
    }
}
