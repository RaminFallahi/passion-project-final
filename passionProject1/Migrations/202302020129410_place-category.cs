namespace passionProject1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class placecategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.categories",
                c => new
                    {
                        categoryID = c.Int(nullable: false, identity: true),
                        categoryName = c.String(),
                    })
                .PrimaryKey(t => t.categoryID);
            
            AddColumn("dbo.places", "categoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.places", "categoryID");
            AddForeignKey("dbo.places", "categoryID", "dbo.categories", "categoryID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.places", "categoryID", "dbo.categories");
            DropIndex("dbo.places", new[] { "categoryID" });
            DropColumn("dbo.places", "categoryID");
            DropTable("dbo.categories");
        }
    }
}
