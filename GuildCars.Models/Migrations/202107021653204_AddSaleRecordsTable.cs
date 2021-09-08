namespace GuildCars.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSaleRecordsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SaleRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.String(),
                        EmployeeId = c.String(),
                        VehicleId = c.Int(nullable: false),
                        PurchasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpectedSalePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.VehicleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleRecords", "VehicleId", "dbo.Vehicles");
            DropIndex("dbo.SaleRecords", new[] { "VehicleId" });
            DropTable("dbo.SaleRecords");
        }
    }
}
