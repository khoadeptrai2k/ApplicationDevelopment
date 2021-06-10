namespace ApplicationDevelopment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryCRUD : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "Name_Index");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Categories", "Name_Index");
            DropTable("dbo.Categories");
        }
    }
}
