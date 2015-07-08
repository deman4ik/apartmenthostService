using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using apartmenthostService.Helpers;
using apartmenthostService.Models;

namespace apartmenthostService.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<apartmenthostContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        public static void PopulateTables(apartmenthostContext context)
        {
            context.Tables.AddOrUpdate(
                p => p.Name,
                new Table()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ConstTable.ApartmentTable
                },
                new Table()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ConstTable.CardTable
                },
                new Table()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ConstTable.ProfileTable
                },
                new Table()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ConstTable.ReservationTable
                });
            context.SaveChanges();
        }

        public static void PopulateDictionaries(apartmenthostContext context)
        {
            context.Dictionaries.AddOrUpdate(
                p => p.Name,
                new Dictionary()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ConstDictionary.ApartmentOptions
                },
                new Dictionary()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ConstDictionary.ApartmentType
                },
                new Dictionary()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ConstDictionary.Cohabitation
                },
                new Dictionary()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ConstDictionary.Gender
                });
            context.SaveChanges();
        }

        public static void PopulateDictionaryItems(apartmenthostContext context)
        {
            Dictionary apartmentTypeDic = context.Dictionaries.SingleOrDefault(a => a.Name == ConstDictionary.ApartmentType);
            Dictionary cohabitationTypeDic = context.Dictionaries.SingleOrDefault(a => a.Name == ConstDictionary.Cohabitation);
            Dictionary genderDic = context.Dictionaries.SingleOrDefault(a => a.Name == ConstDictionary.Gender);
           
            context.DictionaryItems.AddOrUpdate(p => p.StrValue,
                new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = apartmentTypeDic.Id,
                    StrValue = ConstVals.House
                },
                new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = apartmentTypeDic.Id,
                    StrValue = ConstVals.Flat
                },
                new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = apartmentTypeDic.Id,
                    StrValue = ConstVals.Room
                },
                new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = apartmentTypeDic.Id,
                    StrValue = ConstVals.Office
                },
                new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = apartmentTypeDic.Id,
                    StrValue = ConstVals.HotelRoom
                },
                new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = cohabitationTypeDic.Id,
                    StrValue = ConstVals.SeperateResidence
                },
                new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = cohabitationTypeDic.Id,
                    StrValue = ConstVals.Cohabitation
                },
                new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = cohabitationTypeDic.Id,
                    StrValue = ConstVals.Any
                },
                new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = genderDic.Id,
                    StrValue = ConstVals.Male
                },
                new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = genderDic.Id,
                    StrValue = ConstVals.Female
                },
                new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = genderDic.Id,
                    StrValue = ConstVals.Any
                },
                new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = genderDic.Id,
                    StrValue = ConstVals.Thing
                },
                new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = genderDic.Id,
                    StrValue = ConstVals.Alien
                }
                );

            context.SaveChanges();

        }
        protected override void Seed(apartmenthostContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            PopulateTables(context);
            PopulateDictionaries(context);
            PopulateDictionaryItems(context);
            TestDBPopulator.Populate(context);
        }
    }
}
