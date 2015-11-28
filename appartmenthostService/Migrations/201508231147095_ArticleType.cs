using System.Data.Entity.Migrations;

namespace apartmenthostService.Migrations
{
    public partial class ArticleType : DbMigration
    {
        public override void Up()
        {
            AddColumn("apartmenthost.Articles", "Type", c => c.String());
            DropColumn("apartmenthost.Articles", "Tag");
        }

        public override void Down()
        {
            AddColumn("apartmenthost.Articles", "Tag", c => c.String());
            DropColumn("apartmenthost.Articles", "Type");
        }
    }
}