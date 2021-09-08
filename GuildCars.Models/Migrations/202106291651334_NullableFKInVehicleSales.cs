namespace GuildCars.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableFKInVehicleSales : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LoanPaymentDetails", "AmortizedLoanScheduleId", "dbo.AmortizedLoanSchedules");
            DropIndex("dbo.LoanPaymentDetails", new[] { "AmortizedLoanScheduleId" });
            RenameColumn(table: "dbo.VehicleSales", name: "TradeIn_Id", newName: "TradeInId");
            RenameIndex(table: "dbo.VehicleSales", name: "IX_TradeIn_Id", newName: "IX_TradeInId");
            AddColumn("dbo.Vehicles", "UserId", c => c.String());
            AddColumn("dbo.VehicleSales", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.VehicleSales", "DownPayment");
            DropColumn("dbo.VehicleSales", "LoanLength");
            DropColumn("dbo.VehicleSales", "InterestRate");
            DropTable("dbo.AmortizedLoanSchedules");
            DropTable("dbo.LoanPaymentDetails");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
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
            
            AddColumn("dbo.VehicleSales", "InterestRate", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.VehicleSales", "LoanLength", c => c.Int());
            AddColumn("dbo.VehicleSales", "DownPayment", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.VehicleSales", "Date");
            DropColumn("dbo.Vehicles", "UserId");
            RenameIndex(table: "dbo.VehicleSales", name: "IX_TradeInId", newName: "IX_TradeIn_Id");
            RenameColumn(table: "dbo.VehicleSales", name: "TradeInId", newName: "TradeIn_Id");
            CreateIndex("dbo.LoanPaymentDetails", "AmortizedLoanScheduleId");
            AddForeignKey("dbo.LoanPaymentDetails", "AmortizedLoanScheduleId", "dbo.AmortizedLoanSchedules", "Id", cascadeDelete: true);
        }
    }
}
