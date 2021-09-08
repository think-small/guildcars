namespace GuildCars.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDataTypeOfSaleRecordDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SaleRecords", "Date", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SaleRecords", "Date", c => c.DateTime(nullable: false));
        }
    }
}
