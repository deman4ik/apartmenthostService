using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

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

                PopulateCards(context);
                context.SaveChanges();

                PopulateFavorites(context);
                context.SaveChanges();

                PopulateReservations(context);
                context.SaveChanges();

                PopulateReviews(context);
                context.SaveChanges();

                PopulateNotifications(context);
                context.SaveChanges();

                PopulateTables(context);
                context.SaveChanges();

                PopulateDictionaries(context);
                context.SaveChanges();

                PopulateDictionaryItems(context);
                context.SaveChanges();

                RatingJob ratingJob = new RatingJob();
                ratingJob.ExecuteAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("!!!!!!!!!!!!!!!!!!");
                Debug.WriteLine(e.InnerException);
                Debug.WriteLine("!!!!!!!!!!!!!!!!!!");
                Debug.WriteLine(e);
            }

        }

        public static void PopulateUsers(apartmenthostContext context)
        {
            byte[] salt = AuthUtils.generateSalt();

            CloudinaryDotNet.Account account = new CloudinaryDotNet.Account(
  "apartmenthost",
  "276555246944232",
  "agPi304Lt89nplAmwkUTzzuMjwk");

            Cloudinary cloudinary = new Cloudinary(account);
            List<User> users = new List<User>
            {
                new User { Id = "u1", 
                           Email = "parus@parus.ru", 
                           Salt = salt, 
                           SaltedAndHashedPassword = AuthUtils.hash("parusina", salt),
                           Profile = new Profile { Id = "u1",
                                                   FirstName = "Яна",
                                                   LastName = "Парусова",
                                                   Birthday = new DateTime(1989,1,1),
                                                   ContactEmail = "parus@parus.ru",
                                                   ContactKind = "Phone",
                                                   Description = "Информационные Системы Управления",
                                                   Gender = ConstVals.Female,
                                                   Phone = "+74957777777",
                                                   Rating = 0,
                                                   RatingCount = 0,
                                                   Score = 0,
                                                   Lang = ConstLang.RU,
                                                   Picture = new Picture()
                                                   {
                                                       Id = "p1",
                                                       Name = "profile/u1.jpg",
                                                       Description = "Яна Парусова",
                                                       Url = cloudinary.Api.UrlImgUp.BuildUrl("profile/u1.jpg"),
                                                       CloudinaryPublicId = "profile/u1"
                                                   }
                                                 }  
                        },
                new User { Id = "u2",
                           Email = "user2@example.com", 
                           Salt = salt, 
                           SaltedAndHashedPassword = AuthUtils.hash("user2", salt),
                           Profile = new Profile { Id = "u2", 
                                                   FirstName = "Василий", 
                                                   LastName = "Пупович", 
                                                   Birthday = new DateTime(1976,3,23),
                                                   ContactEmail = "user2@example.com",
                                                   ContactKind = "Email",
                                                   Description = "Пуповичи 100 лет на рынке недвижимости!",
                                                   Gender = ConstVals.Male,
                                                   Phone = "+79998887766",
                                                   Rating = 0,
                                                   RatingCount = 0,
                                                   Score = 0,
                                                   Lang = ConstLang.RU,
                                                   Picture = new Picture()
                                                   {
                                                       Id = "p2",
                                                       Name = "profile/u2.jpg",
                                                       Description = "Василий Пупович",
                                                       Url = cloudinary.Api.UrlImgUp.BuildUrl("profile/u2.jpg"),
                                                       CloudinaryPublicId = "profile/u2"
                                                   }
                                                } 
                        },
                
                        new User { Id = "u3",
                           Email = "user3@example.com", 
                           Salt = salt, 
                           SaltedAndHashedPassword = AuthUtils.hash("user3", salt),
                           Profile = new Profile { Id = "u3", 
                                                   FirstName = "Елена", 
                                                   LastName = "Пыжович", 
                                                   Birthday = new DateTime(1976,3,23),
                                                   ContactEmail = "user3@example.com",
                                                   ContactKind = "Email",
                                                   Description = "Привет. Меня зовут Лена!",
                                                   Gender = ConstVals.Female,
                                                   Phone = "+79998987766",
                                                   Rating = 0,
                                                   RatingCount = 0,
                                                   Score = 0,
                                                   Lang = ConstLang.RU,
                                                   Picture = new Picture()
                                                   {
                                                       Id = "p3",
                                                       Name = "profile/u3.jpg",
                                                       Description = "Елена Пыжович",
                                                       Url = cloudinary.Api.UrlImgUp.BuildUrl("profile/u3.jpg"),
                                                       CloudinaryPublicId = "profile/u3"
                                                   }
                                                } 
                        },

                        new User { Id = "u4",
                           Email = "user4@example.com", 
                           Salt = salt, 
                           SaltedAndHashedPassword = AuthUtils.hash("user4", salt),
                           Profile = new Profile { Id = "u4", 
                                                   FirstName = "Дмитрий", 
                                                   LastName = "Трофимов", 
                                                   Birthday = new DateTime(1965,7,12),
                                                   ContactEmail = "user4@example.com",
                                                   ContactKind = "Email",
                                                   Description = "Трофимов",
                                                   Gender = ConstVals.Male,
                                                   Phone = "+79995487766",
                                                   Rating = 0,
                                                   RatingCount = 0,
                                                   Score = 0,
                                                   Lang = ConstLang.RU,
                                                   Picture = new Picture()
                                                   {
                                                       Id = "p4",
                                                       Name = "profile/u4.jpg",
                                                       Description = "Дмитрий Трофимов",
                                                       Url = cloudinary.Api.UrlImgUp.BuildUrl("profile/u4.jpg"),
                                                       CloudinaryPublicId = "profile/u4"
                                                   }
                                                } 
                        },

                        new User { Id = "u5",
                           Email = "user5@example.com", 
                           Salt = salt, 
                           SaltedAndHashedPassword = AuthUtils.hash("user5", salt),
                           Profile = new Profile { Id = "u5", 
                                                   FirstName = "Эдуард", 
                                                   LastName = "Вишняков", 
                                                   Birthday = new DateTime(1986,1,18),
                                                   ContactEmail = "user5@example.com",
                                                   ContactKind = "Email",
                                                   Description = "Трофимов",
                                                   Gender = ConstVals.Male,
                                                   Phone = "+78795487766",
                                                   Rating = 0,
                                                   RatingCount = 0,
                                                   Score = 0,
                                                   Lang = ConstLang.RU,
                                                   Picture = new Picture()
                                                   {
                                                       Id = "p5",
                                                       Name = "profile/u5.jpg",
                                                       Description = "Эдуард Вишняков",
                                                       Url = cloudinary.Api.UrlImgUp.BuildUrl("profile/u5.jpg"),
                                                       CloudinaryPublicId = "profile/u5"
                                                   }
                                                } 
                        },
                        new User { Id = "u6",
                           Email = "user6@example.com", 
                           Salt = salt, 
                           SaltedAndHashedPassword = AuthUtils.hash("user6", salt),
                           Profile = new Profile { Id = "u6", 
                                                   FirstName = "Леонид", 
                                                   LastName = "Нефедов", 
                                                   Birthday = new DateTime(1977,12,23),
                                                   ContactEmail = "user6@example.com",
                                                   ContactKind = "Phone",
                                                   Description = "Вишняков",
                                                   Gender = ConstVals.Male,
                                                   Phone = "+78795487366",
                                                   Rating = 0,
                                                   RatingCount = 0,
                                                   Score = 0,
                                                   Lang = ConstLang.RU,
                                                   Picture = new Picture()
                                                   {
                                                       Id = "p6",
                                                       Name = "profile/u6.jpg",
                                                       Description = "Леонид Нефедов",
                                                       Url = cloudinary.Api.UrlImgUp.BuildUrl("profile/u6.jpg"),
                                                       CloudinaryPublicId = "profile/u6"
                                                   }
                                                } 
                        },
                new User { Id = "u7",
                           Email = "user7@example.com", 
                           Salt = salt, 
                           SaltedAndHashedPassword = AuthUtils.hash("user7", salt),
                           Profile = new Profile { Id = "u7", 
                                                   FirstName = "Дарья", 
                                                   LastName = "Мамонтова", 
                                                   Birthday = new DateTime(1995,8,19),
                                                   ContactEmail = "user7@example.com",
                                                   ContactKind = "Email",
                                                   Description = "Привет. Меня зовут Дарья!",
                                                   Gender = ConstVals.Female,
                                                   Phone = "+79998988966",
                                                   Rating = 0,
                                                   RatingCount = 0,
                                                   Score = 0,
                                                   Lang = ConstLang.RU,
                                                   Picture = new Picture()
                                                   {
                                                       Id = "p7",
                                                       Name = "profile/u7.jpg",
                                                       Description = "Дарья Мамонтова",
                                                       Url = cloudinary.Api.UrlImgUp.BuildUrl("profile/u7.jpg"),
                                                       CloudinaryPublicId = "profile/u7"
                                                   }
                                                } 
                        },
                        new User { Id = "u8",
                           Email = "user8@example.com", 
                           Salt = salt, 
                           SaltedAndHashedPassword = AuthUtils.hash("user8", salt),
                           Profile = new Profile { Id = "u8", 
                                                   FirstName = "Светлана", 
                                                   LastName = "Стрелкова", 
                                                   Birthday = new DateTime(1989,9,9),
                                                   ContactEmail = "user8@example.com",
                                                   ContactKind = "Email",
                                                   Description = "Привет. Меня зовут Светлана!",
                                                   Gender = ConstVals.Female,
                                                   Phone = "+79994988966",
                                                   Rating = 0,
                                                   RatingCount = 0,
                                                   Score = 0,
                                                   Lang = ConstLang.RU,
                                                   Picture = new Picture()
                                                   {
                                                       Id = "p8",
                                                       Name = "profile/u8.jpg",
                                                       Description = "Светлана Стрелкова",
                                                       Url = cloudinary.Api.UrlImgUp.BuildUrl("profile/u8.jpg"),
                                                       CloudinaryPublicId = "profile/u8"
                                                   }
                                                } 
                        },
                         new User { Id = "u9",
                           Email = "user9@example.com", 
                           Salt = salt, 
                           SaltedAndHashedPassword = AuthUtils.hash("user9", salt),
                           Profile = new Profile { Id = "u9", 
                                                   FirstName = "Даша", 
                                                   LastName = "Демидова", 
                                                   Birthday = new DateTime(1981,4,29),
                                                   ContactEmail = "user9@example.com",
                                                   ContactKind = "Email",
                                                   Description = "Привет. Меня зовут Светлана!",
                                                   Gender = ConstVals.Female,
                                                   Phone = "+79994988996",
                                                   Rating = 0,
                                                   RatingCount = 0,
                                                   Score = 0,
                                                   Lang = ConstLang.RU,
                                                   Picture = new Picture()
                                                   {
                                                       Id = "p9",
                                                       Name = "profile/u9.jpg",
                                                       Description = "Лера Демидова",
                                                       Url = cloudinary.Api.UrlImgUp.BuildUrl("profile/u9.jpg"),
                                                       CloudinaryPublicId = "profile/u9"
                                                   }
                                                } 
                        },
                        new User { Id = "u10",
                           Email = "user10@example.com", 
                           Salt = salt, 
                           SaltedAndHashedPassword = AuthUtils.hash("user10", salt),
                           Profile = new Profile { Id = "u10", 
                                                   FirstName = "Лариса", 
                                                   LastName = "Крокодилова", 
                                                   Birthday = new DateTime(1993,4,10),
                                                   ContactEmail = "user10@example.com",
                                                   ContactKind = "Email",
                                                   Description = "Крокодилова клац-клац",
                                                   Gender = ConstVals.Female,
                                                   Phone = "+79994938996",
                                                   Rating = 0,
                                                   RatingCount = 0,
                                                   Score = 0,
                                                   Lang = ConstLang.RU,
                                                   Picture = new Picture()
                                                   {
                                                       Id = "p10",
                                                       Name = "profile/u10.jpg",
                                                       Description = "Лариса Крокодилова",
                                                       Url = cloudinary.Api.UrlImgUp.BuildUrl("profile/u10.jpg"),
                                                       CloudinaryPublicId = "profile/u10"
                                                   }
                                                } 
                        },
                         new User { Id = "u11",
                           Email = "user11@example.com",
                           Salt = salt,
                           SaltedAndHashedPassword = AuthUtils.hash("user11", salt),
                           Profile = new Profile { Id = "u11",
                                                   FirstName = "Лера",
                                                   LastName = "Бундельера",
                                                   Birthday = new DateTime(1988,4,10),
                                                   ContactEmail = "user11@example.com",
                                                   ContactKind = "Email",
                                                   Description = "Бундельер цап-цап",
                                                   Gender = ConstVals.Female,
                                                   Phone = "+79994938996",
                                                   Rating = 0,
                                                   RatingCount = 0,
                                                   Score = 0,
                                                   Lang = ConstLang.RU,
                                                   Picture = new Picture()
                                                   {
                                                       Id = "p11",
                                                       Name = "profile/u11.jpg",
                                                       Description = "Лера Бундельера",
                                                       Url = cloudinary.Api.UrlImgUp.BuildUrl("profile/u11.jpg"),
                                                       CloudinaryPublicId = "profile/u11"
                                                   }
                                                }
                        },
                          new User { Id = "u12",
                           Email = "user12@example.com",
                           Salt = salt,
                           SaltedAndHashedPassword = AuthUtils.hash("user12", salt),
                           Profile = new Profile { Id = "u12",
                                                   FirstName = "Владмир",
                                                   LastName = "Путкин",
                                                   Birthday = new DateTime(1991,4,10),
                                                   ContactEmail = "user12@example.com",
                                                   ContactKind = "Email",
                                                   Description = "не путать с ВВП",
                                                   Gender = ConstVals.Male,
                                                   Phone = "+79994938996",
                                                   Rating = 0,
                                                   RatingCount = 0,
                                                   Score = 0,
                                                   Lang = ConstLang.RU,
                                                   Picture = new Picture()
                                                   {
                                                       Id = "p12",
                                                       Name = "profile/u12.jpg",
                                                       Description = "Владмир Путкин",
                                                       Url = cloudinary.Api.UrlImgUp.BuildUrl("profile/u12.jpg"),
                                                       CloudinaryPublicId = "profile/u12"
                                                   }
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
             
            List<Apartment> apartments = new List<Apartment>()
            {
                new Apartment()
                {
                    Id = "ap1",
                    Name = "Офис Парус",
                    Type = ConstVals.Office,
                    Options = ConstVals.Parking + "," + ConstVals.Concierge + "," + ConstVals.AirConditioning,
                    UserId = "u1",
                    Adress = "Россия, Москва, Алексеевский, Ярославская ул., 10к4",
                    Latitude = new decimal(55.819068), 
                    Longitude = new decimal(37.649776),
                    Lang = ConstLang.RU
                },
                new Apartment()
                {
                    Id = "ap2",
                    Name = "Пупович Плаза",
                    Type = ConstVals.House,
                    Options = ConstVals.Parking + "," + ConstVals.Concierge + "," + ConstVals.AirConditioning + "," +ConstVals.WashingMachine+","+ConstVals.Refrigerator,
                    UserId = "u2",
                    Adress = "Россия, Москва, Бутово, 1-я Горловская ул., 4, строение 21",
                    Latitude = new decimal(55.548484), 
                    Longitude = new decimal(37.581806),
                    Lang = ConstLang.RU
                },
                 new Apartment()
                {
                    Id = "ap3",
                    Name = "Ленкина квартирка в Ясенево",
                    Type = ConstVals.Flat,
                    Options = ConstVals.WashingMachine+","+ConstVals.Refrigerator,
                    UserId = "u3",
                    Adress = "Россия, Москва, Ясенево, Соловьиный пр., 18",
                    Latitude = new decimal(55.604284), 
                    Longitude = new decimal(37.554516),
                    Lang = ConstLang.RU
                },
                    new Apartment()
                {
                    Id = "ap4",
                    Name = "Удальцова 73",
                    Type = ConstVals.Flat,
                    Options = ConstVals.WashingMachine+","+ConstVals.Refrigerator,
                    UserId = "u4",
                    Adress = "Россия, Москва, Проспект Вернадского, ул. Удальцова, 73",
                    Latitude = new decimal(55.677215), 
                    Longitude = new decimal(37.497288),
                    Lang = ConstLang.RU
                },
                    new Apartment()
                {
                    Id = "ap5",
                    Name = "Сумской проезд",
                    Type = ConstVals.Flat,
                    Options = ConstVals.WashingMachine+","+ConstVals.Refrigerator,
                    UserId = "u5",
                    Adress = "Россия, Москва, Чертаново Северное, Сумской проезд, 12к5",
                    Latitude = new decimal(55.63746), 
                    Longitude = new decimal(37.608463),
                    Lang = ConstLang.RU
                },
                   new Apartment()
                {
                    Id = "ap6",
                    Name = "Братеево",
                    Type = ConstVals.Flat,
                    Options = ConstVals.WashingMachine+","+ConstVals.Refrigerator,
                    UserId = "u6",
                    Adress = "Россия, Москва, Братеево, Ключевая улица, 10к2",
                    Latitude = new decimal(55.635426), 
                    Longitude = new decimal(37.757045),
                    Lang = ConstLang.RU
                },
                new Apartment()
                {
                    Id = "ap7",
                    Name = "Замоскворечье",
                    Type = ConstVals.Flat,
                    Options = ConstVals.Concierge + "," + ConstVals.WashingMachine+","+ConstVals.Refrigerator,
                    UserId = "u7",
                    Adress = "Россия, Москва, Замоскворечье, Валовая улица, 10",
                    Latitude = new decimal(55.731272), 
                    Longitude = new decimal(37.631757),
                    Lang = ConstLang.RU
                },
                new Apartment()
                {
                    Id = "ap8",
                    Name = "Замоскворечье около Парка Горького",
                    Type = ConstVals.Flat,
                    Options = ConstVals.Concierge + "," + ConstVals.AirConditioning + "," +ConstVals.WashingMachine+","+ConstVals.Refrigerator,
                    UserId = "u8",
                    Adress = "Россия, Москва, Замоскворечье, 2-й Хвостов переулок, 12",
                    Latitude = new decimal(55.735454), 
                    Longitude = new decimal(37.615003),
                    Lang = ConstLang.RU
                },
                new Apartment()
                {
                    Id = "ap9",
                    Name = "Марфино",
                    Type = ConstVals.Flat,
                    Options =ConstVals.AirConditioning + "," +ConstVals.WashingMachine+","+ConstVals.Refrigerator,
                    UserId = "u9",
                    Adress = "Россия, Москва, Марфино, улица Академика Комарова, 7В",
                    Latitude = new decimal(55.82886), 
                    Longitude = new decimal(37.591422),
                    Lang = ConstLang.RU
                },
                new Apartment()
                {
                    Id = "ap10",
                    Name = "Крылатское",
                    Type = ConstVals.Flat,
                    Options = ConstVals.Parking + "," + ConstVals.Concierge + "," + ConstVals.AirConditioning + "," +ConstVals.WashingMachine+","+ConstVals.Refrigerator,
                    UserId = "u10",
                    Adress = "Россия, Москва, Крылатское, улица Крылатские Холмы, 24к1",
                    Latitude = new decimal(55.762931), 
                    Longitude = new decimal(37.415865),
                    Lang = ConstLang.RU
                },
            };

            foreach (var apartment in apartments)
            {
                context.Set<Apartment>().Add(apartment);
            }
        }

        public static void PopulateCards(apartmenthostContext context)
        {
            List<Card> adverts = new List<Card>()
            {
                new Card()
                {
                    Id = "a1",
                    Name = "Офис совместно с Парус",
                    UserId = "u1",
                    Description = "Бизнес центр ААА-класса. Многоуровневая паркова. Бесплатное питание. У нас есть печеньки!",
                    ApartmentId = "ap1",
                    DateFrom = new DateTime(2015,4,1),
                    DateTo = new DateTime(2015,4,30),
                    PriceDay = 1500,
                    Cohabitation = ConstVals.SeperateResidence,
                    ResidentGender = ConstVals.Any,
                    Lang = ConstLang.RU
                },
                new Card()
                {
                    Id = "a2",
                    Name = "Пупович Плаза в Бутово",
                    UserId = "u2",
                    Description = "Великолепное жилье в центре Бутово. Комфортно и уютно.",
                    ApartmentId = "ap2",
                    DateFrom = new DateTime(2015,5,10),
                    DateTo = new DateTime(2015,6,30),
                    PriceDay = 1000,
                    Cohabitation = ConstVals.Cohabitation,
                    ResidentGender = ConstVals.Female,
                    Lang = ConstLang.RU
                },
                new Card()
                {
                    Id = "a3",
                    Name = "Ленкина квартира в Ясенево",
                    UserId = "u3",
                    Description = "Квартирка на Соловьином. Жить можно.",
                    ApartmentId = "ap3",
                    DateFrom = new DateTime(2015,5,1),
                    DateTo = new DateTime(2015,5,31),
                    PriceDay = 1300,
                    Cohabitation = ConstVals.SeperateResidence,
                    ResidentGender = ConstVals.Male,
                    Lang = ConstLang.RU
                },
                new Card()
                {
                    Id = "a4",
                    Name = "Удальцова 73",
                    UserId = "u4",
                    Description = "Реально классная квартира! Просторная с качественным евро-ремонтом из дорогих материалов. Полностью меблирована. Никогда раньше не сдавалась, все новое! Порядочные соседи (славяне). Все есть для комфортного проживания, шкаф купе в прихожей и в гостиной, диван, компьютерный стол, телевизор, кухонный гарнитур, варочная панель, двухкамерный холодильник. Внимание: лоджия утепленная и площадь квартиры расширилась, идеальное место для кабинета. ",
                    ApartmentId = "ap4",
                    DateFrom = new DateTime(2015,1,1),
                    DateTo = new DateTime(2015,6,15),
                    PriceDay = 1200,
                    Cohabitation = ConstVals.SeperateResidence,
                    ResidentGender = ConstVals.Any,
                    Lang = ConstLang.RU
                },
                new Card()
                {
                    Id = "a5",
                    Name = "Сумской проезд",
                    UserId = "u5",
                    Description = "Обычная кв. с мебелью (дивана пока нет), на длительный срок, для всех семейных.",
                    ApartmentId = "ap5",
                    DateFrom = new DateTime(2015,5,1),
                    DateTo = new DateTime(2015,12,31),
                    PriceDay = 1000,
                    Cohabitation = ConstVals.Cohabitation,
                    ResidentGender = ConstVals.Female,
                    Lang = ConstLang.RU
                },
                new Card()
                {
                    Id = "a6",
                    Name = "Братеево",
                    UserId = "u6",
                    Description = "Новая кровать, чистое постельное белье, вся необходимая бытовая техника в наличии, санузел после ремонта.Без комиссии и залогов. Без подселения.Заселение круглосуточно, 24 часа.",
                    ApartmentId = "ap6",
                    DateFrom = new DateTime(2015,12,1),
                    DateTo = new DateTime(2015,12,31),
                    PriceDay = 860,
                    Cohabitation = ConstVals.SeperateResidence,
                    ResidentGender = ConstVals.Any,
                    Lang = ConstLang.RU
                },
                new Card()
                {
                    Id = "a7",
                    Name = "Замоскворечье",
                    UserId = "u7",
                    Description = "Квартирка на Соловьином. Жить можно, но не долго. Из окна почти ничего не видно, только стенку морга.",
                    ApartmentId = "ap7",
                    DateFrom = new DateTime(2015,9,1),
                    DateTo = new DateTime(2015,9,30),
                    PriceDay = 700,
                    Cohabitation = ConstVals.SeperateResidence,
                    ResidentGender = ConstVals.Any,
                    Lang = ConstLang.RU
                },
                 new Card()
                {
                    Id = "a8",
                    Name = "Замоскворечье около Парка Горького",
                    UserId = "u8",
                    Description = "Хорошая квартира. 5 минут до Парка Горького",
                    ApartmentId = "ap8",
                    DateFrom = new DateTime(2015,6,1),
                    DateTo = new DateTime(2015,8,31),
                    PriceDay = 1800,
                    Cohabitation = ConstVals.SeperateResidence,
                    ResidentGender = ConstVals.Any,
                    Lang = ConstLang.RU
                },
                new Card()
                {
                    Id = "a9",
                    Name = "Марфино",
                    UserId = "u9",
                    Description = "Уютная квартира в пешей доступности. Район с развитой инфраструктурой.",
                    ApartmentId = "ap9",
                    DateFrom = new DateTime(2016,1,15),
                    DateTo = new DateTime(2016,12,31),
                    PriceDay = 1300,
                    Cohabitation = ConstVals.Cohabitation,
                    ResidentGender = ConstVals.Female,
                    Lang = ConstLang.RU
                },
               
                new Card()
                {
                    Id = "a10",
                    Name = "Крылатское",
                    UserId = "u10",
                    Description = "Евро ремонт, вся бытовая техника(телевизор, холодильник, плита ,стиральная машина, микроволновая печь. точка доступа интернет. Предоставляется постельное белье, полотенце, посуда, гель для душа, шампунь. Во дворе парковка. Предоставляем отчетные документы командировочным для бухгалтерии. Звоните, ждем вас!",
                    ApartmentId = "ap10",
                    DateFrom = new DateTime(2015,5,1),
                    DateTo = new DateTime(2015,5,31),
                    PriceDay = 2000,
                    Cohabitation = ConstVals.SeperateResidence,
                    ResidentGender = ConstVals.Any,
                    Lang = ConstLang.RU
                },
                
            };

            foreach (var advert in adverts)
            {
                context.Set<Card>().Add(advert);
            }
        }

        public static void PopulateReservations(apartmenthostContext context)
        {
            List<Reservation> reservations = new List<Reservation>()
            {
                new Reservation()
                {
                    Id = "r12",
                    UserId = "u1",
                    CardId = "a2",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2015,12,1),
                    DateTo = new DateTime(2015,12,25)

                    
                },
                new Reservation()
                {
                    Id = "r110",
                    UserId = "u1",
                    CardId = "a10",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2015,1,1),
                    DateTo = new DateTime(2015,1,25)


                },
                new Reservation()
                {
                    Id = "r23",
                    UserId = "u2",
                    CardId = "a3",
                    Status = ConstVals.Pending,
                    DateFrom = new DateTime(2015,9,1),
                    DateTo = new DateTime(2015,9,6)
                    
                },
                new Reservation()
                {
                    Id = "r31",
                    UserId = "u3",
                    CardId = "a1",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2015,3,1),
                    DateTo = new DateTime(2015,3,6)
                    
                },
                new Reservation()
                {
                    Id = "r51",
                    UserId = "u5",
                    CardId = "a1",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2010,5,10),
                    DateTo = new DateTime(2010,6,30)
                    
                },
                new Reservation()
                {
                    Id = "r91",
                    UserId = "u9",
                    CardId = "a1",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2015,1,10),
                    DateTo = new DateTime(2015,1,30)
                    
                },
                new Reservation()
                {
                    Id = "r101",
                    UserId = "u10",
                    CardId = "a1",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2015,2,1),
                    DateTo = new DateTime(2015,2,20)
                    
                },
                new Reservation()
                {
                    Id = "r45",
                    UserId = "u4",
                    CardId = "a5",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2014,1,1),
                    DateTo = new DateTime(2014,12,31)
                    
                },
                new Reservation()
                {
                    Id = "r52",
                    UserId = "u5",
                    CardId = "a2",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2013,1,1),
                    DateTo = new DateTime(2013,12,31)
                    
                },
                 new Reservation()
                {
                    Id = "r62",
                    UserId = "u6",
                    CardId = "a2",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2016,1,1),
                    DateTo = new DateTime(2016,12,31)
                },
                new Reservation()
                {
                    Id = "r71",
                    UserId = "u7",
                    CardId = "a1",
                    Status = ConstVals.Pending,
                    DateFrom = new DateTime(2016,1,1),
                    DateTo = new DateTime(2016,12,31)
                },
                new Reservation()
                {
                    Id = "r81",
                    UserId = "u8",
                    CardId = "a1",
                    Status = ConstVals.Declined,
                    DateFrom = new DateTime(2016,1,1),
                    DateTo = new DateTime(2016,12,31)
                },
                new Reservation()
                {
                    Id = "r41",
                    UserId = "u4",
                    CardId = "a1",
                    Status = ConstVals.Pending,
                    DateFrom = new DateTime(2015,8,1),
                    DateTo = new DateTime(2015,9,30)
                    
                }
            };

            foreach (var reservation in reservations)
            {
                context.Set<Reservation>().Add(reservation);
            }
        }
        public static void PopulateFavorites(apartmenthostContext context)
        {
            List<Favorite> favorites = new List<Favorite>()
            {
                new Favorite()
                {
                    Id = "f12",
                    UserId = "u1",
                    CardId = "a2"
                    
                },
                new Favorite()
                {
                    Id = "f13",
                    UserId = "u1",
                    CardId = "a3"
                },
                new Favorite()
                {
                    Id = "f23",
                    UserId = "u2",
                    CardId = "a3"
                },
                new Favorite()
                {
                    Id = "f32",
                    UserId = "u3",
                    CardId = "a2"
                    
                },
                new Favorite()
                {
                    Id = "f21",
                    UserId = "u2",
                    CardId = "a1"
                }
            };
            foreach (var favorite in favorites)
            {
                context.Set<Favorite>().Add(favorite);
            }
        }

       
        public static void PopulateReviews(apartmenthostContext context)
        {
            List<Review> reviews = new List<Review>()
            {
                new Review()
                {
                    Id = "rw31",
                    FromUserId = "u3",
                    ToUserId = "u1",
                    ReservationId = "r31",
                    Text = "Отличный офис! Всем довольны! Арендуем еще на год!",
                    Rating = 5
                },
                 new Review()
                {
                    Id = "rw1to10",
                    FromUserId = "u1",
                    ToUserId = "u10",
                    ReservationId = "r110",
                    Text = "Просто отлично!",
                    Rating = 5
                },
                new Review()
                {
                    Id = "rw13",
                    FromUserId = "u1",
                    ToUserId = "u3",
                    ReservationId = "r31",
                    Text = "Ответственный съемщик. Вежливый и аккуратный. Оплата в срок.",
                    Rating = 5
                },
                 new Review()
                {
                    Id = "rw14",
                    FromUserId = "u1",
                    ToUserId = "u4",
                    ReservationId = "r41",
                    Text = "Съемщик хороший, но куда-то пропал и отзыв не оставил, хоть и обещал",
                    Rating = 4
                },
                    new Review()
                {
                    Id = "rw91",
                    FromUserId = "u9",
                    ToUserId = "u1",
                    ReservationId = "r91",
                    Text = "Все хорошо",
                    Rating = 4
                },
                new Review()
                {
                    Id = "rw101",
                    FromUserId = "u10",
                    ToUserId = "u1",
                    ReservationId = "r101",
                    Text = "Норм",
                    Rating = 3
                },
            
                new Review()
                {
                    Id = "rw110",
                    FromUserId = "u1",
                    ToUserId = "u10",
                    ReservationId = "r101",
                    Text = "Плохой съемщик, куча проблем",
                    Rating = 3
                },
                new Review()
                {
                    Id = "rw45",
                    FromUserId = "u4",
                    ToUserId = "u5",
                    ReservationId = "r45",
                    Text = "Очень понравилось. Может быть еще приедем.",
                    Rating = 5
                }
            };
            foreach (var review in reviews)
            {
                context.Set<Review>().Add(review);
            }
        }

        public static void PopulateNotifications(apartmenthostContext context)
        {
            List<Notification> notifications = new List<Notification>()
            {
                new Notification()
                {
                    Id = "nreserv1from7",
                    UserId = "u1",
                    Type = ConstVals.General,
                    CardId = "a1",
                    ReservationId = "r71",
                    Code = RespH.SRV_NOTIF_RESERV_PENDING,
                    Readed = false
                },
                new Notification()
                {
                    Id = "nreserv1from4",
                    UserId = "u1",
                    Type = ConstVals.General,
                    CardId = "a1",
                    ReservationId = "r41",
                    Code = RespH.SRV_NOTIF_RESERV_PENDING,
                    Readed = false
                },
                new Notification()
                {
                    Id = "naccept2from1",
                    UserId = "u1",
                    Type = ConstVals.General,
                    ReservationId = "r12",
                    Code = RespH.SRV_NOTIF_RESERV_ACCEPTED,
                    Readed = false
                },
                new Notification()
                {
                    Id = "naccept10from1",
                    UserId = "u1",
                    Type = ConstVals.General,
                    ReservationId = "r110",
                    Code = RespH.SRV_NOTIF_RESERV_ACCEPTED,
                    Readed = false
                },
                new Notification()
                {
                     Id = "nfav1from2",
                        UserId = "u1",
                        Type = ConstVals.General,
                        FavoriteId = "f21",
                        Code = RespH.SRV_NOTIF_CARD_FAVORITED,
                        Readed = false
                },
                new Notification()
                {
                    Id = "nrev1from3",
                    UserId ="u1",
                    Type = ConstVals.General,
                    ReviewId = "rw31",
                    Code = RespH.SRV_NOTIF_REVIEW_RATING_ADDED,
                    Readed = false
                }
            };

            foreach (var notifcation in notifications)
            {
                context.Set<Notification>().Add(notifcation);
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
            Dictionary apartmentTypeDic = context.Dictionaries.SingleOrDefault(a => a.Name == ConstDictionary.ApartmentType);
            foreach (var apartmentType in ConstDicVals.ApartmentTypesList())
            {
                dictionaryItems.Add(new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = apartmentTypeDic.Id,
                    StrValue = apartmentType
                });
            }
            //CohabitationType
            Dictionary cohabitationTypeDic = context.Dictionaries.SingleOrDefault(a => a.Name == ConstDictionary.Cohabitation);
            foreach (var cohabitationType in ConstDicVals.CohabitationTypesList())
            {
                dictionaryItems.Add(new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = cohabitationTypeDic.Id,
                    StrValue = cohabitationType
                });
            }

            //ApartmentOptions
            Dictionary apartmentOptionDic = context.Dictionaries.SingleOrDefault(a => a.Name == ConstDictionary.ApartmentOptions);
            foreach (var apartmentOption in ConstDicVals.ApartmentOptionsList())
            {
                dictionaryItems.Add(new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = apartmentOptionDic.Id,
                    StrValue = apartmentOption
                });
            }
            foreach (var dictonaryItem in dictionaryItems)
            {
                context.Set<DictionaryItem>().Add(dictonaryItem);
            }

            //Gender
            Dictionary genderDic = context.Dictionaries.SingleOrDefault(a => a.Name == ConstDictionary.Gender);
            foreach (var gender in ConstDicVals.GenderList())
            {
                dictionaryItems.Add(new DictionaryItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    DictionaryId = genderDic.Id,
                    StrValue = gender
                });
            }
            foreach (var dictonaryItem in dictionaryItems)
            {
                context.Set<DictionaryItem>().Add(dictonaryItem);
            }
            
        }
       

       
    }
}
