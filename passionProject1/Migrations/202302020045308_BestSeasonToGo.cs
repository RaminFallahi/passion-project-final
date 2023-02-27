namespace passionProject1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BestSeasonToGo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BestSeasonToGoes",
                c => new
                    {
                        BestSeasonToGoID = c.Int(nullable: false, identity: true),
                        Season = c.String(),
                    })
                .PrimaryKey(t => t.BestSeasonToGoID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BestSeasonToGoes");
        }
    }
}
