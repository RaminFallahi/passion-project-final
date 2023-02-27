namespace passionProject1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class placebestSeasonToGo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.places", "BestSeasonToGoID", c => c.Int(nullable: false));
            CreateIndex("dbo.places", "BestSeasonToGoID");
            AddForeignKey("dbo.places", "BestSeasonToGoID", "dbo.BestSeasonToGoes", "BestSeasonToGoID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.places", "BestSeasonToGoID", "dbo.BestSeasonToGoes");
            DropIndex("dbo.places", new[] { "BestSeasonToGoID" });
            DropColumn("dbo.places", "BestSeasonToGoID");
        }
    }
}
