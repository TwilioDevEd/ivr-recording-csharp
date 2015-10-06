namespace IVRRecording.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExplicitForeignKeyOnRecording : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Recordings", "Agent_Id", "dbo.Agents");
            DropIndex("dbo.Recordings", new[] { "Agent_Id" });
            RenameColumn(table: "dbo.Recordings", name: "Agent_Id", newName: "AgentId");
            AlterColumn("dbo.Recordings", "AgentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Recordings", "AgentId");
            AddForeignKey("dbo.Recordings", "AgentId", "dbo.Agents", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recordings", "AgentId", "dbo.Agents");
            DropIndex("dbo.Recordings", new[] { "AgentId" });
            AlterColumn("dbo.Recordings", "AgentId", c => c.Int());
            RenameColumn(table: "dbo.Recordings", name: "AgentId", newName: "Agent_Id");
            CreateIndex("dbo.Recordings", "Agent_Id");
            AddForeignKey("dbo.Recordings", "Agent_Id", "dbo.Agents", "Id");
        }
    }
}
