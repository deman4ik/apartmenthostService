using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations;

namespace apartmenthostService.Migrations
{
    public partial class Feedback : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "apartmenthost.Feedbacks",
                c => new
                {
                    Id = c.String(false, 128,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(null, "Id")
                            }
                        }),
                    UserId = c.String(maxLength: 128),
                    UserName = c.String(),
                    Email = c.String(),
                    Text = c.String(),
                    AnswerByEmail = c.Boolean(false),
                    Version = c.Binary(false, fixedLength: true, timestamp: true, storeType: "rowversion",
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(null, "Version")
                            }
                        }),
                    CreatedAt = c.DateTimeOffset(false, 7,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(null, "CreatedAt")
                            }
                        }),
                    UpdatedAt = c.DateTimeOffset(precision: 7,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(null, "UpdatedAt")
                            }
                        }),
                    Deleted = c.Boolean(false,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(null, "Deleted")
                            }
                        })
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("apartmenthost.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CreatedAt, clustered: true);
        }

        public override void Down()
        {
            DropForeignKey("apartmenthost.Feedbacks", "UserId", "apartmenthost.Users");
            DropIndex("apartmenthost.Feedbacks", new[] {"CreatedAt"});
            DropIndex("apartmenthost.Feedbacks", new[] {"UserId"});
            DropTable("apartmenthost.Feedbacks", new Dictionary<string, IDictionary<string, object>>
            {
                {
                    "CreatedAt",
                    new Dictionary<string, object>
                    {
                        {"ServiceTableColumn", "CreatedAt"}
                    }
                },
                {
                    "Deleted",
                    new Dictionary<string, object>
                    {
                        {"ServiceTableColumn", "Deleted"}
                    }
                },
                {
                    "Id",
                    new Dictionary<string, object>
                    {
                        {"ServiceTableColumn", "Id"}
                    }
                },
                {
                    "UpdatedAt",
                    new Dictionary<string, object>
                    {
                        {"ServiceTableColumn", "UpdatedAt"}
                    }
                },
                {
                    "Version",
                    new Dictionary<string, object>
                    {
                        {"ServiceTableColumn", "Version"}
                    }
                }
            });
        }
    }
}