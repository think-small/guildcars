namespace GuildCars.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNameAndAddressFieldsToAppUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(maxLength: 100));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(maxLength: 100));
            AddColumn("dbo.AspNetUsers", "Address1", c => c.String(maxLength: 255));
            AddColumn("dbo.AspNetUsers", "Address2", c => c.String(maxLength: 255));
            AddColumn("dbo.AspNetUsers", "City", c => c.String(maxLength: 50));
            AddColumn("dbo.AspNetUsers", "State", c => c.String(maxLength: 50));
            AddColumn("dbo.AspNetUsers", "ZipCode", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ZipCode");
            DropColumn("dbo.AspNetUsers", "State");
            DropColumn("dbo.AspNetUsers", "City");
            DropColumn("dbo.AspNetUsers", "Address2");
            DropColumn("dbo.AspNetUsers", "Address1");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
        }
    }
}
