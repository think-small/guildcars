namespace GuildCars.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMileageDetailsEngineToVehicle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Details",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        IsKeyFeature = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DetailVehicles",
                c => new
                    {
                        Detail_Id = c.Int(nullable: false),
                        Vehicle_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Detail_Id, t.Vehicle_Id })
                .ForeignKey("dbo.Details", t => t.Detail_Id)
                .ForeignKey("dbo.Vehicles", t => t.Vehicle_Id)
                .Index(t => t.Detail_Id)
                .Index(t => t.Vehicle_Id);
            
            AddColumn("dbo.Vehicles", "HighwayMpg", c => c.Short(nullable: false));
            AddColumn("dbo.Vehicles", "CityMpg", c => c.Short(nullable: false));
            AddColumn("dbo.Vehicles", "Engine", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DetailVehicles", "Vehicle_Id", "dbo.Vehicles");
            DropForeignKey("dbo.DetailVehicles", "Detail_Id", "dbo.Details");
            DropIndex("dbo.DetailVehicles", new[] { "Vehicle_Id" });
            DropIndex("dbo.DetailVehicles", new[] { "Detail_Id" });
            DropColumn("dbo.Vehicles", "Engine");
            DropColumn("dbo.Vehicles", "CityMpg");
            DropColumn("dbo.Vehicles", "HighwayMpg");
            DropTable("dbo.DetailVehicles");
            DropTable("dbo.Details");
        }
    }
}
