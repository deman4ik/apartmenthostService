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
                                                   Phone = "+79998887766"               
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
                                                   Phone = "+74957777777"               
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
                    Rating = new decimal(3.5)
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
                    Rating = new decimal(4.3)
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
                    Rating = new decimal(2.7)
                }
            };

            foreach (var apartment in apartments)
            {
                context.Set<Apartment>().Add(apartment);
            }
        }

        public static void PopulateTables(appartmenthostContext context)
        {
            Table table = new Table()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Apartment"
            };

            context.Set<Table>().Add(table);

        }

        public static void PopulateProps(appartmenthostContext context)
        {
            Table table = context.Tables.SingleOrDefault(t => t.Name == "Apartment");
            Prop prop = new Prop()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Тип жилья",
                Type = "Str",
                Tables = new List<Table>() { table }
            };
            context.Set<Prop>().Add(prop);
        }

        public static void PopulatePropVals(appartmenthostContext context)
        {
          //  Table table = context.Tables.SingleOrDefault(t => t.Name == "Apartment");
            Prop prop = context.Props.SingleOrDefault(p => p.Tables.Any(t => t.Name == "Apartment") && p.Name == "Тип Жилья");
            Apartment apartment = context.Apartments.SingleOrDefault(a => a.Name == "Офис Парус");
            
            
            PropVal propVal =  new PropVal()
            {
                Id = Guid.NewGuid().ToString(),
                PropId = prop.Id,
                TableItemId = apartment.Id,
                StrValue = "Офис"
                
            };
            prop.PropVals.Add(propVal);
            context.Entry(prop).State = System.Data.Entity.EntityState.Modified;  
        }
    }
}
