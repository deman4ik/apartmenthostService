using System.Data.Entity.Migrations;

namespace apartmenthostService.Migrations
{
    public partial class CardPrice : DbMigration
    {
        public override void Up()
        {
            DropColumn("apartmenthost.Cards", "PriceDay");
        }

        public override void Down()
        {
            AddColumn("apartmenthost.Cards", "PriceDay", c => c.Decimal(false, 18, 2));
        }
    }
}