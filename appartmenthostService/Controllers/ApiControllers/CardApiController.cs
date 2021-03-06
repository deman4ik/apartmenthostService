﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using CloudinaryDotNet;
using LinqKit;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class CardApiController : ApiController
    {
        private readonly IApartmenthostContext _context = new ApartmenthostContext();
        public ApiServices Services { get; set; }

        public CardApiController()
        {
        }

        public CardApiController(IApartmenthostContext context)
        {
            _context = context;
        }

        /// <summary>
        ///     GET api/Cards/
        /// </summary>
        [Route("api/Cards/")]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        [HttpGet]
        public HttpResponseMessage GetCards([FromUri] string filter = null)
        {
            try
            {
                CardRequestDTO cardRequest;
                var limit = 100;
                var skip = 0;
                string currGender = null;
                // Создаем предикат
                var pre = PredicateBuilder.True<Card>();
                string id = null;
                var periodDays = 0;
                // Получаем объект из строки запроса
                if (!string.IsNullOrWhiteSpace(filter) && filter != "{}")
                {
                    cardRequest = JsonConvert.DeserializeObject<CardRequestDTO>(filter);

                    if (cardRequest == null)
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            RespH.Create(RespH.SRV_CARD_INVALID_FILTER));

                    pre = pre.And(x => x.Deleted == false);

                    // Лимит записей
                    if (cardRequest.Limit > 0)
                    {
                        limit = cardRequest.Limit;
                    }

                    // Пропуск записей
                    if (cardRequest.Skip > 0)
                    {
                        skip = cardRequest.Skip;
                    }
                    // Уникальный идентификатор Карточки
                    if (cardRequest.Id != null)
                    {
                        pre = pre.And(x => x.Id == cardRequest.Id);
                        id = cardRequest.Id;
                    }


                    // Наименование Карточки
                    if (cardRequest.Name != null)
                        pre = pre.And(x => x.Name == cardRequest.Name);

                    // Уникальный идентификатор Google Places
                    if (cardRequest.PlaceId != null)
                        pre = pre.And(x => x.Apartment.PlaceId == cardRequest.PlaceId);
                    // Поиск по координатам
                    if (cardRequest.SwLat != null && cardRequest.SwLong != null && cardRequest.NeLat != null &&
                        cardRequest.NeLong != null)
                    {
                        pre = pre.And(x => x.Apartment.Latitude >= cardRequest.SwLat);
                        pre = pre.And(x => x.Apartment.Longitude >= cardRequest.SwLong);
                        pre = pre.And(x => x.Apartment.Latitude <= cardRequest.NeLat);
                        pre = pre.And(x => x.Apartment.Longitude <= cardRequest.NeLong);
                    }
                    else
                    {
                        // Адрес Жилья
                        if (cardRequest.Adress != null)
                        {
                            string[] adresStrs = cardRequest.Adress.Split(' ', '.', ',');
                            pre = adresStrs.Aggregate(pre,
                                (current, adresStr) => current.And(x => x.Apartment.FormattedAdress.Contains(adresStr)));
                        }
                    }
                    // Уникальный Идентификатор Владельца
                    if (cardRequest.UserId != null)
                        pre = pre.And(x => x.UserId == cardRequest.UserId);

                    // Описание Карточки
                    if (cardRequest.Description != null)
                        pre = pre.And(x => x.Description == cardRequest.Description);

                    // Уникальный Идентификатор Жилья
                    if (cardRequest.ApartmentId != null)
                        pre = pre.And(x => x.ApartmentId == cardRequest.ApartmentId);

                    // Тип Жилья
                    if (cardRequest.Type != null)
                    {
                        var typePre = PredicateBuilder.False<Card>();

                        typePre = cardRequest.Type.Aggregate(typePre,
                            (current, type) => current.Or(t => t.Apartment.Type == type));
                        pre = pre.And(typePre);
                    }

                    // Дополнительные опции Жилья
                    if (cardRequest.Options != null)
                    {
                        pre = pre.And(x => x.Apartment.Options.Contains(cardRequest.Options));
                    }


                    // Дата доступности с по 
                    // Если заданы обе даты
                    if (cardRequest.AvailableDateFrom != null && cardRequest.AvailableDateTo != null)
                    {
                        var t = (DateTime) cardRequest.AvailableDateTo - (DateTime) cardRequest.AvailableDateFrom;
                        periodDays = (int) t.TotalDays;
                        // Ищем когда жилье доступно
                        pre = pre.And(x => x.Dates.Count(date =>
                            // ||--\\--\\--||
                            (date.DateFrom <= cardRequest.AvailableDateFrom &&
                             date.DateFrom <= cardRequest.AvailableDateTo
                             && date.DateTo >= cardRequest.AvailableDateFrom &&
                             date.DateTo >= cardRequest.AvailableDateTo)
                                // \\--||--||--\\
                            ||
                            (date.DateFrom >= cardRequest.AvailableDateFrom &&
                             date.DateFrom <= cardRequest.AvailableDateTo
                             && date.DateTo >= cardRequest.AvailableDateFrom &&
                             date.DateTo >= cardRequest.AvailableDateTo)
                                // ||--\\--||--\\
                            ||
                            (date.DateFrom <= cardRequest.AvailableDateFrom &&
                             date.DateFrom <= cardRequest.AvailableDateTo
                             && date.DateTo >= cardRequest.AvailableDateFrom &&
                             date.DateTo <= cardRequest.AvailableDateTo)
                                //  \\--||--||--\\
                            ||
                            (date.DateFrom >= cardRequest.AvailableDateFrom &&
                             date.DateFrom <= cardRequest.AvailableDateTo
                             && date.DateTo >= cardRequest.AvailableDateFrom &&
                             date.DateTo <= cardRequest.AvailableDateTo)
                            ) == 0
                            );
                        // И по одобренному бронированию
                        pre = pre.And(x => x.Reservations.Count(reserv =>
                            // ||--\\--\\--||
                            ((reserv.DateFrom <= cardRequest.AvailableDateFrom &&
                              reserv.DateFrom <= cardRequest.AvailableDateTo
                              && reserv.DateTo >= cardRequest.AvailableDateFrom &&
                              reserv.DateTo >= cardRequest.AvailableDateTo)
                                // \\--||--||--\\
                             ||
                             (reserv.DateFrom >= cardRequest.AvailableDateFrom &&
                              reserv.DateFrom <= cardRequest.AvailableDateTo
                              && reserv.DateTo >= cardRequest.AvailableDateFrom &&
                              reserv.DateTo >= cardRequest.AvailableDateTo)
                                // ||--\\--||--\\
                             ||
                             (reserv.DateFrom <= cardRequest.AvailableDateFrom &&
                              reserv.DateFrom <= cardRequest.AvailableDateTo
                              && reserv.DateTo >= cardRequest.AvailableDateFrom &&
                              reserv.DateTo <= cardRequest.AvailableDateTo)
                                //  \\--||--||--\\
                             ||
                             (reserv.DateFrom >= cardRequest.AvailableDateFrom &&
                              reserv.DateFrom <= cardRequest.AvailableDateTo
                              && reserv.DateTo >= cardRequest.AvailableDateFrom &&
                              reserv.DateTo <= cardRequest.AvailableDateTo)) && reserv.Status == ConstVals.Accepted
                            ) == 0
                            );
                    }


                    // Тип проживания
                    if (cardRequest.Cohabitation != null)
                    {
                        var cohPre = PredicateBuilder.False<Card>();

                        cohPre = cardRequest.Cohabitation.Aggregate(cohPre,
                            (current, coh) => current.Or(t => t.Cohabitation.Contains(coh)));
                        pre = pre.And(cohPre);
                    }

                    // Пол проживающего
                    //if (cardRequest.ResidentGender != null)
                    //{
                    //    var genPre = PredicateBuilder.False<Card>();

                    //    genPre = cardRequest.ResidentGender.Aggregate(genPre,
                    //        (current, gen) => current.Or(t => t.ResidentGender.Contains(gen)));
                    //    pre = pre.And(genPre);
                    //}

                    // Пол постояльца
                    if (cardRequest.Genders != null)
                    {
                        currGender = cardRequest.Genders.FirstOrDefault();

                        var genPre = PredicateBuilder.False<Card>();
                        genPre = cardRequest.Genders.Aggregate(genPre,
                            (current, gender) => current.Or(x => x.Genders.Any(g => g.Name == gender)));
                        pre = pre.And(genPre);

                        var pricePre = PredicateBuilder.False<Card>();
                        // Цена за день с (с учетом пола)
                        if (cardRequest.PriceDayFrom != null)
                            pricePre =
                                cardRequest.Genders.Where(gender => cardRequest.PriceDayFrom != null)
                                    .Aggregate(pricePre,
                                        (current, gender) =>
                                            current.Or(
                                                x =>
                                                    x.Genders.Any(
                                                        g =>
                                                            g.Price > 0 && g.Name == gender &&
                                                            g.Price >= cardRequest.PriceDayFrom)));
                        // Цена за день по(с учетом пола)
                        if (cardRequest.PriceDayTo != null)
                            pricePre =
                                cardRequest.Genders.Where(gender => cardRequest.PriceDayTo != null)
                                    .Aggregate(pricePre,
                                        (current, gender) =>
                                            current.Or(
                                                x =>
                                                    x.Genders.Any(
                                                        g =>
                                                            g.Price > 0 && g.Name == gender &&
                                                            g.Price <= cardRequest.PriceDayTo)));

                        if (cardRequest.PriceDayFrom != null || cardRequest.PriceDayTo != null)
                            pre = pre.And(pricePre);
                    }
                    else
                    {
                        // Цена за день с 
                        if (cardRequest.PriceDayFrom != null)
                            pre =
                                pre.And(
                                    x =>
                                        x.Genders.Any(
                                            g =>
                                                g.Price >= cardRequest.PriceDayFrom &&
                                                g.Price == x.Genders.Min(ge => ge.Price)));

                        // Цена за день по
                        if (cardRequest.PriceDayTo != null)
                            pre =
                                pre.And(
                                    x =>
                                        x.Genders.Any(
                                            g =>
                                                g.Price <= cardRequest.PriceDayTo &&
                                                g.Price == x.Genders.Min(ge => ge.Price)));
                    }

                    // Избранное (Уникальный идентификатор пользователя)
                    if (cardRequest.IsFavoritedUserId != null)
                    {
                        pre = pre.And(x => x.Favorites.Count(f => f.UserId == cardRequest.IsFavoritedUserId) == 1);
                    }
                    // Дата добавления с
                    if (cardRequest.CreatedAtFrom != null)
                    {
                        pre = pre.And(x => x.CreatedAt >= cardRequest.CreatedAtFrom);
                    }
                    // Дата добавления по
                    if (cardRequest.CreatedAtTo != null)
                    {
                        pre = pre.And(x => x.CreatedAt <= cardRequest.CreatedAtTo);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_CARD_INVALID_FILTER));
                }

                var currentUser = User as ServiceUser;
                var account = AuthUtils.GetUserAccount(_context, currentUser);
                string userId = null;
                if (account != null)
                {
                    userId = account.UserId;
                }
                var count = _context.Cards.AsExpandable().AsNoTracking().Where(pre).Count();
                IQueryable<CardDTO> cards = _context.Cards.AsExpandable().AsNoTracking()
                    .Where(pre)
                    .OrderByDescending(o => o.User.Profile.Rating).ThenByDescending(o => o.CreatedAt)
                    .Skip(() => skip)
                    .Take(() => limit)
                    .Select(x => new CardDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UserId = x.UserId,
                        Description = x.Description,
                        ApartmentId = x.ApartmentId,
                        PeriodDays = periodDays,
                        Cohabitation = x.Cohabitation,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt,
                        Lang = x.Lang
                    });
                CardResponseDTO result = new CardResponseDTO {Count = count, Cards = new List<CardDTO>()};
                var cardsList = cards.ToList();
                if (cardsList.Any())
                {
                    foreach (var card in cardsList)
                    {
                        card.IsFavorite = false;
                        if (userId != null)
                            card.IsFavorite =
                                _context.Favorites.AsNoTracking().Any(f => f.UserId == userId && f.CardId == card.Id);
                        card.PriceDay =
                            currGender != null
                                ? _context.Genders.FirstOrDefault(g => g.Name == currGender && g.CardId == card.Id)
                                    .Price
                                : _context.Genders.Where(g => g.CardId == card.Id).Min(ge => ge.Price);
                        card.PriceGender =
                            currGender ??
                            _context.Genders.FirstOrDefault(
                                gn =>
                                    gn.Price == _context.Genders.Where(g => g.CardId == card.Id).Min(ge => ge.Price) &&
                                    gn.CardId == card.Id).Name;
                        card.PricePeriod =
                            currGender != null
                                ? _context.Genders.FirstOrDefault(g => g.Name == currGender && g.CardId == card.Id)
                                    .Price*periodDays
                                : _context.Genders.Where(g => g.CardId == card.Id).Min(ge => ge.Price)*periodDays;

                        card.Genders = _context.Genders.Where(x => x.CardId == card.Id).Select(g => new GendersDTO
                        {
                            Name = g.Name,
                            Price = g.Price
                        }).ToList();

                        card.Dates =
                            _context.Dates.AsNoTracking().Where(da => da.CardId == card.Id).Select(d => new DatesDTO
                            {
                                DateFrom = d.DateFrom,
                                DateTo = d.DateTo
                            }).ToList();

                        ICollection<DatesDTO> reservDates =
                            _context.Reservations.AsNoTracking()
                                .Where(r => r.Status == ConstVals.Accepted && r.CardId == card.Id)
                                .Select(rv => new DatesDTO
                                {
                                    DateFrom = rv.DateFrom,
                                    DateTo = rv.DateTo
                                }).ToList();
                        foreach (var resD in reservDates)
                        {
                            card.Dates.Add(resD);
                        }


                        card.User =
                            _context.Profile.AsNoTracking().Where(u => u.Id == card.UserId).Select(x => new UserDTO
                            {
                                Id = x.Id,
                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                Gender = x.Gender,
                                Rating = x.Rating,
                                RatingCount = x.RatingCount,
                                Phone = x.Phone,
                                Description = x.Description,
                                Picture = _context.Pictures.Where(p => p.Id == x.PictureId).Select( pic =>
                                new PictureDTO
                                {
                                    Id = pic.Id,
                                    Name = pic.Name,
                                    Description = pic.Description,
                                    Url = pic.Url,
                                    Xsmall = pic.Xsmall,
                                    Small = pic.Small,
                                    Mid = pic.Mid,
                                    Large = pic.Large,
                                    Xlarge = pic.Xlarge,
                                    Default = pic.Default,
                                    CreatedAt = pic.CreatedAt
                                }).FirstOrDefault()
                            }).FirstOrDefault();

                        card.Apartment =
                            _context.Apartments.AsNoTracking()
                                .Where(a => a.Id == card.ApartmentId)
                                .Select(x => new ApartmentDTO
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Type = x.Type,
                                    Options = x.Options,
                                    UserId = x.UserId,
                                    Adress = x.Adress,
                                    FormattedAdress = x.FormattedAdress,
                                    Latitude = x.Latitude,
                                    Longitude = x.Longitude,
                                    PlaceId = x.PlaceId,
                                    Pictures = _context.Pictures.Where(p => p.Apartments.Contains(x)).Select(apic => new PictureDTO
                                    {
                                        Id = apic.Id,
                                        Name = apic.Name,
                                        Description = apic.Description,
                                        Url = apic.Url,
                                        Xsmall = apic.Xsmall,
                                        Small = apic.Small,
                                        Mid = apic.Mid,
                                        Large = apic.Large,
                                        Xlarge = apic.Xlarge,
                                        Default = apic.Default,
                                        CreatedAt = apic.CreatedAt
                                    }).ToList()
                                }).FirstOrDefault();
                        // Уникальный идентификатор Карточки
                        if (cardRequest.Id != null)
                        {
                            card.Reviews =
                                _context.Reviews.AsNoTracking().Where(
                                    inr => inr.Reservation.CardId == id && id != null && inr.ToUserId == card.UserId)
                                    .Select(rev => new ReviewDTO
                                    {
                                        Id = rev.Id,
                                        Rating = rev.Rating,
                                        Text = rev.Text,
                                        CreatedAt = rev.CreatedAt,
                                        FromUser =
                                            _context.Users.Where(u => u.Id == rev.FromUserId)
                                                .Select(us => new BaseUserDTO
                                                {
                                                    Id = us.Profile.Id,
                                                    Email = us.Email,
                                                    FirstName = us.Profile.FirstName,
                                                    LastName = us.Profile.LastName,
                                                    Rating = us.Profile.Rating,
                                                    RatingCount = us.Profile.RatingCount,
                                                    Gender = us.Profile.Gender,
                                                    Picture =
                                                        _context.Pictures.Where(p => p.Id == us.Profile.PictureId)
                                                            .Select(pic => new PictureDTO
                                                            {
                                                                Id = pic.Id,
                                                                Name = pic.Name,
                                                                Description = pic.Description,
                                                                Url = pic.Url,
                                                                Xsmall = pic.Xsmall,
                                                                Small = pic.Small,
                                                                Mid = pic.Mid,
                                                                Large = pic.Large,
                                                                Xlarge = pic.Xlarge,
                                                                Default = pic.Default,
                                                                CreatedAt = pic.CreatedAt
                                                            }).FirstOrDefault()
                                                }).FirstOrDefault()
                                    }).ToList();
                        }
                        result.Cards.Add(card);
                    }
                }
                //RelatedCards =
                //    _context.Cards.Where(
                //        crd => crd.Id != x.Id && crd.Apartment.Type == x.Apartment.Type && id != null)
                //        .Take(5)
                //        .Select(card => new RelatedCardDTO
                //        {
                //            Id = card.Id,
                //            Name = card.Name,
                //            UserId = card.UserId,
                //            Description = card.Description,
                //            ApartmentId = card.ApartmentId,
                //            PriceDay = card.Genders.Min(ge => ge.Price),
                //            PriceGender =
                //                card.Genders.FirstOrDefault(gn => gn.Price == x.Genders.Min(ge => ge.Price))
                //                    .Name,
                //            PricePeriod = card.Genders.Min(ge => ge.Price)*periodDays,
                //            Cohabitation = card.Cohabitation,
                //            IsFavorite = card.Favorites.Any(f => f.UserId == userId),
                //            CreatedAt = card.CreatedAt,
                //            Lang = card.Lang,
                //            Dates = card.Dates.Select(d => new DatesDTO
                //            {
                //                DateFrom = d.DateFrom,
                //                DateTo = d.DateTo
                //            })
                //                .Union(
                //                    card.Reservations.Where(r => r.Status == ConstVals.Accepted)
                //                        .Select(rv => new DatesDTO
                //                        {
                //                            DateFrom = rv.DateFrom,
                //                            DateTo = rv.DateTo
                //                        }).ToList()).ToList(),
                //            Apartment = new ApartmentDTO
                //            {
                //                Id = card.Apartment.Id,
                //                Name = card.Apartment.Name,
                //                Type = card.Apartment.Type,
                //                Options = card.Apartment.Options,
                //                UserId = card.Apartment.UserId,
                //                Adress = card.Apartment.Adress,
                //                FormattedAdress = card.Apartment.FormattedAdress,
                //                Latitude = card.Apartment.Latitude,
                //                Longitude = card.Apartment.Longitude,
                //                PlaceId = card.Apartment.PlaceId,
                //                Pictures = card.Apartment.Pictures.Select(apic => new PictureDTO
                //                {
                //                    Id = apic.Id,
                //                    Name = apic.Name,
                //                    Description = apic.Description,
                //                    Url = apic.Url,
                //                    Xsmall = apic.Xsmall,
                //                    Small = apic.Small,
                //                    Mid = apic.Mid,
                //                    Large = apic.Large,
                //                    Xlarge = apic.Xlarge,
                //                    Default = apic.Default,
                //                    CreatedAt = apic.CreatedAt
                //                }).ToList()
                //            },
                //            User = new BaseUserDTO
                //            {
                //                Id = card.User.Profile.Id,
                //                Email = card.User.Email,
                //                FirstName = card.User.Profile.FirstName,
                //                LastName = card.User.Profile.LastName,
                //                Rating = card.User.Profile.Rating,
                //                RatingCount = card.User.Profile.RatingCount,
                //                Gender = card.User.Profile.Gender,
                //                Picture = new PictureDTO
                //                {
                //                    Id = card.User.Profile.Picture.Id,
                //                    Name = card.User.Profile.Picture.Name,
                //                    Description = card.User.Profile.Picture.Description,
                //                    Url = card.User.Profile.Picture.Url,
                //                    Xsmall = card.User.Profile.Picture.Xsmall,
                //                    Small = card.User.Profile.Picture.Small,
                //                    Mid = card.User.Profile.Picture.Mid,
                //                    Large = card.User.Profile.Picture.Large,
                //                    Xlarge = card.User.Profile.Picture.Xlarge,
                //                    Default = card.User.Profile.Picture.Default,
                //                    CreatedAt = card.User.Profile.Picture.CreatedAt
                //                }
                //            }
                //        }).ToList()


                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (JsonReaderException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_CARD_INVALID_FILTER, new List<string> {ex.ToString()}));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
            }
        }

        /// <summary>
        ///     POST api/Card/48D68C86-6EA6-4C25-AA33-223FC9A27959
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
                if (card == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_CARD_NULL));

                // Check CARD Name is not NULL
                resp = CheckHelper.IsNull(card.Name, "Name", RespH.SRV_CARD_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                if (card.Cohabitation == null) card.Cohabitation = ConstVals.Any;

                // Check Current User
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
                var account = AuthUtils.GetUserAccount(_context, currentUser);
                if (account == null)
                {
                    respList.Add(currentUser.Id);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                        RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }
                resp = CheckHelper.IsProfileFill(_context, account.UserId, true);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);
                // Check CARD not Already Exists
                resp = CheckHelper.IsCardExist(_context, account.UserId, RespH.SRV_CARD_EXISTS);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                //Apartment
                if (card.Apartment == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NULL));

                // Check Apartment Adress is not NULL
                resp = CheckHelper.IsNull(card.Apartment.Adress, "Adress", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Apartment FormatedAdress is not NULL
                resp = CheckHelper.IsNull(card.Apartment.FormattedAdress, "FormattedAdress",
                    RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                if (card.Apartment.Latitude == null || card.Apartment.Longitude == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.SRV_APARTMENT_WRONG_GEO);

                // Check Apartment Type is not NULL
                resp = CheckHelper.IsNull(card.Apartment.Type, "Type", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);


                // Get User Profile
                var profile = _context.Profile.SingleOrDefault(x => x.Id == account.UserId);
                if (profile == null)
                {
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }
                // Update User Phone if it is not defined
                if (string.IsNullOrWhiteSpace(profile.Phone) && !string.IsNullOrWhiteSpace(card.Phone))
                {
                    profile.Phone = card.Phone;
                }
                // Generate 
                var cardGuid = SequentialGuid.NewGuid().ToString();
                var apartmentGuid = SequentialGuid.NewGuid().ToString();

                var cardDates = new List<CardDates>();
                // Check Dates
                if (card.Dates != null)
                {
                    foreach (var dates in card.Dates)
                    {
                        resp = CheckHelper.IsValidDates(dates.DateFrom, dates.DateTo, RespH.SRV_CARD_WRONG_DATE);
                        if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);
                        cardDates.Add(new CardDates
                        {
                            Id = SequentialGuid.NewGuid().ToString(),
                            CardId = cardGuid,
                            DateFrom = dates.DateFrom,
                            DateTo = dates.DateTo
                        });
                    }
                }

                var cardGenders = new List<CardGenders>();
                if (card.Genders != null)
                {
                    foreach (var gender in card.Genders)
                    {
                        resp = CheckHelper.IsNull(gender.Name, "Genders.Name", RespH.SRV_CARD_REQUIRED);
                        if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);
                        cardGenders.Add(new CardGenders
                        {
                            Id = SequentialGuid.NewGuid().ToString(),
                            CardId = cardGuid,
                            Name = gender.Name,
                            Price = gender.Price
                        });
                    }
                }

                var pics = new List<Picture>();
                // Check Apartment Pictures
                if (card.Apartment.Pictures != null)
                {
                    foreach (var picture in card.Apartment.Pictures)
                    {
                        // Check Picture name is not NULL
                        resp = CheckHelper.IsNull(picture.Name, "Name", RespH.SRV_PICTURE_REQUIRED);
                        if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                        // Check Picture CloudinaryPublicId is not NULL
                        resp = CheckHelper.IsNull(picture.CloudinaryPublicId, "CloudinaryPublicId",
                            RespH.SRV_PICTURE_REQUIRED);
                        if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);
                    }

                    foreach (var picture in card.Apartment.Pictures)
                    {
                        var pictureGuid = SequentialGuid.NewGuid().ToString();
                        var pic = new Picture
                        {
                            Id = pictureGuid,
                            Name = picture.Name,
                            Description = picture.Description,
                            Url = CloudinaryHelper.Cloudinary.Api.UrlImgUp.BuildUrl(picture.Name),
                            Xsmall =
                                CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                                    new Transformation().Width(143).Crop("thumb")).BuildUrl(picture.Name),
                            Small =
                                CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                                    new Transformation().Width(190).Crop("thumb")).BuildUrl(picture.Name),
                            Mid =
                                CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                                    new Transformation().Height(225).Width(370).Crop("fill")).BuildUrl(picture.Name),
                            Large =
                                CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                                    new Transformation().Width(552).Crop("limit")).BuildUrl(picture.Name),
                            Xlarge =
                                CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                                    new Transformation().Width(1024).Crop("limit")).BuildUrl(picture.Name),
                            Default = picture.Default ?? false
                        };
                        pics.Add(pic);
                    }
                }
                _context.Cards.Add(new Card
                {
                    Id = cardGuid,
                    Name = card.Name,
                    UserId = account.UserId,
                    Description = card.Description,
                    ApartmentId = apartmentGuid,
                    Cohabitation = card.Cohabitation,
                    ResidentGender = card.ResidentGender,
                    Lang = card.Lang,
                    Apartment = new Apartment
                    {
                        Id = apartmentGuid,
                        Name = card.Name,
                        Type = card.Apartment.Type,
                        Options = card.Apartment.Options,
                        UserId = account.UserId,
                        Adress = card.Apartment.Adress,
                        FormattedAdress = card.Apartment.FormattedAdress,
                        Latitude = card.Apartment.Latitude,
                        Longitude = card.Apartment.Longitude,
                        PlaceId = card.Apartment.PlaceId,
                        Lang = card.Lang,
                        Pictures = pics
                    }
                });
                _context.Dates.AddRange(cardDates);
                _context.Genders.AddRange(cardGenders);
                _context.SaveChanges();
                respList.Add(cardGuid);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.InnerException.ToString()}));
            }
        }

        /// <summary>
        ///     PUT api/Card/48D68C86-6EA6-4C25-AA33-223FC9A27959
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
                if (card == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_CARD_NULL));

                // Check Current CARD is not NULL
                var cardCurrent = _context.Cards.SingleOrDefault(a => a.Id == id);
                if (cardCurrent == null)
                {
                    respList.Add(id);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_CARD_NOTFOUND, respList));
                }

                // Check Current User
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
                var account = AuthUtils.GetUserAccount(_context, currentUser);
                if (account == null)
                {
                    respList.Add(currentUser.Id);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                        RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }
                resp = CheckHelper.IsProfileFill(_context, account.UserId, true);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check CARD User
                if (cardCurrent.UserId != account.UserId)
                {
                    respList.Add(cardCurrent.UserId);
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_CARD_WRONG_USER, respList));
                }

                // Check CARD Name is not NULL
                resp = CheckHelper.IsNull(card.Name, "Name", RespH.SRV_CARD_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check CARD not Already Exists
                resp = CheckHelper.IsCardExist(_context, card.Name, RespH.SRV_CARD_EXISTS);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                if (card.Cohabitation == null) card.Cohabitation = ConstVals.Any;

                var cardDates = new List<CardDates>();
                // Check Dates
                if (card.Dates != null)
                {
                    foreach (var dates in card.Dates)
                    {
                        resp = CheckHelper.IsValidDates(dates.DateFrom, dates.DateTo, RespH.SRV_CARD_WRONG_DATE);
                        if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);
                        cardDates.Add(new CardDates
                        {
                            Id = SequentialGuid.NewGuid().ToString(),
                            CardId = id,
                            DateFrom = dates.DateFrom,
                            DateTo = dates.DateTo
                        });
                    }
                }

                var cardGenders = new List<CardGenders>();
                if (card.Genders != null)
                {
                    foreach (var gender in card.Genders)
                    {
                        resp = CheckHelper.IsNull(gender.Name, "Genders.Name", RespH.SRV_CARD_REQUIRED);
                        if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);
                        cardGenders.Add(new CardGenders
                        {
                            Id = SequentialGuid.NewGuid().ToString(),
                            CardId = id,
                            Name = gender.Name,
                            Price = gender.Price
                        });
                    }
                }
                //Apartment
                if (card.Apartment == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NULL));


                var apartment = _context.Apartments.SingleOrDefault(a => a.Id == cardCurrent.ApartmentId);
                if (apartment == null)
                {
                    respList.Add(cardCurrent.ApartmentId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_APARTMENT_NOTFOUND, respList));
                }

                // Check Apartment Adress is not NULL
                resp = CheckHelper.IsNull(card.Apartment.Adress, "Adress", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Apartment Type is not NULL
                resp = CheckHelper.IsNull(card.Apartment.Type, "Type", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Delete Card Dates
                var currentDates = _context.Dates.Where(d => d.CardId == id);
                if (currentDates.Any())
                {
                    _context.Dates.RemoveRange(currentDates);
                    _context.SaveChanges();
                }

                //Delete Card Genders
                var currentGenders = _context.Genders.Where(g => g.CardId == id);
                if (currentGenders.Any())
                {
                    _context.Genders.RemoveRange(currentGenders);
                    _context.SaveChanges();
                }
                // Update CARD
                cardCurrent.Name = card.Name;
                cardCurrent.Description = card.Description;
                cardCurrent.Cohabitation = card.Cohabitation;
                cardCurrent.ResidentGender = card.ResidentGender;

                // Update Apartment
                apartment.Name = card.Name;
                apartment.Type = card.Apartment.Type;
                apartment.Options = card.Apartment.Options;
                apartment.Adress = card.Apartment.Adress;
                apartment.FormattedAdress = card.Apartment.FormattedAdress;
                apartment.Latitude = card.Apartment.Latitude;
                apartment.Longitude = card.Apartment.Longitude;
                apartment.PlaceId = card.Apartment.PlaceId;
                apartment.Lang = card.Apartment.Lang;

                _context.MarkAsModified(apartment);
                _context.Dates.AddRange(cardDates);
                _context.Genders.AddRange(cardGenders);
                _context.SaveChanges();

                respList.Add(id);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.InnerException.ToString()}));
            }
        }

        /// <summary>
        ///     DELETE api/CARD/48D68C86-6EA6-4C25-AA33-223FC9A27959
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
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_CARD_NOTFOUND, respList));
                }

                // Check Current User
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
                var account = AuthUtils.GetUserAccount(_context, currentUser);
                if (account == null)
                {
                    respList.Add(currentUser.Id);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                        RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                // Check CARD User
                if (card.UserId != account.UserId)
                {
                    respList.Add(card.UserId);
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_CARD_WRONG_USER, respList));
                }

                var apartment = _context.Apartments.SingleOrDefault(a => a.Id == card.ApartmentId);

                // Check Apartment is not NULL
                if (apartment == null)
                {
                    respList.Add(id);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_APARTMENT_NOTFOUND, respList));
                }
                // Delete Dates
                var cardDates = _context.Dates.Where(x => x.CardId == card.Id);
                _context.Dates.RemoveRange(cardDates);
                _context.SaveChanges();
                // Delete Genders
                var cardGenders = _context.Genders.Where(x => x.CardId == card.Id);
                _context.Genders.RemoveRange(cardGenders);
                _context.SaveChanges();
                // Delete Notifications
                var notifications =
                    _context.Notifications.Where(
                        x =>
                            x.CardId == card.Id || x.Reservation.CardId == card.Id ||
                            x.Review.Reservation.Card.Id == card.Id || x.Favorite.Card.Id == card.Id);
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
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_DELETED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.InnerException.ToString()}));
            }
        }
    }
}