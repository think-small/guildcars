namespace GuildCars.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBodyStylesTable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Vehicles", new[] { "ModelId" });
            CreateTable(
                "dbo.BodyStyles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Vehicles", "BodyStyleId", c => c.Int(nullable: false));
            AlterColumn("dbo.Vehicles", "ModelId", c => c.Int(nullable: false));
            AlterColumn("dbo.Models", "Name", c => c.String(maxLength: 20));
            AlterColumn("dbo.Makes", "Name", c => c.String(maxLength: 20));
            CreateIndex("dbo.Vehicles", "ModelId");
            CreateIndex("dbo.Vehicles", "BodyStyleId");
            AddForeignKey("dbo.Vehicles", "BodyStyleId", "dbo.BodyStyles", "Id");
            DropColumn("dbo.Vehicles", "BodyStyle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "BodyStyle", c => c.String(maxLength: 20));
            DropForeignKey("dbo.Vehicles", "BodyStyleId", "dbo.BodyStyles");
            DropIndex("dbo.Vehicles", new[] { "BodyStyleId" });
            DropIndex("dbo.Vehicles", new[] { "ModelId" });
            AlterColumn("dbo.Makes", "Name", c => c.String());
            AlterColumn("dbo.Models", "Name", c => c.String());
            AlterColumn("dbo.Vehicles", "ModelId", c => c.Int());
            DropColumn("dbo.Vehicles", "BodyStyleId");
            DropTable("dbo.BodyStyles");
            CreateIndex("dbo.Vehicles", "ModelId");
        }
    }
}
