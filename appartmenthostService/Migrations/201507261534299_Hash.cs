namespace apartmenthostService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Hash : DbMigration
    {
        public override void Up()
        {
            AddColumn("apartmenthost.Users", "SaltedAndHashedEmail", c => c.Binary());
            AddColumn("apartmenthost.Users", "SaltedAndHashedCode", c => c.Binary());
            AddColumn("apartmenthost.Notifications", "Emailed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("apartmenthost.Notifications", "Emailed");
            DropColumn("apartmenthost.Users", "SaltedAndHashedCode");
            DropColumn("apartmenthost.Users", "SaltedAndHashedEmail");
        }
    }
}
