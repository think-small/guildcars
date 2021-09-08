namespace GuildCars.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFKFromVehicleToMakes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "MakeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Vehicles", "MakeId");
            AddForeignKey("dbo.Vehicles", "MakeId", "dbo.Makes", "Id");
            DropColumn("dbo.Vehicles", "Make");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "Make", c => c.String(maxLength: 20));
            DropForeignKey("dbo.Vehicles", "MakeId", "dbo.Makes");
            DropIndex("dbo.Vehicles", new[] { "MakeId" });
            DropColumn("dbo.Vehicles", "MakeId");
        }
    }
}
