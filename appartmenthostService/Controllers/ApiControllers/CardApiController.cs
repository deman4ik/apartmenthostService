using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using Itenso.TimePeriod;
using LinqKit;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    //[AuthorizeLevel(AuthorizationLevel.Application)]
    public class CardApiController : ApiController
    {
        public ApiServices Services { get; set; }
        private readonly apartmenthostContext _context = new apartmenthostContext();

        /// <summary>
        /// GET api/Cards/
        /// </summary>
        [Route("api/Cards/")]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        [HttpGet]
        public HttpResponseMessage GetCards([FromUri] string cardRequest)
        {
            try
            {

                //var respList = new List<string>();
                //var pre = PredicateBuilder.True<Card>();
                return this.Request.CreateResponse(HttpStatusCode.OK, cardRequest);
                //// Уникальный идентификатор Карточки
                //if (cardRequest.Id != null)
                //    pre = pre.And(x => x.Id == cardRequest.Id);

                //// Наименование Карточки
                //if (cardRequest.Name != null)
                //    pre = pre.And(x => x.Name == cardRequest.Name);
                //// Адрес Жилья
                //if (cardRequest.Adress != null)
                //    pre = pre.And(x => x.Apartment.Adress == cardRequest.Adress);

                //// Уникальный Идентификатор Владельца
                //if (cardRequest.UserId != null)
                //    pre = pre.And(x => x.UserId == cardRequest.UserId);

                //// Описание Карточки
                //if (cardRequest.Description != null)
                //    pre = pre.And(x => x.Description == cardRequest.Description);

                //// Уникальный Идентификатор Жилья
                //if (cardRequest.ApartmentId != null)
                //    pre = pre.And(x => x.ApartmentId == cardRequest.ApartmentId);
          
        //// Тип Жилья
        //        if (cardRequest.Type != null)
        //        { 
        //            var typePre =  PredicateBuilder.False<Card>();

        //            typePre = cardRequest.Type.Aggregate(typePre, (current, type) => current.Or(t => t.Apartment.Type == type));
        //            pre = pre.And(typePre);
        //        }

        //        // Дополнительные опции Жилья
        //          if (cardRequest.Options != null)
        //        { 
        //            pre = pre.And(x => x.Apartment.Options.Contains(cardRequest.Options));
        //        }
        
        //// Дата доступности с по 
        //        if (cardRequest.AvailableDateFrom != null && cardRequest.AvailableDateTo != null)
        //        {
        //            TimeRange availableDates = new TimeRange((DateTime)cardRequest.AvailableDateFrom, (DateTime)cardRequest.AvailableDateTo);
        //            pre = pre.And(
        //                x => x.Dates.ToList().Select(unDate => new TimeRange(unDate.DateFrom, unDate.DateTo, false)).ToList().Any(unavailableDate => !unavailableDate.IntersectsWith(availableDates) )
                        
        //                );
                     
        //        }

        //        var currentUser = User as ServiceUser;
        //        var account = AuthUtils.GetUserAccount(_context, currentUser);
        //        string userId = null;
        //        if (account != null)
        //        {
        //            userId = account.UserId;
        //        }
        //        var result = _context.Cards.Where(pre).Select(x => new CardDTO()
        //        {
        //            Id = x.Id,
        //            Name = x.Name,
        //            UserId = x.UserId,
        //            Description = x.Description,
        //            ApartmentId = x.ApartmentId,
        //            PriceDay = x.PriceDay,
        //            PricePeriod = x.PriceDay * 7,
        //            Cohabitation = x.Cohabitation,
        //            ResidentGender = x.ResidentGender,
        //            IsFavorite = x.Favorites.Any(f => f.UserId == userId),
        //            CreatedAt = x.CreatedAt,
        //            UpdatedAt = x.UpdatedAt,
        //            Lang = x.Lang,
        //            Dates = x.Dates.Select(d => new DatesDTO()
        //            {
        //                DateFrom = d.DateFrom,
        //                DateTo = d.DateTo
        //            }).Union(x.Reservations.Where(r => r.Status == ConstVals.Accepted).Select(rv => new DatesDTO()
        //            {
        //                DateFrom = rv.DateFrom,
        //                DateTo = rv.DateTo
        //            }).ToList()).ToList(),
        //            User = new UserDTO()
        //            {
        //                Id = x.User.Profile.Id,
        //                FirstName = x.User.Profile.FirstName,
        //                LastName = x.User.Profile.LastName,
        //                Gender = x.User.Profile.Gender,
        //                Rating = x.User.Profile.Rating,
        //                RatingCount = x.User.Profile.RatingCount,
        //                Phone = x.User.Profile.Phone,
        //                Picture = new PictureDTO()
        //                {
        //                    Id = x.User.Profile.Picture.Id,
        //                    Name = x.User.Profile.Picture.Name,
        //                    Description = x.User.Profile.Picture.Description,
        //                    Url = x.User.Profile.Picture.Url,
        //                    Default = x.User.Profile.Picture.Default,
        //                    CreatedAt = x.User.Profile.Picture.CreatedAt
        //                }
        //            },
        //            Apartment = new ApartmentDTO()
        //            {
        //                Id = x.Apartment.Id,
        //                Name = x.Apartment.Name,
        //                Type = x.Apartment.Type,
        //                Options = x.Apartment.Options,
        //                UserId = x.Apartment.UserId,
        //                Adress = x.Apartment.Adress,
        //                Latitude = x.Apartment.Latitude,
        //                Longitude = x.Apartment.Longitude,
        //            } 
        //        });
                // Дата доступности по
        
       // // Цена за день с
       // PriceDayFrom 
       // // Цена за день по
       //PriceDayTo 
       // // Цена за период с
       //  PricePeriodFrom 
       // // Цена за период по
       //  PricePeriodTo
       // // Тип проживания
       //  Cohabitation 
       // // Пол проживающего
       //  ResidentGender 
       // // Избранное (Уникальный идентификатор пользователя)
       //IsFavoritedUserId 
       // // Дата добавления с
       //  CreatedAtFrom 
       // // Дата добавления по
       //  CreatedAtTo 


                
               // return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }
        /// <summary>
        /// POST api/Card/48D68C86-6EA6-4C25-AA33-223FC9A27959
        /// </summary>
        [Route("api/Card")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage PostCard(CardDTO card)
        {
            try
            {
                var respList = new List<string>();
                ResponseDTO resp;

                // Check Card is not NULL 
                if (card == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_CARD_NULL));

                // Check CARD Name is not NULL
                resp = CheckHelper.isNull(card.Name, "Name", RespH.SRV_CARD_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);



                // Check CARD Cohabitation is not null
                //resp = CheckHelper.isNull(CARD.Cohabitation, "Cohabitation", RespH.SRV_CARD_REQUIRED);
                //if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);
                if (card.Cohabitation == null) card.Cohabitation = ConstVals.Any;
                // Check CARD Cohabitation Dictionary
                //resp = CheckHelper.isValidDicItem(context, CARD.Cohabitation, ConstDictionary.Cohabitation, "Cohabitation", RespH.SRV_CARD_INVALID_DICITEM);
                //if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check CARD Resident Gender is not null
                //resp = CheckHelper.isNull(card.ResidentGender, "ResidentGender", RespH.SRV_CARD_REQUIRED);
                //if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);



                // Check Current User
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
                var account = AuthUtils.GetUserAccount(_context, currentUser);
                if (account == null)
                {
                    respList.Add(currentUser.Id);
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                // Check CARD not Already Exists
                resp = CheckHelper.isCardExist(_context, account.UserId, RespH.SRV_CARD_EXISTS);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                //Apartment
                if (card.Apartment == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NULL));

                // Check Apartment Adress is not NULL
                resp = CheckHelper.isNull(card.Apartment.Adress, "Adress", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Apartment Type is not NULL
                resp = CheckHelper.isNull(card.Apartment.Type, "Type", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Apartment Type Dictionary
                resp = CheckHelper.isValidDicItem(_context, card.Apartment.Type, ConstDictionary.ApartmentType, "Type", RespH.SRV_APARTMENT_INVALID_DICITEM);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);



                // Get User Profile
                var profile = _context.Profile.SingleOrDefault(x => x.Id == account.UserId);
                if (profile == null)
                {
                    respList.Add(account.UserId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }
                // Update User Phone if it is not defined
                if (string.IsNullOrWhiteSpace(profile.Phone) && !string.IsNullOrWhiteSpace(card.Phone))
                {
                    profile.Phone = card.Phone;
                }
                // Generate 
                string cardGuid = Guid.NewGuid().ToString();
                string apartmentGuid = Guid.NewGuid().ToString();

                List<CardDates> cardDates = new List<CardDates>();
                // Check Dates
                if (card.Dates != null)
                {
                    foreach (var dates in card.Dates)
                    {
                        resp = CheckHelper.isValidDates(dates.DateFrom, dates.DateTo, RespH.SRV_CARD_WRONG_DATE);
                        if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);
                        cardDates.Add(new CardDates()
                        {
                            Id = Guid.NewGuid().ToString(),
                            CardId = cardGuid,
                            DateFrom = dates.DateFrom,
                            DateTo = dates.DateTo
                        });
                    }
                }

                _context.Set<Card>().Add(new Card()
                {
                    Id = cardGuid,
                    Name = card.Name,
                    UserId = account.UserId,
                    Description = card.Description,
                    ApartmentId = apartmentGuid,
                    PriceDay = card.PriceDay,
                    Cohabitation = card.Cohabitation,
                    ResidentGender = card.ResidentGender,
                    Lang = card.Lang,
                    Apartment = new Apartment()
                    {
                        Id = apartmentGuid,
                        Name = card.Name,
                        Type = card.Apartment.Type,
                        Options = card.Apartment.Options,
                        UserId = account.UserId,
                        Adress = card.Apartment.Adress,
                        Latitude = card.Apartment.Latitude,
                        Longitude = card.Apartment.Longitude,
                        Lang = card.Lang

                    }
                });
                _context.Set<CardDates>().AddRange(cardDates);
                _context.SaveChanges();
                respList.Add(cardGuid);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }


        /// <summary>
        /// PUT api/Card/48D68C86-6EA6-4C25-AA33-223FC9A27959
        /// </summary>
        /// <param name="id">The ID of the Card.</param>
        /// <param name="card">The CARD changed object.</param>
        [Route("api/Card/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPut]
        public HttpResponseMessage PutCard(string id, CardDTO card)
        {
            try
            {
                var respList = new List<string>();
                ResponseDTO resp;
                // Check CARD is not NULL 
                if (card == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_CARD_NULL));

                // Check Current CARD is not NULL
                var cardCurrent = _context.Cards.SingleOrDefault(a => a.Id == id);
                if (cardCurrent == null)
                {
                    respList.Add(id);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_CARD_NOTFOUND, respList));
                }

                // Check Current User
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
                var account = AuthUtils.GetUserAccount(_context, currentUser);
                if (account == null)
                {
                    respList.Add(currentUser.Id);
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                // Check CARD User
                if (cardCurrent.UserId != account.UserId)
                {
                    respList.Add(cardCurrent.UserId);
                    respList.Add(account.UserId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_CARD_WRONG_USER, respList));
                }

                // Check CARD Name is not NULL
                resp = CheckHelper.isNull(card.Name, "Name", RespH.SRV_CARD_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check CARD not Already Exists
                resp = CheckHelper.isCardExist(_context, card.Name, RespH.SRV_CARD_EXISTS);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check CARD Cohabitation is not null
                //resp = CheckHelper.isNull(CARD.Cohabitation, "Cohabitation", RespH.SRV_CARD_REQUIRED);
                //if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);
                if (card.Cohabitation == null) card.Cohabitation = ConstVals.Any;
                // Check CARD Cohabitation Dictionary
                //resp = CheckHelper.isValidDicItem(context, CARD.Cohabitation, ConstDictionary.Cohabitation, "Cohabitation", RespH.SRV_CARD_INVALID_DICITEM);
                //if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check CARD Resident Gender is not null
                resp = CheckHelper.isNull(card.ResidentGender, "ResidentGender", RespH.SRV_CARD_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check CARD Resident Gender Dictionary
                //resp = CheckHelper.isValidDicItem(_context, card.ResidentGender, ConstDictionary.Gender, "ResidentGender", RespH.SRV_CARD_INVALID_DICITEM);
                //if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);



                List<CardDates> cardDates = new List<CardDates>();
                // Check Dates
                if (card.Dates != null)
                {
                    foreach (var dates in card.Dates)
                    {
                        resp = CheckHelper.isValidDates(dates.DateFrom, dates.DateTo, RespH.SRV_CARD_WRONG_DATE);
                        if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);
                        cardDates.Add(new CardDates()
                        {
                            Id = Guid.NewGuid().ToString(),
                            CardId = id,
                            DateFrom = dates.DateFrom,
                            DateTo = dates.DateTo
                        });
                    }
                }
                //Apartment
                if (card.Apartment == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NULL));


                var apartment = _context.Apartments.SingleOrDefault(a => a.Id == cardCurrent.ApartmentId);
                if (apartment == null)
                {
                    respList.Add(cardCurrent.ApartmentId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NOTFOUND, respList));
                }

                // Check Apartment Adress is not NULL
                resp = CheckHelper.isNull(card.Apartment.Adress, "Adress", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Apartment Type is not NULL
                resp = CheckHelper.isNull(card.Apartment.Type, "Type", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Apartment Type Dictionary
                resp = CheckHelper.isValidDicItem(_context, card.Apartment.Type, ConstDictionary.ApartmentType, "Type", RespH.SRV_APARTMENT_INVALID_DICITEM);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Delete Card Dates
                var currentDates = _context.Dates.Where(d => d.CardId == id);
                if (currentDates.Any()) 
                {
                    _context.Dates.RemoveRange(currentDates);
                _context.SaveChanges();
                }

                // Update CARD
                cardCurrent.Name = card.Name;
                cardCurrent.Description = card.Description;
                cardCurrent.PriceDay = cardCurrent.PriceDay;
                cardCurrent.Cohabitation = cardCurrent.Cohabitation;
                cardCurrent.ResidentGender = cardCurrent.ResidentGender;

                // Update Apartment
                cardCurrent.Apartment.Name = card.Name;
                cardCurrent.Apartment.Type = apartment.Type;
                cardCurrent.Apartment.Options = apartment.Options;
                cardCurrent.Apartment.Adress = apartment.Adress;
                cardCurrent.Apartment.Latitude = apartment.Latitude;
                cardCurrent.Apartment.Longitude = apartment.Longitude;
                cardCurrent.Apartment.Lang = apartment.Lang;

                _context.SaveChanges();
                _context.Set<CardDates>().AddRange(cardDates);
                _context.SaveChanges();

                respList.Add(id);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }

        /// <summary>
        /// DELETE api/CARD/48D68C86-6EA6-4C25-AA33-223FC9A27959
        /// </summary>
        /// <param name="id">The ID of the Card.</param>
        [Route("api/Card/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpDelete]
        public HttpResponseMessage DeleteCard(string id)
        {
            try
            {
                var respList = new List<string>();
                var card = _context.Cards.SingleOrDefault(a => a.Id == id);

                // Check Card is not NULL
                if (card == null)
                {
                    respList.Add(id);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_CARD_NOTFOUND, respList));
                }

                // Check Current User
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
                var account = AuthUtils.GetUserAccount(_context, currentUser);
                if (account == null)
                {
                    respList.Add(currentUser.Id);
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                // Check CARD User
                if (card.UserId != account.UserId)
                {
                    respList.Add(card.UserId);
                    respList.Add(account.UserId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_CARD_WRONG_USER, respList));
                }

                var apartment = _context.Apartments.SingleOrDefault(a => a.Id == card.ApartmentId);

                // Check Apartment is not NULL
                if (apartment == null)
                {
                    respList.Add(id);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NOTFOUND, respList));
                }
                // Delete Dates
                var cardDates = _context.Dates.Where(x => x.CardId == card.Id);
                _context.Dates.RemoveRange(cardDates);
                _context.SaveChanges();
                // Delete Notifications
                var notifications = _context.Notifications.Where(x => x.CardId == card.Id || x.Reservation.CardId == card.Id || x.Review.Reservation.Card.Id == card.Id || x.Favorite.Card.Id == card.Id);
                _context.Notifications.RemoveRange(notifications);
                _context.SaveChanges();
                // Delete Reviews
                var reviews = _context.Reviews.Where(x => x.Reservation.Card.Id == card.Id);
                _context.Reviews.RemoveRange(reviews);
                _context.SaveChanges();
                // Delete Reservations
                var reservations = _context.Reservations.Where(x => x.CardId == card.Id);
                _context.Reservations.RemoveRange(reservations);
                _context.SaveChanges();
                // Delete Favorites
                var favorites = _context.Favorites.Where(x => x.CardId == card.Id);
                _context.Favorites.RemoveRange(favorites);
                _context.SaveChanges();
                // Delete CARD
                _context.Cards.Remove(card);
                _context.SaveChanges();
                _context.Apartments.Remove(apartment);
                _context.SaveChanges();
                respList.Add(card.Id);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_DELETED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }
    }
}
