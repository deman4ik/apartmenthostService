using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Models;

namespace apartmenthostService.App_Start
{
    public class TestDBPopulator
    {
        public static void Populate(apartmenthostContext context)
        {
            try
            {
                PopulateUsers(context);
                context.SaveChanges();

                PopulateApartments(context);
                context.SaveChanges();

                PopulateAdverts(context);
                context.SaveChanges();

                PopulateTables(context);
                context.SaveChanges();

                PopulateDictionaries(context);
                context.SaveChanges();

                PopulateDictionaryItems(context);
                context.SaveChanges();

                PopulateProps(context);
                context.SaveChanges();

                PopulatePropVals(context);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("!!!!!!!!!!!!!!!!!!");
                System.Diagnostics.Debug.WriteLine(e.InnerException);
                System.Diagnostics.Debug.WriteLine("!!!!!!!!!!!!!!!!!!");
                System.Diagnostics.Debug.WriteLine(e);
            }

        }

        public static void PopulateUsers(apartmenthostContext context)
        {
            byte[] salt = AuthUtils.generateSalt();
            string id1 = "u1";
            string id2 = "u2";
            List<User> users = new List<User>
            {

                new User { Id = id1,
                           Email = "vasek@example.com", 
                           Salt = salt, 
                           SaltedAndHashedPassword = AuthUtils.hash("parusina", salt),
                           Profile = new Profile { Id = id1, 
                                                   FirstName = "Василий", 
                                                   LastName = "Пупович", 
                                                   Birthday = new DateTime(1976,3,23),
                                                   ContactEmail = "vasek@example.com",
                                                   ContactKind = "Email",
                                                   Description = "Пуповичи 100 лет на рынке недвижимости!",
                                                   Gender = "Male",
                                                   Phone = "+79998887766",
                                                   Rating = 4,
                                                   Lang = ConstLang.RU
                                                } 
                        },

                new User { Id = id2, 
                           Email = "parus@parus.ru", 
                           Salt = salt, 
                           SaltedAndHashedPassword = AuthUtils.hash("parusina", salt),
                           Profile = new Profile { Id = id2,
                                                   FirstName = "ЦИТК",
                                                   LastName = "Парус",
                                                   Birthday = new DateTime(1989,1,1),
                                                   ContactEmail = "parus@parus.ru",
                                                   ContactKind = "Phone",
                                                   Description = "Информационные Системы Управления",
                                                   Gender = "Male",
                                                   Phone = "+74957777777",
                                                   Rating = 5,
                                                   Lang = ConstLang.RU
                                                 }  
                        },
          };

           
            foreach (User user in users)
            {
                context.Set<User>().Add(user);
            }

            context.SaveChanges();

            AuthUtils.CreateAccount("standart", "parus@parus.ru", "standart:parus@parus.ru", "parus@parus.ru");

        }

        public static void PopulateApartments(apartmenthostContext context)
        {
            User user1 = context.Users.SingleOrDefault(u => u.Email == "vasek@example.com");
            User user2 = context.Users.SingleOrDefault(u => u.Email == "parus@parus.ru");

            List<Apartment> apartments = new List<Apartment>()
            {
                new Apartment()
                {
                    Id = "ap1",
                    Name = "Пупович Плаза",
                    UserId = user1.Id,
                    Adress = "Россия, Москва, Бутово, 1-я Горловская ул., 4, строение 21",
                    Latitude = new decimal(55.548484), 
                    Longitude = new decimal(37.581806),
                    Lang = ConstLang.RU
                },
                 new Apartment()
                {
                    Id = "ap2",
                    Name = "Пупович Ясенево",
                    UserId = user1.Id,
                    Adress = "Россия, Москва, Ясенево, Соловьиный пр., 18",
                    Latitude = new decimal(55.604284), 
                    Longitude = new decimal(37.554516),
                    Lang = ConstLang.RU
                },
                new Apartment()
                {
                    Id = "ap3",
                    Name = "Офис Парус",
                    UserId = user2.Id,
                    Adress = "Россия, Москва, Алексеевский, Ярославская ул., 10к4",
                    Latitude = new decimal(55.819068), 
                    Longitude = new decimal(37.649776),
                    Lang = ConstLang.RU
                }
            };

            foreach (var apartment in apartments)
            {
                context.Set<Apartment>().Add(apartment);
            }
        }

