namespace ApplicationDevelopment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredIdCategory : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Courses", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "Description", c => c.String(nullable: false));
        }
    }
}
