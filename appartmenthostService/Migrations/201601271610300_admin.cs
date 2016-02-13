using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations;

namespace apartmenthostService.Migrations
{
    public partial class admin : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "apartmenthost.Admins",
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
                    Email = c.String(),
                    Salt = c.Binary(),
                    SaltedAndHashedPassword = c.Binary(),
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
                .Index(t => t.CreatedAt, clustered: true);

            DropColumn("apartmenthost.Profiles", "ContactEmail");
            DropColumn("apartmenthost.Profiles", "ContactKind");
        }

        public override void Down()
        {
            AddColumn("apartmenthost.Profiles", "ContactKind", c => c.String());
            AddColumn("apartmenthost.Profiles", "ContactEmail", c => c.String());
            DropIndex("apartmenthost.Admins", new[] {"CreatedAt"});
            DropTable("apartmenthost.Admins", new Dictionary<string, IDictionary<string, object>>
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