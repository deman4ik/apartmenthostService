namespace apartmenthostService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CardPrice : DbMigration
    {
        public override void Up()
        {
            DropColumn("apartmenthost.Cards", "PriceDay");
        }
        
        public override void Down()
        {
            AddColumn("apartmenthost.Cards", "PriceDay", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
