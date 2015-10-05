using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using CloudinaryDotNet;

namespace apartmenthostService.Migrations
{
    public class TestDBPopulator
    {
        public static void Populate(apartmenthostContext context)
        {
            try
            {
                PopulateArticles(context);
                context.SaveChanges();

                //PopulateUsers(context);
                //context.SaveChanges();

                PopulateProfiles(context);
                context.SaveChanges();

                PopulateProfilePic(context);
                context.SaveChanges();

                PopulateApartments(context);
                context.SaveChanges();

                PopulateApartmentPics(context);
                context.SaveChanges();

                PopulateCards(context);
                context.SaveChanges();

                PopulateCardDates(context);
                context.SaveChanges();

                PopulateCardGenders(context);
                context.SaveChanges();

                PopulateFavorites(context);
                context.SaveChanges();

                PopulateReservations(context);
                context.SaveChanges();

                PopulateReviews(context);
                context.SaveChanges();

                UpdateRating(context);
                context.SaveChanges();

                PopulateNotifications(context);
                context.SaveChanges();

                //RatingJob ratingJob = new RatingJob();
                //ratingJob.ExecuteAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("!!!!!!!!!!!!!!!!!!");
                Debug.WriteLine(e.InnerException);
                Debug.WriteLine("!!!!!!!!!!!!!!!!!!");
                Debug.WriteLine(e);
            }
        }

