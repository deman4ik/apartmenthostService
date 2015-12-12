using System.Data.Entity.Migrations;

namespace apartmenthostService.Migrations
{
    public partial class PhoneConfirmation : DbMigration
    {
        public override void Up()
        {
            AddColumn("apartmenthost.Users", "SaltedAndHashedSmsCode", c => c.Binary());
            AddColumn("apartmenthost.Users", "PhoneStatus", c => c.String());
            AddColumn("apartmenthost.Users", "PhoneCodeRequestedAt", c => c.DateTime());
        }

        public override void Down()
        {
            DropColumn("apartmenthost.Users", "PhoneCodeRequestedAt");
            DropColumn("apartmenthost.Users", "PhoneStatus");
            DropColumn("apartmenthost.Users", "SaltedAndHashedSmsCode");
        }
    }
}