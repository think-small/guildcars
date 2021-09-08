namespace GuildCars.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnforceFKOnSaleRecordVehicleId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SaleRecords", "VehicleId", "dbo.Vehicles");
            AddForeignKey("dbo.SaleRecords", "VehicleId", "dbo.Vehicles", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleRecords", "VehicleId", "dbo.Vehicles");
        }
    }
}