        public static void PopulateArticles(apartmenthostContext context)
        {
            var articles = new List<Article>
            {
                new Article
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ConstVals.Greet,
                    Text = "Здравствуйте, <b>{0}</b>.",
                    Type = ConstVals.EmailTemp,
                    Lang = ConstLang.RU
                },
                new Article
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = RespH.SRV_NOTIF_CARD_FAVORITED,
                    Title = "Apartmenthost - Ваше объявление добавили в избранное",
                    Text = "Пользователь <b>{0}</b> добавил <a href=\"{1}\"> Ваше объявление </a> в избранное.",
                    Type = ConstVals.EmailTemp,
                    Lang = ConstLang.RU
                },
                new Article
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = RespH.SRV_NOTIF_RESERV_PENDING,
                    Title = "Apartmenthost - Заявка на бронирование",
                    Text =
                        "Ваша заявку на бронирование <b>{0}</b> в период с <b>{1}</b> по <b>{2}</b> получена. <br><br> Ожидайте подтверждения от владельца.",
                    Type = ConstVals.EmailTemp,
                    Lang = ConstLang.RU
                },
                new Article
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = RespH.SRV_NOTIF_RESERV_ACCEPTED,
                    Title = "Apartmenthost - Подтверждение бронирования",
                    Text =
                        "{0} подтвердил вашу заявку на бронирование <b>{0}</b> в период с <b>{1}</b> по <b>{2}</b>. <br><br> Свяжитесь с владельцем для получения дополнительной информации.",
                    Type = ConstVals.EmailTemp,
                    Lang = ConstLang.RU
                },
                new Article
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = RespH.SRV_NOTIF_RESERV_DECLINED,
                    Title = "Apartmenthost - Бронирование отклонено",
                    Text =
                        "К сожалению ваше бронирование <b>{0}</b> в период с <b>{1}</b> по <b>{2}</b> отклонено владельцем. <br><br> Свяжитесь с владельцем для получения дополнительной информации.",
                    Type = ConstVals.EmailTemp,
                    Lang = ConstLang.RU
                },
                new Article
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = RespH.SRV_NOTIF_REVIEW_ADDED,
                    Title = "Apartmenthost - Вам оставили отзыв",
                    Text = "Пользователь <b>{0}</b> оставил отзыв <b>{1}</b>.",
                    Type = ConstVals.EmailTemp,
                    Lang = ConstLang.RU
                },
                new Article
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = RespH.SRV_NOTIF_REVIEW_RATING_ADDED,
                    Title = "Apartmenthost - Вам оставили отзыв",
                    Text = "Пользователь <b>{0}</b> оставил отзыв <b>{1}</b> и оценил вас в <b>{2}</b> из 5.",
                    Type = ConstVals.EmailTemp,
                    Lang = ConstLang.RU
                },
                new Article
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = RespH.SRV_NOTIF_REVIEW_AVAILABLE,
                    Title = "Apartmenthost - Вы можете оставить отзыв",
                    Text = "По бронированию <b>{0}</b> в период с <b>{1}</b> по <b>{2}</b> вы можете оставить отзыв.",
                    Type = ConstVals.EmailTemp,
                    Lang = ConstLang.RU
                },
                new Article
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ConstVals.Reg,
                    Title = "Apartmenthost - Подтверждение Email",
                    Text =
                        "Спасибо за регистрацию на Apartmenthost! <br> Для подтверждения Email используйте следующий код: <b>{0}</b> <br> или перейдите по ссылке <b>{1}</b>",
                    Type = ConstVals.EmailTemp,
                    Lang = ConstLang.RU
                },
                new Article
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ConstVals.Restore,
                    Title = "Apartmenthost - Восстановление пароля",
                    Text =
                        "Для восстановления пароля используйте следующий код: <b>{0}</b> <br> или перейдите по ссылке <b>{1}</b>",
                    Type = ConstVals.EmailTemp,
                    Lang = ConstLang.RU
                }
            };

            foreach (var art in articles)
            {
                var ex = context.Article.FirstOrDefault(x => x.Name == art.Name);
                if (ex != null)
                {
                    art.CreatedAt = ex.CreatedAt;
                }

                context.Article.AddOrUpdate(p => p.Name, art
                    );

                context.SaveChanges();
            }
        }

        public static void PopulateUsers(apartmenthostContext context)
        {
            var salt = AuthUtils.generateSalt();

            var users = new List<User>
            {
                new User
                {
                    Id = "u1",
                    Email = "parus@parus.ru",
                    Salt = salt,
                    SaltedAndHashedPassword = AuthUtils.hash("parusina", salt)
                },
                new User
                {
                    Id = "u2",
                    Email = "user2@example.com",
                    Salt = salt,
                    SaltedAndHashedPassword = AuthUtils.hash("user2", salt)
                },
                new User
                {
                    Id = "u3",
                    Email = "user3@example.com",
                    Salt = salt,
                    SaltedAndHashedPassword = AuthUtils.hash("user3", salt)
                },
                new User
                {
                    Id = "u4",
                    Email = "user4@example.com",
                    Salt = salt,
                    SaltedAndHashedPassword = AuthUtils.hash("user4", salt)
                },
                new User
                {
                    Id = "u5",
                    Email = "user5@example.com",
                    Salt = salt,
                    SaltedAndHashedPassword = AuthUtils.hash("user5", salt)
                },
                new User
                {
                    Id = "u6",
                    Email = "user6@example.com",
                    Salt = salt,
                    SaltedAndHashedPassword = AuthUtils.hash("user6", salt)
                },
                new User
                {
                    Id = "u7",
                    Email = "user7@example.com",
                    Salt = salt,
                    SaltedAndHashedPassword = AuthUtils.hash("user7", salt)
                },
                new User
                {
                    Id = "u8",
                    Email = "user8@example.com",
                    Salt = salt,
                    SaltedAndHashedPassword = AuthUtils.hash("user8", salt)
                },
                new User
                {
                    Id = "u9",
                    Email = "user9@example.com",
                    Salt = salt,
                    SaltedAndHashedPassword = AuthUtils.hash("user9", salt)
                },
                new User
                {
                    Id = "u10",
                    Email = "user10@example.com",
                    Salt = salt,
                    SaltedAndHashedPassword = AuthUtils.hash("user10", salt)
                },
                new User
                {
                    Id = "u11",
                    Email = "user11@example.com",
                    Salt = salt,
                    SaltedAndHashedPassword = AuthUtils.hash("user11", salt)
                },
                new User
                {
                    Id = "u12",
                    Email = "user12@example.com",
                    Salt = salt,
                    SaltedAndHashedPassword = AuthUtils.hash("user12", salt)
                }
            };

            foreach (var user in users)
            {
                var ex = context.Users.FirstOrDefault(x => x.Id == user.Id);
                if (ex != null)
                {
                    user.CreatedAt = ex.CreatedAt;
                }
                context.Users.AddOrUpdate(p => p.Id, user);
                context.SaveChanges();
            }


            AuthUtils.CreateAccount("standart", "parus@parus.ru", "standart:parus@parus.ru", "parus@parus.ru");
        }

        public static void PopulateProfiles(apartmenthostContext context)
        {
            var profiles = new List<Profile>
            {
                new Profile
                {
                    Id = "u1",
                    FirstName = "Яна",
                    LastName = "Парусова",
                    Birthday = new DateTime(1989, 1, 1),
                    ContactEmail = "apartmenthost@inbox.ru",
                    ContactKind = "Phone",
                    Description = "Информационные Системы Управления",
                    Gender = ConstVals.PFemale,
                    Phone = "+74957777777",
                    Rating = 0,
                    RatingCount = 0,
                    Score = 0,
                    Lang = ConstLang.RU
                },
                new Profile
                {
                    Id = "u2",
                    FirstName = "Василий",
                    LastName = "Пупович",
                    Birthday = new DateTime(1976, 3, 23),
                    ContactEmail = "apartmenthost@inbox.ru",
                    ContactKind = "Email",
                    Description = "Пуповичи 100 лет на рынке недвижимости!",
                    Gender = ConstVals.PMale,
                    Phone = "+79998887766",
                    Rating = 0,
                    RatingCount = 0,
                    Score = 0,
                    Lang = ConstLang.RU
                },
                new Profile
                {
                    Id = "u3",
                    FirstName = "Елена",
                    LastName = "Пыжович",
                    Birthday = new DateTime(1976, 3, 23),
                    ContactEmail = "apartmenthost@inbox.ru",
                    ContactKind = "Email",
                    Description = "Привет. Меня зовут Лена!",
                    Gender = ConstVals.PFemale,
                    Phone = "+79998987766",
                    Rating = 0,
                    RatingCount = 0,
                    Score = 0,
                    Lang = ConstLang.RU
                },
                new Profile
                {
                    Id = "u4",
                    FirstName = "Дмитрий",
                    LastName = "Трофимов",
                    Birthday = new DateTime(1965, 7, 12),
                    ContactEmail = "apartmenthost@inbox.ru",
                    ContactKind = "Email",
                    Description = "Трофимов",
                    Gender = ConstVals.PMale,
                    Phone = "+79995487766",
                    Rating = 0,
                    RatingCount = 0,
                    Score = 0,
                    Lang = ConstLang.RU
                },
                new Profile
                {
                    Id = "u5",
                    FirstName = "Эдуард",
                    LastName = "Вишняков",
                    Birthday = new DateTime(1986, 1, 18),
                    ContactEmail = "apartmenthost@inbox.ru",
                    ContactKind = "Email",
                    Description = "Трофимов",
                    Gender = ConstVals.PMale,
                    Phone = "+78795487766",
                    Rating = 0,
                    RatingCount = 0,
                    Score = 0,
                    Lang = ConstLang.RU
                },
                new Profile
                {
                    Id = "u6",
                    FirstName = "Леонид",
                    LastName = "Нефедов",
                    Birthday = new DateTime(1977, 12, 23),
                    ContactEmail = "apartmenthost@inbox.ru",
                    ContactKind = "Phone",
                    Description = "Вишняков",
                    Gender = ConstVals.PMale,
                    Phone = "+78795487366",
                    Rating = 0,
                    RatingCount = 0,
                    Score = 0,
                    Lang = ConstLang.RU
                },
                new Profile
                {
                    Id = "u7",
                    FirstName = "Дарья",
                    LastName = "Мамонтова",
                    Birthday = new DateTime(1995, 8, 19),
                    ContactEmail = "apartmenthost@inbox.ru",
                    ContactKind = "Email",
                    Description = "Привет. Меня зовут Дарья!",
                    Gender = ConstVals.PFemale,
                    Phone = "+79998988966",
                    Rating = 0,
                    RatingCount = 0,
                    Score = 0,
                    Lang = ConstLang.RU
                },
                new Profile
                {
                    Id = "u8",
                    FirstName = "Светлана",
                    LastName = "Стрелкова",
                    Birthday = new DateTime(1989, 9, 9),
                    ContactEmail = "apartmenthost@inbox.ru",
                    ContactKind = "Email",
                    Description = "Привет. Меня зовут Светлана!",
                    Gender = ConstVals.PFemale,
                    Phone = "+79994988966",
                    Rating = 0,
                    RatingCount = 0,
                    Score = 0,
                    Lang = ConstLang.RU
                },
                new Profile
                {
                    Id = "u9",
                    FirstName = "Даша",
                    LastName = "Демидова",
                    Birthday = new DateTime(1981, 4, 29),
                    ContactEmail = "apartmenthost@inbox.ru",
                    ContactKind = "Email",
                    Description = "Привет. Меня зовут Светлана!",
                    Gender = ConstVals.PFemale,
                    Phone = "+79994988996",
                    Rating = 0,
                    RatingCount = 0,
                    Score = 0,
                    Lang = ConstLang.RU
                },
                new Profile
                {
                    Id = "u10",
                    FirstName = "Лариса",
                    LastName = "Крокодилова",
                    Birthday = new DateTime(1993, 4, 10),
                    ContactEmail = "apartmenthost@inbox.ru",
                    ContactKind = "Email",
                    Description = "Крокодилова клац-клац",
                    Gender = ConstVals.PFemale,
                    Phone = "+79994938996",
                    Rating = 0,
                    RatingCount = 0,
                    Score = 0,
                    Lang = ConstLang.RU
                },
                new Profile
                {
                    Id = "u11",
                    FirstName = "Лера",
                    LastName = "Бундельера",
                    Birthday = new DateTime(1988, 4, 10),
                    ContactEmail = "apartmenthost@inbox.ru",
                    ContactKind = "Email",
                    Description = "Бундельер цап-цап",
                    Gender = ConstVals.PFemale,
                    Phone = "+79994938996",
                    Rating = 0,
                    RatingCount = 0,
                    Score = 0,
                    Lang = ConstLang.RU
                },
                new Profile
                {
                    Id = "u12",
                    FirstName = "Владмир",
                    LastName = "Путкин",
                    Birthday = new DateTime(1991, 4, 10),
                    ContactEmail = "apartmenthost@inbox.ru",
                    ContactKind = "Email",
                    Description = "не путать с ВВП",
                    Gender = ConstVals.PMale,
                    Phone = "+79994938996",
                    Rating = 0,
                    RatingCount = 0,
                    Score = 0,
                    Lang = ConstLang.RU
                }
            };

            foreach (var profile in profiles)
            {
                var ex = context.Profile.FirstOrDefault(x => x.Id == profile.Id);
                if (ex != null)
                {
                    profile.CreatedAt = ex.CreatedAt;
                }
                context.Profile.AddOrUpdate(p => p.Id,
                    profile);


                context.SaveChanges();
            }
        }

        public static void PopulateProfilePic(apartmenthostContext context)
        {
            for (var i = 1; i < 13; i++)
            {
                var prof = context.Profile.SingleOrDefault(x => x.Id == "u" + i);

                var pic = new Picture
                {
                    Id = "p" + i,
                    Name = "profile/u" + i + ".jpg",
                    Url = CloudinaryHelper.Cloudinary.Api.UrlImgUp.BuildUrl("profile/u" + i + ".jpg"),
                    Small =
                        CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                            new Transformation().Width(34).Height(34).Crop("thumb")).BuildUrl("profile/u" + i + ".jpg"),
                    Mid =
                        CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                            new Transformation().Width(62).Height(62).Crop("thumb")).BuildUrl("profile/u" + i + ".jpg"),
                    Large =
                        CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                            new Transformation().Width(76).Height(76).Crop("thumb")).BuildUrl("profile/u" + i + ".jpg"),
                    Xlarge =
                        CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                            new Transformation().Width(96).Height(96).Crop("thumb")).BuildUrl("profile/u" + i + ".jpg"),
                    CloudinaryPublicId = "profile/u" + i,
                    Default = true
                };

                var ex = context.Pictures.FirstOrDefault(x => x.Id == pic.Id);

                if (ex != null)
                {
                    context.Pictures.Remove(ex);
                    context.SaveChanges();
                }


                prof.Picture = pic;
                context.SaveChanges();
            }
        }

        public static void PopulateApartments(apartmenthostContext context)
        {
            var apartments = new List<Apartment>
            {
                new Apartment
                {
                    Id = "ap1",
                    Name = "Офис Парус",
                    Type = ConstVals.Office,
                    Options = ConstVals.Parking + "," + ConstVals.Concierge + "," + ConstVals.AirConditioning,
                    UserId = "u1",
                    Adress = "Ярославская ул., 10к4",
                    FormattedAdress = "Ярославская ул., 10 корпус 4, Москва, Россия, 129366",
                    AdressTypes = "street_address",
                    Latitude = 55.8192216,
                    Longitude = 37.6499904,
                    PlaceId = "ChIJ5-Lsj9k1tUYR5sJA7m6S7gw",
                    Lang = ConstLang.RU
                },
                new Apartment
                {
                    Id = "ap2",
                    Name = "Пупович Плаза",
                    Type = ConstVals.House,
                    Options =
                        ConstVals.Parking + "," + ConstVals.Concierge + "," + ConstVals.AirConditioning + "," +
                        ConstVals.WashingMachine + "," + ConstVals.Refrigerator,
                    UserId = "u2",
                    Adress = "1-я Горловская ул., 4с21",
                    FormattedAdress = "1-я Горловская ул., 4 строение 21, Москва, Россия, 117623",
                    AdressTypes = "street_address",
                    Latitude = 55.548317,
                    Longitude = 37.581559,
                    PlaceId = "ChIJA0yVwNKtSkERnMBTTfhj-k8",
                    Lang = ConstLang.RU
                },
                new Apartment
                {
                    Id = "ap3",
                    Name = "Ленкина квартирка в Ясенево",
                    Type = ConstVals.Flat,
                    Options = ConstVals.WashingMachine + "," + ConstVals.Refrigerator,
                    UserId = "u3",
                    Adress = "Соловьиный пр-д, 18",
                    FormattedAdress = "Соловьиный пр., 18, Москва, Россия, 117593",
                    AdressTypes = "street_address",
                    Latitude = 55.6048635,
                    Longitude = 37.5560459999999,
                    PlaceId = "ChIJKdkyK2KtSkERVV3xLtXg7VE",
                    Lang = ConstLang.RU
                },
                new Apartment
                {
                    Id = "ap4",
                    Name = "Удальцова 73",
                    Type = ConstVals.Flat,
                    Options = ConstVals.WashingMachine + "," + ConstVals.Refrigerator,
                    UserId = "u4",
                    Adress = "ул. Удальцова, 73",
                    FormattedAdress = "ул. Удальцова, 73, Москва, Россия, 119454",
                    AdressTypes = "street_address",
                    Latitude = 55.677495,
                    Longitude = 37.49704,
                    PlaceId = "ChIJ1Y6qnqtNtUYR5wu_G5QNWOw",
                    Lang = ConstLang.RU
                },
                new Apartment
                {
                    Id = "ap5",
                    Name = "Сумской проезд",
                    Type = ConstVals.Flat,
                    Options = ConstVals.WashingMachine + "," + ConstVals.Refrigerator,
                    UserId = "u5",
                    Adress = "Сумской пр-д, 12к5",
                    FormattedAdress = "Сумской пр., 12 корпус 5, Москва, Россия, 117208",
                    AdressTypes = "street_address",
                    Latitude = 55.63736600000001,
                    Longitude = 37.6083519,
                    PlaceId = "ChIJA7icfOGySkERrX5lm9uDVAY",
                    Lang = ConstLang.RU
                },
                new Apartment
                {
                    Id = "ap6",
                    Name = "Братеево",
                    Type = ConstVals.Flat,
                    Options = ConstVals.WashingMachine + "," + ConstVals.Refrigerator,
                    UserId = "u6",
                    Adress = "Ключевая ул., 10к2",
                    FormattedAdress = "Ключевая ул., 10 корпус 2, Москва, Россия, 115612",
                    AdressTypes = "street_address",
                    Latitude = 55.635362,
                    Longitude = 37.756657,
                    PlaceId = "ChIJQd6MRlSxSkERMgb5At9ic4g",
                    Lang = ConstLang.RU
                },
                new Apartment
                {
                    Id = "ap7",
                    Name = "Замоскворечье",
                    Type = ConstVals.Flat,
                    Options = ConstVals.Concierge + "," + ConstVals.WashingMachine + "," + ConstVals.Refrigerator,
                    UserId = "u7",
                    Adress = "Валовая ул., 10",
                    FormattedAdress = "Валовая ул., 10, Москва, Россия, 115054",
                    AdressTypes = "street_address",
                    Latitude = 55.731279,
                    Longitude = 37.631815,
                    PlaceId = "ChIJ4WVEbRxLtUYR__0S1BVkg_k",
                    Lang = ConstLang.RU
                },
                new Apartment
                {
                    Id = "ap8",
                    Name = "Замоскворечье около Парка Горького",
                    Type = ConstVals.Flat,
                    Options =
                        ConstVals.Concierge + "," + ConstVals.AirConditioning + "," + ConstVals.WashingMachine + "," +
                        ConstVals.Refrigerator,
                    UserId = "u8",
                    Adress = "2-й Хвостов пер., 12",
                    FormattedAdress = "2-й Хвостов пер., 12, Москва, Россия, 119180",
                    AdressTypes = "street_address",
                    Latitude = 55.735488,
                    Longitude = 37.614915,
                    PlaceId = "ChIJGV4apQZLtUYRcsiTOvz4tc0",
                    Lang = ConstLang.RU
                },
                new Apartment
                {
                    Id = "ap9",
                    Name = "Марфино",
                    Type = ConstVals.Flat,
                    Options = ConstVals.AirConditioning + "," + ConstVals.WashingMachine + "," + ConstVals.Refrigerator,
                    UserId = "u9",
                    Adress = "ул. Академика Комарова, 7В",
                    FormattedAdress = "ул. Академика Комарова, 7В, Москва, Россия, 127276",
                    AdressTypes = "street_address",
                    Latitude = 55.828883,
                    Longitude = 37.591417,
                    PlaceId = "ChIJrXf_Wj82tUYRkH6d8hkR4kg",
                    Lang = ConstLang.RU
                },
                new Apartment
                {
                    Id = "ap10",
                    Name = "Крылатское",
                    Type = ConstVals.Flat,
                    Options =
                        ConstVals.Parking + "," + ConstVals.Concierge + "," + ConstVals.AirConditioning + "," +
                        ConstVals.WashingMachine + "," + ConstVals.Refrigerator,
                    UserId = "u10",
                    Adress = "ул. Крылатские Холмы, 24",
                    FormattedAdress = "ул. Крылатские Холмы, 24, Москва, Россия, 121614",
                    AdressTypes = "street_address",
                    Latitude = 55.7628909,
                    Longitude = 37.415231,
                    PlaceId =
                        "Ek3Rg9C7LiDQmtGA0YvQu9Cw0YLRgdC60LjQtSDQpdC-0LvQvNGLLCAyNCwg0JzQvtGB0LrQstCwLCDQoNC-0YHRgdC40Y8sIDEyMTYxNA",
                    Lang = ConstLang.RU
                }
            };

            foreach (var ap in apartments)
            {
                var ex = context.Apartments.FirstOrDefault(x => x.Id == ap.Id);

                if (ex != null)
                {
                    ap.CreatedAt = ex.CreatedAt;
                }
                context.Apartments.AddOrUpdate(p => p.Id, ap);
            }
        }

        public static void PopulateApartmentPics(apartmenthostContext context)
        {
            for (var i = 1; i < 11; i++)
            {
                var apart = context.Apartments.SingleOrDefault(x => x.Id == "ap" + i);
                for (var j = 1; j < 4; j++)
                {
                    var pic = new Picture
                    {
                        Id = "pa" + i + "-" + j,
                        Name = "card/a" + i + "/a" + i + "-" + j + ".jpg",
                        Url =
                            CloudinaryHelper.Cloudinary.Api.UrlImgUp.BuildUrl("card/a" + i + "/a" + i + "-" + j + ".jpg"),
                        Xsmall =
                            CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                                new Transformation().Width(143).Crop("thumb"))
                                .BuildUrl("card/a" + i + "/a" + i + "-" + j + ".jpg"),
                        Small =
                            CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                                new Transformation().Width(190).Crop("thumb"))
                                .BuildUrl("card/a" + i + "/a" + i + "-" + j + ".jpg"),
                        Mid =
                            CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                                new Transformation().Height(225).Width(370).Crop("fill"))
                                .BuildUrl("card/a" + i + "/a" + i + "-" + j + ".jpg"),
                        Large =
                            CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                                new Transformation().Width(552).Crop("limit"))
                                .BuildUrl("card/a" + i + "/a" + i + "-" + j + ".jpg"),
                        Xlarge =
                            CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                                new Transformation().Width(1024).Crop("limit"))
                                .BuildUrl("card/a" + i + "/a" + i + "-" + j + ".jpg"),
                        CloudinaryPublicId = "card/a" + i + "/a" + i + "-" + j,
                        Default = j == 1
                    };
                    var ex = context.Pictures.FirstOrDefault(x => x.Id == pic.Id);

                    if (ex != null)
                    {
                        context.Pictures.Remove(ex);
                        context.SaveChanges();
                    }
                    apart.Pictures.Add(pic);


                    context.SaveChanges();
                }
            }
        }

        public static void PopulateCards(apartmenthostContext context)
        {
            var cards = new List<Card>
            {
                new Card
                {
                    Id = "a1",
                    Name = "Офис совместно с Парус",
                    UserId = "u1",
                    Description =
                        "Бизнес центр ААА-класса. Многоуровневая паркова. Бесплатное питание. У нас есть печеньки!",
                    ApartmentId = "ap1",
                    Cohabitation = ConstVals.SeperateResidence,
                    ResidentGender = ConstVals.Any,
                    Lang = ConstLang.RU
                },
                new Card
                {
                    Id = "a2",
                    Name = "Пупович Плаза в Бутово",
                    UserId = "u2",
                    Description = "Великолепное жилье в центре Бутово. Комфортно и уютно.",
                    ApartmentId = "ap2",
                    Cohabitation = ConstVals.Cohabitation,
                    ResidentGender = ConstVals.Female,
                    Lang = ConstLang.RU
                },
                new Card
                {
                    Id = "a3",
                    Name = "Ленкина квартира в Ясенево",
                    UserId = "u3",
                    Description = "Квартирка на Соловьином. Жить можно.",
                    ApartmentId = "ap3",
                    Cohabitation = ConstVals.SeperateResidence,
                    ResidentGender = ConstVals.Male,
                    Lang = ConstLang.RU
                },
                new Card
                {
                    Id = "a4",
                    Name = "Удальцова 73",
                    UserId = "u4",
                    Description =
                        "Реально классная квартира! Просторная с качественным евро-ремонтом из дорогих материалов. Полностью меблирована. Никогда раньше не сдавалась, все новое! Порядочные соседи (славяне). Все есть для комфортного проживания, шкаф купе в прихожей и в гостиной, диван, компьютерный стол, телевизор, кухонный гарнитур, варочная панель, двухкамерный холодильник. Внимание: лоджия утепленная и площадь квартиры расширилась, идеальное место для кабинета. ",
                    ApartmentId = "ap4",
                    Cohabitation = ConstVals.SeperateResidence,
                    ResidentGender = ConstVals.Any,
                    Lang = ConstLang.RU
                },
                new Card
                {
                    Id = "a5",
                    Name = "Сумской проезд",
                    UserId = "u5",
                    Description = "Обычная кв. с мебелью (дивана пока нет), на длительный срок, для всех семейных.",
                    ApartmentId = "ap5",
                    Cohabitation = ConstVals.Cohabitation,
                    ResidentGender = ConstVals.Female,
                    Lang = ConstLang.RU
                },
                new Card
                {
                    Id = "a6",
                    Name = "Братеево",
                    UserId = "u6",
                    Description =
                        "Новая кровать, чистое постельное белье, вся необходимая бытовая техника в наличии, санузел после ремонта.Без комиссии и залогов. Без подселения.Заселение круглосуточно, 24 часа.",
                    ApartmentId = "ap6",
                    Cohabitation = ConstVals.SeperateResidence,
                    ResidentGender = ConstVals.Any,
                    Lang = ConstLang.RU
                },
                new Card
                {
                    Id = "a7",
                    Name = "Замоскворечье",
                    UserId = "u7",
                    Description =
                        "Квартирка на Соловьином. Жить можно, но не долго. Из окна почти ничего не видно, только стенку морга.",
                    ApartmentId = "ap7",
                    Cohabitation = ConstVals.SeperateResidence,
                    ResidentGender = ConstVals.Any,
                    Lang = ConstLang.RU
                },
                new Card
                {
                    Id = "a8",
                    Name = "Замоскворечье около Парка Горького",
                    UserId = "u8",
                    Description = "Хорошая квартира. 5 минут до Парка Горького",
                    ApartmentId = "ap8",
                    Cohabitation = ConstVals.SeperateResidence,
                    ResidentGender = ConstVals.Any,
                    Lang = ConstLang.RU
                },
                new Card
                {
                    Id = "a9",
                    Name = "Марфино",
                    UserId = "u9",
                    Description = "Уютная квартира в пешей доступности. Район с развитой инфраструктурой.",
                    ApartmentId = "ap9",
                    Cohabitation = ConstVals.Cohabitation,
                    ResidentGender = ConstVals.Female,
                    Lang = ConstLang.RU
                },
                new Card
                {
                    Id = "a10",
                    Name = "Крылатское",
                    UserId = "u10",
                    Description =
                        "Евро ремонт, вся бытовая техника(телевизор, холодильник, плита ,стиральная машина, микроволновая печь. точка доступа интернет. Предоставляется постельное белье, полотенце, посуда, гель для душа, шампунь. Во дворе парковка. Предоставляем отчетные документы командировочным для бухгалтерии. Звоните, ждем вас!",
                    ApartmentId = "ap10",
                    Cohabitation = ConstVals.SeperateResidence,
                    ResidentGender = ConstVals.Any,
                    Lang = ConstLang.RU
                }
            };
            foreach (var card in cards)
            {
                var ex = context.Cards.FirstOrDefault(x => x.Id == card.Id);

                if (ex != null)
                {
                    card.CreatedAt = ex.CreatedAt;
                }

                context.Cards.AddOrUpdate(p => p.Id,
                    card);
            }
        }

        public static void PopulateCardDates(apartmenthostContext context)
        {
            var dateses = new List<CardDates>
            {
                new CardDates
                {
                    Id = "cd1",
                    CardId = "a1",
                    DateFrom = new DateTime(2015, 4, 1),
                    DateTo = new DateTime(2015, 4, 30)
                },
                new CardDates
                {
                    Id = "cd2",
                    CardId = "a2",
                    DateFrom = new DateTime(2015, 5, 10),
                    DateTo = new DateTime(2015, 6, 30)
                },
                new CardDates
                {
                    Id = "cd3",
                    CardId = "a3",
                    DateFrom = new DateTime(2015, 5, 1),
                    DateTo = new DateTime(2015, 5, 31)
                },
                new CardDates
                {
                    Id = "cd4",
                    CardId = "a4",
                    DateFrom = new DateTime(2015, 1, 1),
                    DateTo = new DateTime(2015, 6, 15)
                },
                new CardDates
                {
                    Id = "cd5",
                    CardId = "a5",
                    DateFrom = new DateTime(2015, 5, 1),
                    DateTo = new DateTime(2015, 12, 31)
                },
                new CardDates
                {
                    Id = "cd6",
                    CardId = "a6",
                    DateFrom = new DateTime(2015, 12, 1),
                    DateTo = new DateTime(2015, 12, 31)
                },
                new CardDates
                {
                    Id = "cd7",
                    CardId = "a7",
                    DateFrom = new DateTime(2015, 9, 1),
                    DateTo = new DateTime(2015, 9, 30)
                }
            };
            foreach (var d in dateses)
            {
                var ex = context.Dates.FirstOrDefault(x => x.Id == d.Id);

                if (ex != null)
                {
                    d.CreatedAt = ex.CreatedAt;
                }

                context.Dates.AddOrUpdate(p => p.Id, d
                    );
            }
        }

        public static void PopulateCardGenders(apartmenthostContext context)
        {
            var genders = new List<CardGenders>
            {
                new CardGenders()
                {
                    Id = "cg11",
                    CardId = "a1",
                    Name = ConstVals.Female,
                    Price = 30000
                },
                new CardGenders()
                {
                    Id = "cg12",
                    CardId = "a1",
                    Name = ConstVals.Male,
                    Price = 40000
                },
                new CardGenders()
                {
                    Id = "cg13",
                    CardId = "a1",
                    Name = ConstVals.Alien,
                    Price = 50000
                },
                new CardGenders()
                {
                    Id = "cg21",
                    CardId = "a2",
                    Name = ConstVals.Female,
                    Price = 33000
                },
                new CardGenders()
                {
                    Id = "cg22",
                    CardId = "a2",
                    Name = ConstVals.Male,
                    Price = 20000
                },
                new CardGenders()
                {
                    Id = "cg31",
                    CardId = "a3",
                    Name = ConstVals.Alien,
                    Price = 55000
                },
                new CardGenders()
                {
                    Id = "cg41",
                    CardId = "a4",
                    Name = ConstVals.Female,
                    Price = 13000
                },
                new CardGenders()
                {
                    Id = "cg52",
                    CardId = "a5",
                    Name = ConstVals.Male,
                    Price = 10000
                },
                new CardGenders()
                {
                    Id = "cg61",
                    CardId = "a6",
                    Name = ConstVals.Alien,
                    Price = 15000
                },
                new CardGenders()
                {
                    Id = "cg62",
                    CardId = "a6",
                    Name = ConstVals.Female,
                    Price = 13000
                },
                new CardGenders()
                {
                    Id = "cg72",
                    CardId = "a7",
                    Name = ConstVals.Male,
                    Price = 90000
                },
                new CardGenders()
                {
                    Id = "cg71",
                    CardId = "a7",
                    Name = ConstVals.Alien,
                    Price = 85000
                },
                new CardGenders()
                {
                    Id = "cg81",
                    CardId = "a8",
                    Name = ConstVals.Female,
                    Price = 19000
                },
                new CardGenders()
                {
                    Id = "cg91",
                    CardId = "a9",
                    Name = ConstVals.Male,
                    Price = 20000
                },
                new CardGenders()
                {
                    Id = "cg101",
                    CardId = "a10",
                    Name = ConstVals.Alien,
                    Price = 76000
                }
            };
            foreach (var d in genders)
            {
                var ex = context.Genders.FirstOrDefault(x => x.Id == d.Id);

                if (ex != null)
                {
                    d.CreatedAt = ex.CreatedAt;
                }

                context.Genders.AddOrUpdate(p => p.Id, d
                    );
            }
        }

        public static void PopulateReservations(apartmenthostContext context)
        {
            var reservations = new List<Reservation>
            {
                new Reservation
                {
                    Id = "r12",
                    UserId = "u1",
                    CardId = "a2",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2015, 12, 1),
                    DateTo = new DateTime(2015, 12, 25)
                },
                new Reservation
                {
                    Id = "r110",
                    UserId = "u1",
                    CardId = "a10",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2015, 1, 1),
                    DateTo = new DateTime(2015, 1, 25)
                },
                new Reservation
                {
                    Id = "r23",
                    UserId = "u2",
                    CardId = "a3",
                    Status = ConstVals.Pending,
                    DateFrom = new DateTime(2015, 9, 1),
                    DateTo = new DateTime(2015, 9, 6)
                },
                new Reservation
                {
                    Id = "r31",
                    UserId = "u3",
                    CardId = "a1",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2015, 3, 1),
                    DateTo = new DateTime(2015, 3, 6)
                },
                new Reservation
                {
                    Id = "r51",
                    UserId = "u5",
                    CardId = "a1",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2010, 5, 10),
                    DateTo = new DateTime(2010, 6, 30)
                },
                new Reservation
                {
                    Id = "r91",
                    UserId = "u9",
                    CardId = "a1",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2015, 1, 10),
                    DateTo = new DateTime(2015, 1, 30)
                },
                new Reservation
                {
                    Id = "r101",
                    UserId = "u10",
                    CardId = "a1",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2015, 2, 1),
                    DateTo = new DateTime(2015, 2, 20)
                },
                new Reservation
                {
                    Id = "r45",
                    UserId = "u4",
                    CardId = "a5",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2014, 1, 1),
                    DateTo = new DateTime(2014, 12, 31)
                },
                new Reservation
                {
                    Id = "r52",
                    UserId = "u5",
                    CardId = "a2",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2013, 1, 1),
                    DateTo = new DateTime(2013, 12, 31)
                },
                new Reservation
                {
                    Id = "r62",
                    UserId = "u6",
                    CardId = "a2",
                    Status = ConstVals.Accepted,
                    DateFrom = new DateTime(2016, 1, 1),
                    DateTo = new DateTime(2016, 12, 31)
                },
                new Reservation
                {
                    Id = "r71",
                    UserId = "u7",
                    CardId = "a1",
                    Status = ConstVals.Pending,
                    DateFrom = new DateTime(2016, 1, 1),
                    DateTo = new DateTime(2016, 12, 31)
                },
                new Reservation
                {
                    Id = "r81",
                    UserId = "u8",
                    CardId = "a1",
                    Status = ConstVals.Declined,
                    DateFrom = new DateTime(2016, 1, 1),
                    DateTo = new DateTime(2016, 12, 31)
                },
                new Reservation
                {
                    Id = "r41",
                    UserId = "u4",
                    CardId = "a1",
                    Status = ConstVals.Pending,
                    DateFrom = new DateTime(2015, 8, 1),
                    DateTo = new DateTime(2015, 9, 30)
                }
            };
            foreach (var res in reservations)
            {
                var ex = context.Reservations.FirstOrDefault(x => x.Id == res.Id);

                if (ex != null)
                {
                    res.CreatedAt = ex.CreatedAt;
                }

                context.Reservations.AddOrUpdate(p => p.Id,
                    res);
            }
        }

        public static void PopulateFavorites(apartmenthostContext context)
        {
            var favorites = new List<Favorite>
            {
                new Favorite
                {
                    Id = "f12",
                    UserId = "u1",
                    CardId = "a2"
                },
                new Favorite
                {
                    Id = "f13",
                    UserId = "u1",
                    CardId = "a3"
                },
                new Favorite
                {
                    Id = "f23",
                    UserId = "u2",
                    CardId = "a3"
                },
                new Favorite
                {
                    Id = "f32",
                    UserId = "u3",
                    CardId = "a2"
                },
                new Favorite
                {
                    Id = "f21",
                    UserId = "u2",
                    CardId = "a1"
                }
            };

            foreach (var f in favorites)
            {
                var ex = context.Favorites.FirstOrDefault(x => x.Id == f.Id);

                if (ex != null)
                {
                    f.CreatedAt = ex.CreatedAt;
                }
                context.Favorites.AddOrUpdate(p => p.Id, f
                    );
            }
        }

        public static void PopulateReviews(apartmenthostContext context)
        {
            var reviews = new List<Review>
            {
                new Review
                {
                    Id = "rw31",
                    FromUserId = "u3",
                    ToUserId = "u1",
                    ReservationId = "r31",
                    Text = "Отличный офис! Всем довольны! Арендуем еще на год!",
                    Rating = 5
                },
                new Review
                {
                    Id = "rw1to10",
                    FromUserId = "u1",
                    ToUserId = "u10",
                    ReservationId = "r110",
                    Text = "Просто отлично!",
                    Rating = 5
                },
                new Review
                {
                    Id = "rw13",
                    FromUserId = "u1",
                    ToUserId = "u3",
                    ReservationId = "r31",
                    Text = "Ответственный съемщик. Вежливый и аккуратный. Оплата в срок.",
                    Rating = 5
                },
                new Review
                {
                    Id = "rw14",
                    FromUserId = "u1",
                    ToUserId = "u4",
                    ReservationId = "r41",
                    Text = "Съемщик хороший, но куда-то пропал и отзыв не оставил, хоть и обещал",
                    Rating = 4
                },
                new Review
                {
                    Id = "rw91",
                    FromUserId = "u9",
                    ToUserId = "u1",
                    ReservationId = "r91",
                    Text = "Все хорошо",
                    Rating = 4
                },
                new Review
                {
                    Id = "rw101",
                    FromUserId = "u10",
                    ToUserId = "u1",
                    ReservationId = "r101",
                    Text = "Норм",
                    Rating = 3
                },
                new Review
                {
                    Id = "rw110",
                    FromUserId = "u1",
                    ToUserId = "u10",
                    ReservationId = "r101",
                    Text = "Плохой съемщик, куча проблем",
                    Rating = 3
                },
                new Review
                {
                    Id = "rw45",
                    FromUserId = "u4",
                    ToUserId = "u5",
                    ReservationId = "r45",
                    Text = "Очень понравилось. Может быть еще приедем.",
                    Rating = 5
                }
            };

            foreach (var r in reviews)
            {
                var ex = context.Reviews.FirstOrDefault(x => x.Id == r.Id);

                if (ex != null)
                {
                    r.CreatedAt = ex.CreatedAt;
                }
                context.Reviews.AddOrUpdate(p => p.Id, r
                    );
            }
        }

        public static void UpdateRating(apartmenthostContext context)
        {
            var profiles = context.Profile.ToList();
            foreach (var profile in profiles)
            {
                var reviews = context.Reviews.Where(rev => rev.ToUserId == profile.Id && rev.Rating > 0);
                var count = reviews.Count();
                if (count > 0)
                {
                    profile.RatingCount = count;
                    profile.Rating = reviews.Average(x => x.Rating);
                    profile.Score = reviews.Sum(x => x.Rating);
                }
            }
        }

        public static void PopulateNotifications(apartmenthostContext context)
        {
            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = "nreserv1from7",
                    UserId = "u1",
                    Type = ConstVals.General,
                    CardId = "a1",
                    ReservationId = "r71",
                    Code = RespH.SRV_NOTIF_RESERV_PENDING,
                    Readed = false
                },
                new Notification
                {
                    Id = "nreserv1from4",
                    UserId = "u1",
                    Type = ConstVals.General,
                    CardId = "a1",
                    ReservationId = "r41",
                    Code = RespH.SRV_NOTIF_RESERV_PENDING,
                    Readed = false
                },
                new Notification
                {
                    Id = "naccept2from1",
                    UserId = "u1",
                    Type = ConstVals.General,
                    ReservationId = "r12",
                    Code = RespH.SRV_NOTIF_RESERV_ACCEPTED,
                    Readed = false
                },
                new Notification
                {
                    Id = "naccept10from1",
                    UserId = "u1",
                    Type = ConstVals.General,
                    ReservationId = "r110",
                    Code = RespH.SRV_NOTIF_RESERV_ACCEPTED,
                    Readed = false
                },
                new Notification
                {
                    Id = "nfav1from2",
                    UserId = "u1",
                    Type = ConstVals.General,
                    FavoriteId = "f21",
                    Code = RespH.SRV_NOTIF_CARD_FAVORITED,
                    Readed = false
                },
                new Notification
                {
                    Id = "nrev1from3",
                    UserId = "u1",
                    Type = ConstVals.General,
                    ReviewId = "rw31",
                    Code = RespH.SRV_NOTIF_REVIEW_RATING_ADDED,
                    Readed = false
                }
            };
            foreach (var n in notifications)
            {
                var ex = context.Notifications.FirstOrDefault(x => x.Id == n.Id);

                if (ex != null)
                {
                    n.CreatedAt = ex.CreatedAt;
                }
                context.Notifications.AddOrUpdate(p => p.Id, n
                    );
            }
        }
    }
}