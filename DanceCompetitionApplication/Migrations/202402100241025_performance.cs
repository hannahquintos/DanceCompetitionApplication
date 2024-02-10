namespace DanceCompetitionApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class performance : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Dancers", "Performance_PerformanceId", "dbo.Performances");
            DropIndex("dbo.Dancers", new[] { "Performance_PerformanceId" });
            DropColumn("dbo.Dancers", "Performance_PerformanceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dancers", "Performance_PerformanceId", c => c.Int());
            CreateIndex("dbo.Dancers", "Performance_PerformanceId");
            AddForeignKey("dbo.Dancers", "Performance_PerformanceId", "dbo.Performances", "PerformanceId");
        }
    }
}
