namespace apartmenthostService.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;

    public partial class Refactor : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("apartmenthost.DictionaryItems", "DictionaryId", "apartmenthost.Dictionaries");
            DropForeignKey("apartmenthost.PropVals", "PropId", "apartmenthost.Props");
            DropForeignKey("apartmenthost.TableProp", "TableRefId", "apartmenthost.Tables");
            DropForeignKey("apartmenthost.TableProp", "PropRefId", "apartmenthost.Props");
            DropForeignKey("apartmenthost.Props", "DictionaryId", "apartmenthost.Dictionaries");
            DropForeignKey("apartmenthost.PropVals", "DictionaryItemId", "apartmenthost.DictionaryItems");
            DropForeignKey("apartmenthost.PropVals", "ReservationItemId", "apartmenthost.Reservations");
            DropForeignKey("apartmenthost.Articles", "PictureId", "apartmenthost.Pictures");
            DropForeignKey("apartmenthost.PropVals", "AdvertItemId", "apartmenthost.Cards");
            DropForeignKey("apartmenthost.PropVals", "ApartmentItemId", "apartmenthost.Apartments");
            DropIndex("apartmenthost.PropVals", new[] {"PropId"});
            DropIndex("apartmenthost.PropVals", new[] {"ApartmentItemId"});
            DropIndex("apartmenthost.PropVals", new[] {"AdvertItemId"});
            DropIndex("apartmenthost.PropVals", new[] {"ReservationItemId"});
            DropIndex("apartmenthost.PropVals", new[] {"DictionaryItemId"});
            DropIndex("apartmenthost.PropVals", new[] {"CreatedAt"});
            DropIndex("apartmenthost.DictionaryItems", new[] {"DictionaryId"});
            DropIndex("apartmenthost.DictionaryItems", new[] {"CreatedAt"});
            DropIndex("apartmenthost.Dictionaries", new[] {"CreatedAt"});
            DropIndex("apartmenthost.Props", new[] {"DictionaryId"});
            DropIndex("apartmenthost.Props", new[] {"CreatedAt"});
            DropIndex("apartmenthost.Tables", new[] {"CreatedAt"});
            DropIndex("apartmenthost.Articles", new[] {"PictureId"});
            DropIndex("apartmenthost.TableProp", new[] {"TableRefId"});
            DropIndex("apartmenthost.TableProp", new[] {"PropRefId"});
            CreateTable(
                "apartmenthost.CardGenders",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "Id")
                            },
                        }),
                    Name = c.String(),
                    Price = c.Decimal(precision: 18, scale: 2),
                    CardId = c.String(nullable: false, maxLength: 128),
                    Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "Version")
                            },
                        }),
                    CreatedAt = c.DateTimeOffset(nullable: false, precision: 7,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                            },
                        }),
                    UpdatedAt = c.DateTimeOffset(precision: 7,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                            },
                        }),
                    Deleted = c.Boolean(nullable: false,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "Deleted")
                            },
                        }),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("apartmenthost.Cards", t => t.CardId)
                .Index(t => t.CardId)
                .Index(t => t.CreatedAt, clustered: true);

            AddColumn("apartmenthost.Users", "SaltedAndHashedEmail", c => c.Binary());
            AddColumn("apartmenthost.Users", "SaltedAndHashedCode", c => c.Binary());
            AddColumn("apartmenthost.Users", "EmailConfirmed", c => c.Boolean(nullable: false));
            AddColumn("apartmenthost.Users", "ResetRequested", c => c.Boolean(nullable: false));
            AddColumn("apartmenthost.Users", "Blocked", c => c.Boolean(nullable: false));
            AddColumn("apartmenthost.Notifications", "Emailed", c => c.Boolean(nullable: false));
            DropColumn("apartmenthost.Articles", "PictureId");
            DropTable("apartmenthost.PropVals",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreatedAt",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "CreatedAt"},
                        }
                    },
                    {
                        "Deleted",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "Deleted"},
                        }
                    },
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "Id"},
                        }
                    },
                    {
                        "UpdatedAt",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "UpdatedAt"},
                        }
                    },
                    {
                        "Version",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "Version"},
                        }
                    },
                });
            DropTable("apartmenthost.DictionaryItems",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreatedAt",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "CreatedAt"},
                        }
                    },
                    {
                        "Deleted",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "Deleted"},
                        }
                    },
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "Id"},
                        }
                    },
                    {
                        "UpdatedAt",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "UpdatedAt"},
                        }
                    },
                    {
                        "Version",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "Version"},
                        }
                    },
                });
            DropTable("apartmenthost.Dictionaries",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreatedAt",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "CreatedAt"},
                        }
                    },
                    {
                        "Deleted",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "Deleted"},
                        }
                    },
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "Id"},
                        }
                    },
                    {
                        "UpdatedAt",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "UpdatedAt"},
                        }
                    },
                    {
                        "Version",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "Version"},
                        }
                    },
                });
            DropTable("apartmenthost.Props",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreatedAt",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "CreatedAt"},
                        }
                    },
                    {
                        "Deleted",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "Deleted"},
                        }
                    },
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "Id"},
                        }
                    },
                    {
                        "UpdatedAt",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "UpdatedAt"},
                        }
                    },
                    {
                        "Version",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "Version"},
                        }
                    },
                });
            DropTable("apartmenthost.Tables",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreatedAt",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "CreatedAt"},
                        }
                    },
                    {
                        "Deleted",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "Deleted"},
                        }
                    },
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "Id"},
                        }
                    },
                    {
                        "UpdatedAt",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "UpdatedAt"},
                        }
                    },
                    {
                        "Version",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "Version"},
                        }
                    },
                });
            DropTable("apartmenthost.TableProp");
        }

        public override void Down()
        {
            CreateTable(
                "apartmenthost.TableProp",
                c => new
                {
                    TableRefId = c.String(nullable: false, maxLength: 128),
                    PropRefId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new {t.TableRefId, t.PropRefId});

            CreateTable(
                "apartmenthost.Tables",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "Id")
                            },
                        }),
                    Name = c.String(),
                    Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "Version")
                            },
                        }),
                    CreatedAt = c.DateTimeOffset(nullable: false, precision: 7,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                            },
                        }),
                    UpdatedAt = c.DateTimeOffset(precision: 7,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                            },
                        }),
                    Deleted = c.Boolean(nullable: false,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "Deleted")
                            },
                        }),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "apartmenthost.Props",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "Id")
                            },
                        }),
                    Name = c.String(),
                    Type = c.String(),
                    DataType = c.String(),
                    GetRule_Visible = c.Boolean(nullable: false),
                    GetRule_RequiredForm = c.Boolean(nullable: false),
                    GetRule_RequiredTransfer = c.Boolean(nullable: false),
                    GetRule_Order = c.Int(nullable: false),
                    PostRule_Visible = c.Boolean(nullable: false),
                    PostRule_RequiredForm = c.Boolean(nullable: false),
                    PostRule_RequiredTransfer = c.Boolean(nullable: false),
                    PostRule_Order = c.Int(nullable: false),
                    PutRule_Visible = c.Boolean(nullable: false),
                    PutRule_RequiredForm = c.Boolean(nullable: false),
                    PutRule_RequiredTransfer = c.Boolean(nullable: false),
                    PutRule_Order = c.Int(nullable: false),
                    DeleteRule_Visible = c.Boolean(nullable: false),
                    DeleteRule_RequiredForm = c.Boolean(nullable: false),
                    DeleteRule_RequiredTransfer = c.Boolean(nullable: false),
                    DeleteRule_Order = c.Int(nullable: false),
                    DictionaryId = c.String(maxLength: 128),
                    Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "Version")
                            },
                        }),
                    CreatedAt = c.DateTimeOffset(nullable: false, precision: 7,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                            },
                        }),
                    UpdatedAt = c.DateTimeOffset(precision: 7,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                            },
                        }),
                    Deleted = c.Boolean(nullable: false,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "Deleted")
                            },
                        }),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "apartmenthost.Dictionaries",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "Id")
                            },
                        }),
                    Name = c.String(),
                    Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "Version")
                            },
                        }),
                    CreatedAt = c.DateTimeOffset(nullable: false, precision: 7,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                            },
                        }),
                    UpdatedAt = c.DateTimeOffset(precision: 7,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                            },
                        }),
                    Deleted = c.Boolean(nullable: false,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "Deleted")
                            },
                        }),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "apartmenthost.DictionaryItems",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "Id")
                            },
                        }),
                    DictionaryId = c.String(nullable: false, maxLength: 128),
                    StrValue = c.String(),
                    NumValue = c.Decimal(precision: 18, scale: 2),
                    DateValue = c.DateTime(),
                    BoolValue = c.Boolean(),
                    Lang = c.String(),
                    Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "Version")
                            },
                        }),
                    CreatedAt = c.DateTimeOffset(nullable: false, precision: 7,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                            },
                        }),
                    UpdatedAt = c.DateTimeOffset(precision: 7,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                            },
                        }),
                    Deleted = c.Boolean(nullable: false,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "Deleted")
                            },
                        }),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "apartmenthost.PropVals",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "Id")
                            },
                        }),
                    PropId = c.String(nullable: false, maxLength: 128),
                    ApartmentItemId = c.String(maxLength: 128),
                    AdvertItemId = c.String(maxLength: 128),
                    ReservationItemId = c.String(maxLength: 128),
                    StrValue = c.String(),
                    NumValue = c.Decimal(precision: 18, scale: 2),
                    DateValue = c.DateTime(),
                    BoolValue = c.Boolean(),
                    DictionaryItemId = c.String(maxLength: 128),
                    Lang = c.String(),
                    Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "Version")
                            },
                        }),
                    CreatedAt = c.DateTimeOffset(nullable: false, precision: 7,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                            },
                        }),
                    UpdatedAt = c.DateTimeOffset(precision: 7,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                            },
                        }),
                    Deleted = c.Boolean(nullable: false,
                        annotations: new Dictionary<string, AnnotationValues>
                        {
                            {
                                "ServiceTableColumn",
                                new AnnotationValues(oldValue: null, newValue: "Deleted")
                            },
                        }),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("apartmenthost.Articles", "PictureId", c => c.String(maxLength: 128));
            DropForeignKey("apartmenthost.CardGenders", "CardId", "apartmenthost.Cards");
            DropIndex("apartmenthost.CardGenders", new[] {"CreatedAt"});
            DropIndex("apartmenthost.CardGenders", new[] {"CardId"});
            DropColumn("apartmenthost.Notifications", "Emailed");
            DropColumn("apartmenthost.Users", "Blocked");
            DropColumn("apartmenthost.Users", "ResetRequested");
            DropColumn("apartmenthost.Users", "EmailConfirmed");
            DropColumn("apartmenthost.Users", "SaltedAndHashedCode");
            DropColumn("apartmenthost.Users", "SaltedAndHashedEmail");
            DropTable("apartmenthost.CardGenders",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreatedAt",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "CreatedAt"},
                        }
                    },
                    {
                        "Deleted",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "Deleted"},
                        }
                    },
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "Id"},
                        }
                    },
                    {
                        "UpdatedAt",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "UpdatedAt"},
                        }
                    },
                    {
                        "Version",
                        new Dictionary<string, object>
                        {
                            {"ServiceTableColumn", "Version"},
                        }
                    },
                });
            CreateIndex("apartmenthost.TableProp", "PropRefId");
            CreateIndex("apartmenthost.TableProp", "TableRefId");
            CreateIndex("apartmenthost.Articles", "PictureId");
            CreateIndex("apartmenthost.Tables", "CreatedAt", clustered: true);
            CreateIndex("apartmenthost.Props", "CreatedAt", clustered: true);
            CreateIndex("apartmenthost.Props", "DictionaryId");
            CreateIndex("apartmenthost.Dictionaries", "CreatedAt", clustered: true);
            CreateIndex("apartmenthost.DictionaryItems", "CreatedAt", clustered: true);
            CreateIndex("apartmenthost.DictionaryItems", "DictionaryId");
            CreateIndex("apartmenthost.PropVals", "CreatedAt", clustered: true);
            CreateIndex("apartmenthost.PropVals", "DictionaryItemId");
            CreateIndex("apartmenthost.PropVals", "ReservationItemId");
            CreateIndex("apartmenthost.PropVals", "AdvertItemId");
            CreateIndex("apartmenthost.PropVals", "ApartmentItemId");
            CreateIndex("apartmenthost.PropVals", "PropId");
            AddForeignKey("apartmenthost.PropVals", "ApartmentItemId", "apartmenthost.Apartments", "Id");
            AddForeignKey("apartmenthost.PropVals", "AdvertItemId", "apartmenthost.Cards", "Id");
            AddForeignKey("apartmenthost.Articles", "PictureId", "apartmenthost.Pictures", "Id");
            AddForeignKey("apartmenthost.PropVals", "ReservationItemId", "apartmenthost.Reservations", "Id");
            AddForeignKey("apartmenthost.PropVals", "DictionaryItemId", "apartmenthost.DictionaryItems", "Id");
            AddForeignKey("apartmenthost.Props", "DictionaryId", "apartmenthost.Dictionaries", "Id");
            AddForeignKey("apartmenthost.TableProp", "PropRefId", "apartmenthost.Props", "Id", cascadeDelete: true);
            AddForeignKey("apartmenthost.TableProp", "TableRefId", "apartmenthost.Tables", "Id", cascadeDelete: true);
            AddForeignKey("apartmenthost.PropVals", "PropId", "apartmenthost.Props", "Id", cascadeDelete: true);
            AddForeignKey("apartmenthost.DictionaryItems", "DictionaryId", "apartmenthost.Dictionaries", "Id",
                cascadeDelete: true);
        }
    }
}