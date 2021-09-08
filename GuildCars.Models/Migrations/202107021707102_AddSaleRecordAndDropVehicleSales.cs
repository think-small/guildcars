namespace GuildCars.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSaleRecordAndDropVehicleSales : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VehicleSales", "TradeInId", "dbo.Vehicles");
            DropForeignKey("dbo.VehicleSales", "VehicleId", "dbo.Vehicles");
            DropIndex("dbo.VehicleSales", new[] { "VehicleId" });
            DropIndex("dbo.VehicleSales", new[] { "TradeInId" });
            AddColumn("dbo.SaleRecords", "TradeInId", c => c.Int());
            AddColumn("dbo.SaleRecords", "PurchaseType", c => c.String(maxLength: 20));
            AlterColumn("dbo.SaleRecords", "CustomerId", c => c.String(maxLength: 50));
            AlterColumn("dbo.SaleRecords", "EmployeeId", c => c.String(maxLength: 50));
            CreateIndex("dbo.SaleRecords", "TradeInId");
            AddForeignKey("dbo.SaleRecords", "TradeInId", "dbo.Vehicles", "Id");
            DropTable("dbo.VehicleSales");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.VehicleSales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VehicleId = c.Int(nullable: false),
                        PurchasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PurchaseType = c.String(),
                        TradeInId = c.Int(),
                        CustomerId = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.SaleRecords", "TradeInId", "dbo.Vehicles");
            DropIndex("dbo.SaleRecords", new[] { "TradeInId" });
            AlterColumn("dbo.SaleRecords", "EmployeeId", c => c.String());
            AlterColumn("dbo.SaleRecords", "CustomerId", c => c.String());
            DropColumn("dbo.SaleRecords", "PurchaseType");
            DropColumn("dbo.SaleRecords", "TradeInId");
            CreateIndex("dbo.VehicleSales", "TradeInId");
            CreateIndex("dbo.VehicleSales", "VehicleId");
            AddForeignKey("dbo.VehicleSales", "VehicleId", "dbo.Vehicles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.VehicleSales", "TradeInId", "dbo.Vehicles", "Id");
        }
    }
}
