﻿using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Web.Http;
using System.Web.Http.Controllers;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class CardController : TableController<Card>
    {
        private apartmenthostContext _context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            _context = new apartmenthostContext();
            DomainManager = new EntityDomainManager<Card>(_context, Request, Services);
        }

        // GET tables/Card
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        public IQueryable<CardDTO> GetAllCards()
        {
            var currentUser = User as ServiceUser;
            var account = AuthUtils.GetUserAccount(_context, currentUser);
            string userId = null;
            if (account != null)
            {
                userId = account.UserId;
            }
            return Query().Select(x => new CardDTO()
            {
                Id = x.Id,
                Name = x.Name,
                UserId = x.UserId,
                Description = x.Description,
                ApartmentId = x.ApartmentId,
                DateFrom = x.DateFrom,
                DateTo = x.DateTo,
                PriceDay = x.PriceDay,
                PricePeriod = x.PriceDay * 7,
                Cohabitation = x.Cohabitation,
                ResidentGender = x.ResidentGender,
                IsFavorite = x.Favorites.Any(f => f.UserId == userId),
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Lang = x.Lang,
                User = new UserDTO()
                {
                    Id = x.User.Profile.Id,
                    FirstName = x.User.Profile.FirstName,
                    LastName = x.User.Profile.LastName,
                    Gender = x.User.Profile.Gender,
                    Rating = x.User.Profile.Rating,
                    Phone = x.User.Profile.Phone
                },
                Apartment = new ApartmentDTO()
                {
                    Id = x.Apartment.Id,
                    Name = x.Apartment.Name,
                    Type = x.Apartment.Type,
                    Options = x.Apartment.Options,
                    UserId = x.Apartment.UserId,
                    Adress = x.Apartment.Adress,
                    Latitude = x.Apartment.Latitude,
                    Longitude = x.Apartment.Longitude,
                },
                ApprovedReservations = x.Reservations.Where(r => r.Status == ConstVals.Accepted).Select(rv => new ReservationDTO()
                {
                    DateFrom = rv.DateFrom,
                    DateTo = rv.DateTo,
                    UserId = rv.UserId,
                    User = new BaseUserDTO()
                    {
                        Id = rv.User.Profile.Id,
                        Email = rv.User.Email,
                        FirstName = rv.User.Profile.FirstName,
                        LastName = rv.User.Profile.LastName,
                        Rating = rv.User.Profile.Rating,
                        Gender = rv.User.Profile.Gender
                    }
                }).ToList()


            });
        }

        // GET tables/Card/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        public SingleResult<CardDTO> GetCard(string id)
        {

            var currentUser = User as ServiceUser;
            var account = AuthUtils.GetUserAccount(_context, currentUser);
            string userId = null;
            if (account != null)
            {
                userId = account.UserId;
            }
           // var result = Lookup(id).Queryable
            var result = _context.Cards.Where(c => c.Id == id).Take(1).Select(x => new CardDTO()
            {
                Id = x.Id,
                Name = x.Name,
                UserId = x.UserId,
                Description = x.Description,
                ApartmentId = x.ApartmentId,
                DateFrom = x.DateFrom,
                DateTo = x.DateTo,
                PriceDay = x.PriceDay,
                PricePeriod = x.PriceDay * 7,
                Cohabitation = x.Cohabitation,
                ResidentGender = x.ResidentGender,
                IsFavorite = x.Favorites.Any(f => f.UserId == userId),
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Lang = x.Lang,
                User = new UserDTO()
                {
                    Id = x.User.Profile.Id,
                    Email = x.User.Email,
                    FirstName = x.User.Profile.FirstName,
                    LastName = x.User.Profile.LastName,
                    Gender = x.User.Profile.Gender,
                    Phone = x.User.Profile.Phone
                },
                Apartment = new ApartmentDTO()
                {
                    Id = x.Apartment.Id,
                    Name = x.Apartment.Name,
                    Type = x.Apartment.Type,
                    Options = x.Apartment.Options,
                    UserId = x.Apartment.UserId,
                    Adress = x.Apartment.Adress
                },
                ApprovedReservations = x.Reservations.Where(r => r.Status == ConstVals.Accepted).Select(rv => new ReservationDTO()
                {
                    DateFrom = rv.DateFrom,
                    DateTo = rv.DateTo,
                    UserId = rv.UserId,
                    User = new BaseUserDTO()
                    {
                        Id = rv.User.Profile.Id,
                        Email = rv.User.Email,
                        FirstName = rv.User.Profile.FirstName,
                        LastName = rv.User.Profile.LastName,
                        Rating = rv.User.Profile.Rating,
                        Gender = rv.User.Profile.Gender
                    }
                }).ToList(),
                Reviews = x.User.InReviews.Where(inr => inr.Reservation.CardId == x.Id).Select(rev => new ReviewDTO()
                {
                    Id = rev.Id,
                    Rating = rev.Rating,
                    Text = rev.Text,
                    CreatedAt = rev.CreatedAt,
                    FromUser = new BaseUserDTO()
                    {
                        Id = rev.FromUser.Profile.Id,
                        Email = rev.FromUser.Email,
                        FirstName = rev.FromUser.Profile.FirstName,
                        LastName = rev.FromUser.Profile.LastName,
                        Rating = rev.FromUser.Profile.Rating,
                        Gender = rev.FromUser.Profile.Gender
                    }
                }).ToList(),
                RelatedCards = _context.Cards.Where(crd => crd.Id != x.Id && crd.ResidentGender == x.ResidentGender && crd.Apartment.Type == x.Apartment.Type).Take(5).Select(card => new RelatedCardDTO
                {
                    Id = card.Id,
                    Name = card.Name,
                    UserId = card.UserId,
                    Description = card.Description,
                    ApartmentId = card.ApartmentId,
                    DateFrom = card.DateFrom,
                    DateTo = card.DateTo,
                    PriceDay = card.PriceDay,
                    PricePeriod = card.PriceDay * 7,
                    Cohabitation = card.Cohabitation,
                    ResidentGender = card.ResidentGender,
                    IsFavorite = card.Favorites.Any(f => f.UserId == userId),
                    CreatedAt = card.CreatedAt,
                    Lang = card.Lang,


                    Apartment = new ApartmentDTO()
                    {
                        Id = card.Apartment.Id,
                        Name = card.Apartment.Name,
                        Type = card.Apartment.Type,
                        Options = card.Apartment.Options,
                        UserId = card.Apartment.UserId,
                        Adress = card.Apartment.Adress
                    },
                    User = new BaseUserDTO()
                    {
                        Id = card.User.Profile.Id,
                        Email = card.User.Email,
                        FirstName = card.User.Profile.FirstName,
                        LastName = card.User.Profile.LastName,
                        Rating = card.User.Profile.Rating,
                        Gender = card.User.Profile.Gender
                    }

                }).ToList()

            });
            return SingleResult.Create(result);
        }



    }
}