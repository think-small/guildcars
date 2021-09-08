namespace GuildCars.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedVehicleTypePropFromStringToBool : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "IsNew", c => c.Boolean(nullable: false));
            DropColumn("dbo.Vehicles", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "Type", c => c.String(maxLength: 20));
            DropColumn("dbo.Vehicles", "IsNew");
        }
    }
}
