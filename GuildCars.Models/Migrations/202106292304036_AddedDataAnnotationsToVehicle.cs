namespace GuildCars.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDataAnnotationsToVehicle : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vehicles", "Make", c => c.String(maxLength: 20));
            AlterColumn("dbo.Vehicles", "Model", c => c.String(maxLength: 20));
            AlterColumn("dbo.Vehicles", "Type", c => c.String(maxLength: 20));
            AlterColumn("dbo.Vehicles", "BodyStyle", c => c.String(maxLength: 20));
            AlterColumn("dbo.Vehicles", "Transmission", c => c.String(maxLength: 20));
            AlterColumn("dbo.Vehicles", "Color", c => c.String(maxLength: 20));
            AlterColumn("dbo.Vehicles", "Interior", c => c.String(maxLength: 20));
            AlterColumn("dbo.Vehicles", "VIN", c => c.String(maxLength: 17));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vehicles", "VIN", c => c.String());
            AlterColumn("dbo.Vehicles", "Interior", c => c.String());
            AlterColumn("dbo.Vehicles", "Color", c => c.String());
            AlterColumn("dbo.Vehicles", "Transmission", c => c.String());
            AlterColumn("dbo.Vehicles", "BodyStyle", c => c.String());
            AlterColumn("dbo.Vehicles", "Type", c => c.String());
            AlterColumn("dbo.Vehicles", "Model", c => c.String());
            AlterColumn("dbo.Vehicles", "Make", c => c.String());
        }
    }
}
