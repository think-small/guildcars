namespace GuildCars.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AmortizedLoanSchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OriginationDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        InterestRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LoanPaymentDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DueDate = c.DateTime(nullable: false),
                        PaymentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Principal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Interest = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CumulativeInterest = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmortizedLoanScheduleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AmortizedLoanSchedules", t => t.AmortizedLoanScheduleId, cascadeDelete: true)
                .Index(t => t.AmortizedLoanScheduleId);
            
            CreateTable(
                "dbo.ImagePaths",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        VehicleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.VehicleId);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Make = c.String(),
                        Model = c.String(),
                        Type = c.String(),
                        BodyStyle = c.String(),
                        Year = c.Int(nullable: false),
                        Transmission = c.String(),
                        Color = c.String(),
                        Interior = c.String(),
                        Mileage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VIN = c.String(),
                        MSRP = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VehicleSales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VehicleId = c.Int(nullable: false),
                        PurchasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PurchaseType = c.String(),
                        DownPayment = c.Decimal(precision: 18, scale: 2),
                        LoanLength = c.Int(),
                        InterestRate = c.Decimal(precision: 18, scale: 2),
                        UserId = c.String(),
                        TradeIn_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.TradeIn_Id)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.VehicleId)
                .Index(t => t.TradeIn_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VehicleSales", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.VehicleSales", "TradeIn_Id", "dbo.Vehicles");
            DropForeignKey("dbo.ImagePaths", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.LoanPaymentDetails", "AmortizedLoanScheduleId", "dbo.AmortizedLoanSchedules");
            DropIndex("dbo.VehicleSales", new[] { "TradeIn_Id" });
            DropIndex("dbo.VehicleSales", new[] { "VehicleId" });
            DropIndex("dbo.ImagePaths", new[] { "VehicleId" });
            DropIndex("dbo.LoanPaymentDetails", new[] { "AmortizedLoanScheduleId" });
            DropTable("dbo.VehicleSales");
            DropTable("dbo.Vehicles");
            DropTable("dbo.ImagePaths");
            DropTable("dbo.LoanPaymentDetails");
            DropTable("dbo.AmortizedLoanSchedules");
        }
    }
}
