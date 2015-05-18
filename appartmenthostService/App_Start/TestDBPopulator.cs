﻿using System;
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

                PopulateFavorites(context);
                context.SaveChanges();

                PopulateReservations(context);
                context.SaveChanges();

                PopulateTables(context);
                context.SaveChanges();

                PopulateDictionaries(context);
                context.SaveChanges();

                PopulateDictionaryItems(context);
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
                                                   Gender = ConstVals.Male,
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
                                                   Gender = ConstVals.Female,
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
                    Type = ConstVals.House,
                    Options = ConstVals.Parking + ";" + ConstVals.Concierge + ";" + ConstVals.AirConditioning + ";" +ConstVals.WashingMachine+";"+ConstVals.Refrigerator,
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
                    Type = ConstVals.Flat,
                    Options = ConstVals.WashingMachine+";"+ConstVals.Refrigerator,
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
                    Type = ConstVals.Office,
                    Options = ConstVals.Parking + ";" + ConstVals.Concierge + ";" + ConstVals.AirConditioning,
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
                    DateFrom = new DateTime(2015,5,1),
                    DateTo = new DateTime(2015,5,31),
                    PriceDay = 968,
                    PricePeriod = 30000,
                    Cohabitation = ConstVals.SeperateResidence,
                    ResidentGender = ConstVals.Male,
                    Lang = ConstLang.RU
                },
                new Advert()
                {
                    Id = "a2",
                    Name = "Пупович Плаза в Бутово",
                    UserId = user1.Id,
                    Description = "Квартирка на Соловьином. Жить можно, но не долго. Из окна почти ничего не видно, только стенку морга.",
                    ApartmentId = apartment2.Id,
                    DateFrom = new DateTime(2015,5,10),
                    DateTo = new DateTime(2015,6,30),
                    PriceDay = 1000,
                    PricePeriod = 50000,
                    Cohabitation = ConstVals.Cohabitation,
                    ResidentGender = ConstVals.Female,
                    Lang = ConstLang.RU
                },
                new Advert()
                {
                    Id = "a3",
                    Name = "Офис совместно с Парус",
                    UserId = user2.Id,
                    Description = "Бизнес центр ААА-класса. Многоуровневая паркова. Бесплатное питание. У нас есть печеньки!",
                    ApartmentId = apartment3.Id,
                    DateFrom = new DateTime(2015,4,30),
                    DateTo = new DateTime(2015,12,31),
                    PriceDay = 1500,
                    PricePeriod = 60000,
                    Cohabitation = ConstVals.SeperateResidence,
                    ResidentGender = ConstVals.Any,
                    Lang = ConstLang.RU
                },
            };

            foreach (var advert in adverts)
            {
                context.Set<Advert>().Add(advert);
            }
        }

        public static void PopulateFavorites(apartmenthostContext context)
        {
            List<Favorite> favorites = new List<Favorite>()
            {
                new Favorite()
                {
                    Id = "f1",
                    UserId = "u1",
                    AdvertId = "a2"
                    
                },
                new Favorite()
                {
                    Id = "f2",
                    UserId = "u2",
                    AdvertId = "a3"
                    
                }
            };
            foreach (var favorite in favorites)
            {
                context.Set<Favorite>().Add(favorite);
            }
        }

        public static void PopulateReservations(apartmenthostContext context)
        {
            List<Reservation> reservations = new List<Reservation>()
            {
                new Reservation()
                {
                    Id = "r1",
                    UserId = "u1",
                    AdvertId = "a2",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2015,12,1),
                    DateTo = new DateTime(2015,12,25)

                    
                },
                new Reservation()
                {
                    Id = "f2",
                    UserId = "u2",
                    AdvertId = "a3",
                    Status = ConstVals.Pending,
                    DateFrom = new DateTime(2015,9,1),
                    DateTo = new DateTime(2015,9,6)
                    
                }
            };
            foreach (var reservation in reservations)
            {
                context.Set<Reservation>().Add(reservation);
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
                    DictionaryId = apartmentOptionDic.Id,
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
