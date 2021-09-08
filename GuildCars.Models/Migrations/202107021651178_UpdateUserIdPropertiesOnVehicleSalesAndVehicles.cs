namespace GuildCars.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserIdPropertiesOnVehicleSalesAndVehicles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "OwnerId", c => c.String());
            AddColumn("dbo.VehicleSales", "CustomerId", c => c.String());
            DropColumn("dbo.Vehicles", "UserId");
            DropColumn("dbo.VehicleSales", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VehicleSales", "UserId", c => c.String());
            AddColumn("dbo.Vehicles", "UserId", c => c.String());
            DropColumn("dbo.VehicleSales", "CustomerId");
            DropColumn("dbo.Vehicles", "OwnerId");
        }
    }
}