        public static void PopulateAdverts(apartmenthostContext context)
        {
            User user1 = context.Users.SingleOrDefault(u => u.Email == "vasek@example.com");
            User user2 = context.Users.SingleOrDefault(u => u.Email == "parus@parus.ru");
            Apartment apartment1 = context.Apartments.SingleOrDefault(a => a.Id == "ap1");
            Apartment apartment2 = context.Apartments.SingleOrDefault(a => a.Id == "ap2");
            Apartment apartment3 = context.Apartments.SingleOrDefault(a => a.Id == "ap3");

            List<Advert> adverts = new List<Advert>()
            {
                new Advert()
                {
                    Id = "a1",
                    Name = "Пупович хата в Ясенево",
                    UserId = user1.Id,
                    Description = "Великолепное жилье в центре Бутово. Комфортно и уютно. Из окна не видно помойки! Уже хорошо!",
                    ApartmentId = apartment1.Id,
                    DateFrom = new DateTimeOffset(2015,5,1,0,0,0,new TimeSpan(3,0,0)),
                    DateTo = new DateTimeOffset(2015,5,31,0,0,0,new TimeSpan(3,0,0)),
                    PriceDay = 968,
                    PricePeriod = 30000,
                    Lang = ConstLang.RU
                },
                new Advert()
                {
                    Id = "a2",
                    Name = "Пупович Плаза в Бутово",
                    UserId = user1.Id,
                    Description = "Квартирка на Соловьином. Жить можно, но не долго. Из окна почти ничего не видно, только стенку морга.",
                    ApartmentId = apartment2.Id,
                    DateFrom = new DateTimeOffset(2015,5,10,0,0,0,new TimeSpan(3,0,0)),
                    DateTo = new DateTimeOffset(2015,6,30,0,0,0,new TimeSpan(3,0,0)),
                    PriceDay = 1000,
                    PricePeriod = 50000,
                    Lang = ConstLang.RU
                },
                new Advert()
                {
                    Id = "a3",
                    Name = "Офис совместно с Парус",
                    UserId = user2.Id,
                    Description = "Бизнес центр ААА-класса. Многоуровневая паркова. Бесплатное питание. У нас есть печеньки!",
                    ApartmentId = apartment3.Id,
                    DateFrom = new DateTimeOffset(2015,4,30,0,0,0,new TimeSpan(3,0,0)),
                    DateTo = new DateTimeOffset(2015,12,31,0,0,0,new TimeSpan(3,0,0)),
                    PriceDay = 1500,
                    PricePeriod = 60000,
                    Lang = ConstLang.RU
                },
            };

            foreach (var advert in adverts)
            {
                context.Set<Advert>().Add(advert);
            }
        }
        public static void PopulateTables(apartmenthostContext context)
        {
            List<Table> tables = new List<Table>()
            {
                new Table()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ConstTable.ApartmentTable
                },
                new Table()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ConstTable.AdvertTable
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
                },

            };

