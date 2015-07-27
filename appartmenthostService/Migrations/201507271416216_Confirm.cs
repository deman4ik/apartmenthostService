namespace apartmenthostService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Confirm : DbMigration
    {
        public override void Up()
        {
            AddColumn("apartmenthost.Users", "SaltedAndHashedEmail", c => c.Binary());
            AddColumn("apartmenthost.Users", "SaltedAndHashedCode", c => c.Binary());
            AddColumn("apartmenthost.Users", "EmailConfirmed", c => c.Boolean(nullable: false));
            AddColumn("apartmenthost.Users", "ResetRequested", c => c.Boolean(nullable: false));
            AddColumn("apartmenthost.Notifications", "Emailed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("apartmenthost.Notifications", "Emailed");
            DropColumn("apartmenthost.Users", "ResetRequested");
            DropColumn("apartmenthost.Users", "EmailConfirmed");
            DropColumn("apartmenthost.Users", "SaltedAndHashedCode");
            DropColumn("apartmenthost.Users", "SaltedAndHashedEmail");
        }
    }
}
