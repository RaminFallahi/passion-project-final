namespace passionProject1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class place : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.places",
                c => new
                    {
                        PlaceID = c.Int(nullable: false, identity: true),
                        PlaceName = c.String(),
                    })
                .PrimaryKey(t => t.PlaceID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.places");
        }
    }
}