            foreach (var table in tables)
            {
                context.Set<Table>().Add(table);
            }
            

        }

        public static void PopulateDictionaries(apartmenthostContext context)
        {
            //ApartmentType
            List<Dictionary> dictionaries = new List<Dictionary>()
            {
                new Dictionary()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ConstProp.ApartmentType
                },
                new Dictionary()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ConstProp.CohabitationType
                },
                 new Dictionary()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ConstProp.ApartmentOptions
                },
            };

            foreach (var dictionary in dictionaries)
            {
                context.Set<Dictionary>().Add(dictionary);
            }
        }

        public static void PopulateDictionaryItems(apartmenthostContext context)
        {
            List<DictionaryItem> dictionaryItems = new List<DictionaryItem>();
            //ApartmentType
            Dictionary apartmentTypeDic = context.Dictionaries.SingleOrDefault(a => a.Name == ConstProp.ApartmentType);
            foreach (var apartmentType in ConstDicValsRU.ApartmentTypesList())
            {
                dictionaryItems.Add(new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = apartmentTypeDic.Id,
                    StrValue = apartmentType,
                    Lang = ConstLang.RU
                });
            }
            //CohabitationType
            Dictionary cohabitationTypeDic = context.Dictionaries.SingleOrDefault(a => a.Name == ConstProp.CohabitationType);
            foreach (var cohabitationType in ConstDicValsRU.CohabitationTypesList())
            {
                dictionaryItems.Add(new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = cohabitationTypeDic.Id,
                    StrValue = cohabitationType,
                    Lang = ConstLang.RU
                });
            }

            //ApartmentOptions
            Dictionary apartmentOptionDic = context.Dictionaries.SingleOrDefault(a => a.Name == ConstProp.ApartmentOptions);
            foreach (var apartmentOption in ConstDicValsRU.ApartmentOptionsList())
            {
                dictionaryItems.Add(new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = apartmentOptionDic.Id,
                    StrValue = apartmentOption,
                    Lang = ConstLang.RU
                });
            }
            foreach (var dictonaryItem in dictionaryItems)
            {
                context.Set<DictionaryItem>().Add(dictonaryItem);
            }
            
        }
        public static void PopulateProps(apartmenthostContext context)
        {
            List<Prop> propsList = new List<Prop>();

            // Apartment
            Table apartmentTable = context.Tables.SingleOrDefault(t => t.Name == ConstTable.ApartmentTable);
            Table advertTable = context.Tables.SingleOrDefault(t => t.Name == ConstTable.AdvertTable);
            Dictionary apartmentTypeDic = context.Dictionaries.SingleOrDefault(a => a.Name == ConstProp.ApartmentType);
            Dictionary cohabitationTypeDic = context.Dictionaries.SingleOrDefault(a => a.Name == ConstProp.CohabitationType);
            Dictionary apartmentOptionDic = context.Dictionaries.SingleOrDefault(a => a.Name == ConstProp.ApartmentOptions);
           // Custom : ApartmentType
            propsList.Add(new Prop()
            {
                Id = Guid.NewGuid().ToString(),
                Name = ConstProp.ApartmentType,
                Type = ConstType.Str,
                DataType = ConstDataType.List,
                Visbile = true,
                Required = true,
                DictionaryId = apartmentTypeDic.Id,
                Dictionary = apartmentTypeDic,
                Tables = new List<Table>() { apartmentTable }

            });
            // Base : CohabitationType
            propsList.Add(new Prop()
            {
                Id = Guid.NewGuid().ToString(),
                Name = ConstProp.CohabitationType,
                Type = ConstType.Str,
                Visbile = true,
                Required = true,
                DataType = ConstDataType.List,
                DictionaryId = cohabitationTypeDic.Id,
                Dictionary = cohabitationTypeDic,
                Tables = new List<Table>() { apartmentTable }

            });

            // Base : ApartmentOption
            propsList.Add(new Prop()
            {
                Id = Guid.NewGuid().ToString(),
                Name = ConstProp.ApartmentOptions,
                Type = ConstType.Str,
                Visbile = true,
                Required = false,
                DataType = ConstDataType.Multibox,
                DictionaryId = apartmentOptionDic.Id,
                Dictionary = apartmentOptionDic,
                Tables = new List<Table>() { advertTable }

            });

            foreach (var prop in propsList)
            {
                context.Set<Prop>().Add(prop);
            }
           
        }

        public static void PopulatePropVals(apartmenthostContext context)
        {
          //  Table table = context.Tables.SingleOrDefault(t => t.Name == "Apartment");
            
            Prop propApartmentType = context.Props.SingleOrDefault(p => p.Tables.Any(t => t.Name == ConstTable.ApartmentTable) && p.Name == ConstProp.ApartmentType);
            Apartment apartmentOffice = context.Apartments.SingleOrDefault(a => a.Name == "Офис Парус");
            Dictionary dicApartmentType = context.Dictionaries.SingleOrDefault(a => a.Name == ConstProp.ApartmentType);
            DictionaryItem dicItemOffice =
                context.DictionaryItems.SingleOrDefault(i => i.DictionaryId == dicApartmentType.Id && i.StrValue == "Office");
            DictionaryItem dicItemFlat =
                context.DictionaryItems.SingleOrDefault(i => i.DictionaryId == dicApartmentType.Id && i.StrValue == "Flat");


            Prop propCohab = context.Props.SingleOrDefault(p => p.Tables.Any(t => t.Name == ConstTable.ApartmentTable) && p.Name == ConstProp.CohabitationType);
            Apartment apartmentPlaza = context.Apartments.SingleOrDefault(a => a.Name == "Пупович Плаза");
            Apartment apartmentYasenevo = context.Apartments.SingleOrDefault(a => a.Name == "Пупович Ясенево");
            Dictionary dicCohab = context.Dictionaries.SingleOrDefault(a => a.Name == ConstProp.CohabitationType);
            DictionaryItem dicItemSepRes =
                context.DictionaryItems.SingleOrDefault(i => i.DictionaryId == dicCohab.Id && i.StrValue == "Separate residence");
            DictionaryItem dicItemCohab =
                context.DictionaryItems.SingleOrDefault(i => i.DictionaryId == dicCohab.Id && i.StrValue == "Cohabitation");


            Prop propApartmentOption = context.Props.SingleOrDefault(p => p.Name == ConstProp.ApartmentOptions);
            Advert advertYasenevo = context.Adverts.SingleOrDefault(a => a.Id == "a1");
            Advert advertPlaza = context.Adverts.SingleOrDefault(a => a.Id == "a2");
            Advert advertOffice = context.Adverts.SingleOrDefault(a => a.Id == "a3");
            Dictionary dicOption = context.Dictionaries.SingleOrDefault(a => a.Name == ConstProp.ApartmentOptions);
            DictionaryItem dicItemParking =
               context.DictionaryItems.SingleOrDefault(i => i.DictionaryId == dicOption.Id && i.StrValue == "Parking");
            DictionaryItem dicItemСoncierge =
               context.DictionaryItems.SingleOrDefault(i => i.DictionaryId == dicOption.Id && i.StrValue == "Сoncierge");
            DictionaryItem dicItemRefrigerator =
               context.DictionaryItems.SingleOrDefault(i => i.DictionaryId == dicOption.Id && i.StrValue == "Refrigerator");
            DictionaryItem dicItemWM =
               context.DictionaryItems.SingleOrDefault(i => i.DictionaryId == dicOption.Id && i.StrValue == "Washing machine");
            DictionaryItem dicItemAC =
               context.DictionaryItems.SingleOrDefault(i => i.DictionaryId == dicOption.Id && i.StrValue == "Air conditioning");

            List<PropVal> propVals = new List<PropVal>();
            //ApartmentType
          propVals.Add(new PropVal()
            {
                Id = Guid.NewGuid().ToString(),
                PropId = propApartmentType.Id,
                ApartmentItemId = apartmentOffice.Id,
                DictionaryItemId = dicItemOffice.Id,
                Lang = ConstLang.RU,  
            });

          propVals.Add(new PropVal()
          {
              Id = Guid.NewGuid().ToString(),
              PropId = propApartmentType.Id,
              ApartmentItemId = apartmentPlaza.Id,
              DictionaryItemId = dicItemFlat.Id,
              Lang = ConstLang.RU
          });

          propVals.Add(new PropVal()
          {
              Id = Guid.NewGuid().ToString(),
              PropId = propApartmentType.Id,
              ApartmentItemId = apartmentYasenevo.Id,
              DictionaryItemId = dicItemFlat.Id,
              Lang = ConstLang.RU
          });
          //CohabType
          propVals.Add(new PropVal()
          {
              Id = Guid.NewGuid().ToString(),
              PropId = propCohab.Id,
              ApartmentItemId = apartmentOffice.Id,
              DictionaryItemId = dicItemCohab.Id,
              Lang = ConstLang.RU
          });

          propVals.Add(new PropVal()
          {
              Id = Guid.NewGuid().ToString(),
              PropId = propCohab.Id,
              ApartmentItemId = apartmentPlaza.Id,
              DictionaryItemId = dicItemSepRes.Id,
              Lang = ConstLang.RU
          });

          propVals.Add(new PropVal()
          {
              Id = Guid.NewGuid().ToString(),
              PropId = propCohab.Id,
              ApartmentItemId = apartmentYasenevo.Id,
              DictionaryItemId = dicItemCohab.Id,
              Lang = ConstLang.RU
          });

          // ApartmentOption
          propVals.Add(new PropVal()
          {
              Id = Guid.NewGuid().ToString(),
              PropId = propApartmentOption.Id,
              AdvertItemId = advertPlaza.Id,
              DictionaryItemId = dicItemParking.Id,
              Lang = ConstLang.RU
          });

          propVals.Add(new PropVal()
          {
              Id = Guid.NewGuid().ToString(),
              PropId = propApartmentOption.Id,
              AdvertItemId = advertPlaza.Id,
              DictionaryItemId = dicItemСoncierge.Id,
              Lang = ConstLang.RU
          });

          propVals.Add(new PropVal()
          {
              Id = Guid.NewGuid().ToString(),
              PropId = propApartmentOption.Id,
              AdvertItemId = advertPlaza.Id,
              DictionaryItemId = dicItemRefrigerator.Id,
              Lang = ConstLang.RU
          });

          propVals.Add(new PropVal()
          {
              Id = Guid.NewGuid().ToString(),
              PropId = propApartmentOption.Id,
              AdvertItemId = advertPlaza.Id,
              DictionaryItemId = dicItemWM.Id,
              Lang = ConstLang.RU
          });

          propVals.Add(new PropVal()
          {
              Id = Guid.NewGuid().ToString(),
              PropId = propApartmentOption.Id,
              AdvertItemId = advertPlaza.Id,
              DictionaryItemId = dicItemAC.Id,
              Lang = ConstLang.RU
          });

          propVals.Add(new PropVal()
          {
              Id = Guid.NewGuid().ToString(),
              PropId = propApartmentOption.Id,
              AdvertItemId = advertOffice.Id,
              DictionaryItemId = dicItemСoncierge.Id,
              Lang = ConstLang.RU
          });

          propVals.Add(new PropVal()
          {
              Id = Guid.NewGuid().ToString(),
              PropId = propApartmentOption.Id,
              AdvertItemId = advertYasenevo.Id,
              DictionaryItemId = dicItemRefrigerator.Id,
              Lang = ConstLang.RU
          });
            foreach (var propVal in propVals)
            {
                context.PropVals.Add(propVal);
            }

        }
    }
}
