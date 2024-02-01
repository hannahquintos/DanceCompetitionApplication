namespace DanceCompetitionApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dancers1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DancerPerformances",
                c => new
                    {
                        DancerPerformanceId = c.Int(nullable: false, identity: true),
                        DancerId = c.Int(nullable: false),
                        PerformanceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DancerPerformanceId)
                .ForeignKey("dbo.Dancers", t => t.DancerId, cascadeDelete: true)
                .ForeignKey("dbo.Performances", t => t.PerformanceId, cascadeDelete: true)
                .Index(t => t.DancerId)
                .Index(t => t.PerformanceId);
            
            CreateTable(
                "dbo.Dancers",
                c => new
                    {
                        DancerId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DancerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DancerPerformances", "PerformanceId", "dbo.Performances");
            DropForeignKey("dbo.DancerPerformances", "DancerId", "dbo.Dancers");
            DropIndex("dbo.DancerPerformances", new[] { "PerformanceId" });
            DropIndex("dbo.DancerPerformances", new[] { "DancerId" });
            DropTable("dbo.Dancers");
            DropTable("dbo.DancerPerformances");
        }
    }
}
