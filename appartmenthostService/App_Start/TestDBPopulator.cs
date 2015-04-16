using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using appartmenthostService.Authentication;
using appartmenthostService.Helpers;
using appartmenthostService.Models;

namespace appartmenthostService.App_Start
{
    public class TestDBPopulator
    {
        public static void Populate(appartmenthostContext context)
        {
            PopulateUsers(context);
            context.SaveChanges();

            PopulateApartments(context);
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

        public static void PopulateUsers(appartmenthostContext context)
        {
            byte[] salt = AuthUtils.generateSalt();
            string id1 = Guid.NewGuid().ToString();
            string id2 = Guid.NewGuid().ToString();
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
                                                   Lang = ConstLang.RU
                                                 }  
                        },
          };

           
            foreach (User user in users)
            {
                context.Set<User>().Add(user);
            }
           
        }

        public static void PopulateApartments(appartmenthostContext context)
        {
            User user1 = context.Users.SingleOrDefault(u => u.Email == "vasek@example.com");
            User user2 = context.Users.SingleOrDefault(u => u.Email == "parus@parus.ru");

            List<Apartment> apartments = new List<Apartment>()
            {
                new Apartment()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Пупович Плаза",
                    UserId = user1.Id,
                    Cohabitation = "Раздельное",
                   Price = 2000,
                    Adress = "Россия, Москва, Бутово, 1-я Горловская ул., 4, строение 21",
                    Latitude = new decimal(55.548484), 
                    Longitude = new decimal(37.581806),
                    Rating = new decimal(3.5),
                    Lang = ConstLang.RU
                },
                 new Apartment()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Пупович Ясенево",
                    UserId = user1.Id,
                    Cohabitation = "Совместное",
                    Price = 1000,
                    Adress = "Россия, Москва, Ясенево, Соловьиный пр., 18",
                    Latitude = new decimal(55.604284), 
                    Longitude = new decimal(37.554516),
                    Rating = new decimal(4.3),
                    Lang = ConstLang.RU
                },
                new Apartment()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Офис Парус",
                    UserId = user2.Id,
                    Cohabitation = "Совместное",
                    Price = 3000,
                    Adress = "Россия, Москва, Алексеевский, Ярославская ул., 10к4",
                    Latitude = new decimal(55.819068), 
                    Longitude = new decimal(37.649776),
                    Rating = new decimal(2.7),
                    Lang = ConstLang.RU
                }
            };

            foreach (var apartment in apartments)
            {
                context.Set<Apartment>().Add(apartment);
            }
        }

        public static void PopulateTables(appartmenthostContext context)
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

        public static void PopulateDictionaries(appartmenthostContext context)
        {
            //ApartmentType
            List<Dictionary> dictionaries = new List<Dictionary>()
            {
                new Dictionary()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ConstProp.ApartmentType
                }
            };

            foreach (var dictionary in dictionaries)
            {
                context.Set<Dictionary>().Add(dictionary);
            }
        }

        public static void PopulateDictionaryItems(appartmenthostContext context)
        {
            //ApartmentType
            Dictionary apartmentTypeDic = context.Dictionaries.SingleOrDefault(a => a.Name == ConstProp.ApartmentType);
            List<DictionaryItem> dictionaryItems = new List<DictionaryItem>();

            foreach (var apartmentType in ConstDicValsRU.ApartmentTypesList)
            {
                dictionaryItems.Add(new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = apartmentTypeDic.Name,
                    StrValue = apartmentType,
                    Lang = ConstLang.RU
                    
                });
            }
                
            foreach (var dictonaryItem in dictionaryItems)
            {
                context.Set<DictionaryItem>().Add(dictonaryItem);
            }
            
        }
        public static void PopulateProps(appartmenthostContext context)
        {
            Table apartmentTable = context.Tables.SingleOrDefault(t => t.Name == ConstTable.ApartmentTable);
            Dictionary apartmentTypeDic = context.Dictionaries.SingleOrDefault(a => a.Name == ConstProp.ApartmentType);
            Prop prop = new Prop()
            {
                Id = Guid.NewGuid().ToString(),
                Name = ConstProp.ApartmentType,
                DataType = ConstDataType.Str,
                Type = Const.Custom,
                DictionaryId = apartmentTypeDic.Id,
                Dictionary = apartmentTypeDic,
                Tables = new List<Table>() { apartmentTable }

            };
            context.Set<Prop>().Add(prop);
        }

        public static void PopulatePropVals(appartmenthostContext context)
        {
          //  Table table = context.Tables.SingleOrDefault(t => t.Name == "Apartment");
            Prop prop = context.Props.SingleOrDefault(p => p.Tables.Any(t => t.Name == ConstTable.ApartmentTable) && p.Name == ConstProp.ApartmentType);
            Apartment apartment = context.Apartments.SingleOrDefault(a => a.Name == "Офис Парус");
            Dictionary apartmentTypeDic = context.Dictionaries.SingleOrDefault(a => a.Name == ConstProp.ApartmentType);
            DictionaryItem officeItem =
                context.DictionaryItems.SingleOrDefault(i => i.Dictionary == apartmentTypeDic && i.Name == "Офис");
            

            PropVal propVal =  new PropVal()
            {
                Id = Guid.NewGuid().ToString(),
                PropId = prop.Id,
                TableItemId = apartment.Id,
                DictionaryItemId = officeItem.Id,
                DictionaryItem = officeItem
                
            };
            prop.PropVals.Add(propVal);
            context.Entry(prop).State = System.Data.Entity.EntityState.Modified;  
        }
    }
}
