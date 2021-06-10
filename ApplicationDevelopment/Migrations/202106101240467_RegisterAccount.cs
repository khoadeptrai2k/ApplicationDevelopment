namespace ApplicationDevelopment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegisterAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "WorkingPlace", c => c.String());
            AddColumn("dbo.AspNetUsers", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Phone", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Phone");
            DropColumn("dbo.AspNetUsers", "Age");
            DropColumn("dbo.AspNetUsers", "WorkingPlace");
            DropColumn("dbo.AspNetUsers", "Name");
        }
    }
}
