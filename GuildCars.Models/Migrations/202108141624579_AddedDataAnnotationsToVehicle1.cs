namespace GuildCars.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDataAnnotationsToVehicle1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vehicles", "Color", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Vehicles", "Interior", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Vehicles", "VIN", c => c.String(nullable: false, maxLength: 17));
            AlterColumn("dbo.Vehicles", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Vehicles", "Engine", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vehicles", "Engine", c => c.String(maxLength: 20));
            AlterColumn("dbo.Vehicles", "Description", c => c.String());
            AlterColumn("dbo.Vehicles", "VIN", c => c.String(maxLength: 17));
            AlterColumn("dbo.Vehicles", "Interior", c => c.String(maxLength: 20));
            AlterColumn("dbo.Vehicles", "Color", c => c.String(maxLength: 20));
        }
    }
}
