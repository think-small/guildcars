namespace GuildCars.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFKFromSaleRecordToPurchaseType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleRecords", "PurchaseTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.SaleRecords", "PurchaseTypeId");
            AddForeignKey("dbo.SaleRecords", "PurchaseTypeId", "dbo.PurchaseTypes", "Id");
            DropColumn("dbo.SaleRecords", "PurchaseType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SaleRecords", "PurchaseType", c => c.String(maxLength: 20));
            DropForeignKey("dbo.SaleRecords", "PurchaseTypeId", "dbo.PurchaseTypes");
            DropIndex("dbo.SaleRecords", new[] { "PurchaseTypeId" });
            DropColumn("dbo.SaleRecords", "PurchaseTypeId");
        }
    }
}
