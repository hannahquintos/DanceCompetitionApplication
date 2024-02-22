namespace DanceCompetitionApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDancerPerformance : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DancerPerformances", "PerformanceId", "dbo.Performances");
            DropForeignKey("dbo.DancerPerformances", "DancerId", "dbo.Dancers");
            DropIndex("dbo.DancerPerformances", new[] { "DancerId" });
            DropIndex("dbo.DancerPerformances", new[] { "PerformanceId" });
            AlterColumn("dbo.DancerPerformances", "DancerId", c => c.Int());
            AlterColumn("dbo.DancerPerformances", "PerformanceId", c => c.Int());
            CreateIndex("dbo.DancerPerformances", "DancerId");
            CreateIndex("dbo.DancerPerformances", "PerformanceId");
            AddForeignKey("dbo.DancerPerformances", "PerformanceId", "dbo.Performances", "PerformanceId");
            AddForeignKey("dbo.DancerPerformances", "DancerId", "dbo.Dancers", "DancerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DancerPerformances", "DancerId", "dbo.Dancers");
            DropForeignKey("dbo.DancerPerformances", "PerformanceId", "dbo.Performances");
            DropIndex("dbo.DancerPerformances", new[] { "PerformanceId" });
            DropIndex("dbo.DancerPerformances", new[] { "DancerId" });
            AlterColumn("dbo.DancerPerformances", "PerformanceId", c => c.Int(nullable: false));
            AlterColumn("dbo.DancerPerformances", "DancerId", c => c.Int(nullable: false));
            CreateIndex("dbo.DancerPerformances", "PerformanceId");
            CreateIndex("dbo.DancerPerformances", "DancerId");
            AddForeignKey("dbo.DancerPerformances", "DancerId", "dbo.Dancers", "DancerId", cascadeDelete: true);
            AddForeignKey("dbo.DancerPerformances", "PerformanceId", "dbo.Performances", "PerformanceId", cascadeDelete: true);
        }
    }
}
