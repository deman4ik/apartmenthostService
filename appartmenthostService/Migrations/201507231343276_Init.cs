namespace apartmenthostService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            AddColumn("apartmenthost.Notifications", "Emailed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("apartmenthost.Notifications", "Emailed");
        }
    }
}
