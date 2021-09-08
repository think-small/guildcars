namespace GuildCars.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedClassNameDetailsToDetail : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DetailVehicles", newName: "VehicleDetails");
            DropPrimaryKey("dbo.VehicleDetails");
            AddPrimaryKey("dbo.VehicleDetails", new[] { "Vehicle_Id", "Detail_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.VehicleDetails");
            AddPrimaryKey("dbo.VehicleDetails", new[] { "Detail_Id", "Vehicle_Id" });
            RenameTable(name: "dbo.VehicleDetails", newName: "DetailVehicles");
        }
    }
}
