using System.Data.Entity.Migrations;

namespace apartmenthostService.Migrations
{
    public partial class FeedbackTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("apartmenthost.Feedbacks", "AbuserId", c => c.String(maxLength: 128));
            AddColumn("apartmenthost.Feedbacks", "Type", c => c.String());
            CreateIndex("apartmenthost.Feedbacks", "AbuserId");
            AddForeignKey("apartmenthost.Feedbacks", "AbuserId", "apartmenthost.Users", "Id");
        }

        public override void Down()
        {
            DropForeignKey("apartmenthost.Feedbacks", "AbuserId", "apartmenthost.Users");
            DropIndex("apartmenthost.Feedbacks", new[] {"AbuserId"});
            DropColumn("apartmenthost.Feedbacks", "Type");
            DropColumn("apartmenthost.Feedbacks", "AbuserId");
        }
    }
}