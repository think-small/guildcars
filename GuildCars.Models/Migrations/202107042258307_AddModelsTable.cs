namespace GuildCars.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModelsTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vehicles", "MakeId", "dbo.Makes");
            DropIndex("dbo.Vehicles", new[] { "MakeId" });
            CreateTable(
                "dbo.Models",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MakeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Makes", t => t.MakeId)
                .Index(t => t.MakeId);
            
            AddColumn("dbo.Vehicles", "ModelId", c => c.Int());
            CreateIndex("dbo.Vehicles", "ModelId");
            AddForeignKey("dbo.Vehicles", "ModelId", "dbo.Models");
            DropColumn("dbo.Vehicles", "MakeId");
            DropColumn("dbo.Vehicles", "Model");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "Model", c => c.String(maxLength: 20));
            AddColumn("dbo.Vehicles", "MakeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Vehicles", "ModelId", "dbo.Models");
            DropForeignKey("dbo.Models", "MakeId", "dbo.Makes");
            DropIndex("dbo.Models", new[] { "MakeId" });
            DropIndex("dbo.Vehicles", new[] { "ModelId" });
            DropColumn("dbo.Vehicles", "ModelId");
            DropTable("dbo.Models");
            CreateIndex("dbo.Vehicles", "MakeId");
            AddForeignKey("dbo.Vehicles", "MakeId", "dbo.Makes", "Id");
        }
    }
}
