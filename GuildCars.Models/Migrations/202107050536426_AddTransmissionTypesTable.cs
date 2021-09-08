namespace GuildCars.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransmissionTypesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransmissionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Vehicles", "TransmissionTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Vehicles", "TransmissionTypeId");
            AddForeignKey("dbo.Vehicles", "TransmissionTypeId", "dbo.TransmissionTypes", "Id");
            DropColumn("dbo.Vehicles", "Transmission");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "Transmission", c => c.String(maxLength: 20));
            DropForeignKey("dbo.Vehicles", "TransmissionTypeId", "dbo.TransmissionTypes");
            DropIndex("dbo.Vehicles", new[] { "TransmissionTypeId" });
            DropColumn("dbo.Vehicles", "TransmissionTypeId");
            DropTable("dbo.TransmissionTypes");
        }
    }
}
