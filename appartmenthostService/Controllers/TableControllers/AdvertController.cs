using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using Microsoft.WindowsAzure.Mobile.Service;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class AdvertController : TableController<Advert>
    {
        private apartmenthostContext _context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            _context = new apartmenthostContext();
            DomainManager = new EntityDomainManager<Advert>(_context, Request, Services);
        }

        // GET tables/Advert
        // [QueryableExpand("Apartments")]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        public IQueryable<AdvertDTO> GetAllAdverts()
        {
            var currentUser = User as ServiceUser;
            var account = AuthUtils.GetUserAccount(_context, currentUser);
            string userId = null;
            if (account != null)
            {
                userId = account.UserId;
            }
            return Query().Select(x => new AdvertDTO()
            {
                Id = x.Id,
                Name = x.Name,
                UserId = x.UserId,
                Description = x.Description,
                ApartmentId = x.ApartmentId,
                DateFrom = x.DateFrom,
                DateTo = x.DateTo,
                PriceDay = x.PriceDay,
                PricePeriod = x.PricePeriod,
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

        // GET tables/Advert/48D68C86-6EA6-4C25-AA33-223FC9A27959
        // [QueryableExpand("Apartments")]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        public SingleResult<AdvertDTO> GetAdvert(string id)
        {

            var currentUser = User as ServiceUser;
            var account = AuthUtils.GetUserAccount(_context, currentUser);
            string userId = null;
            if (account != null)
            {
                userId = account.UserId;
            }
            var result = Lookup(id).Queryable.Select(x => new AdvertDTO()
            {
                Id = x.Id,
                Name = x.Name,
                UserId = x.UserId,
                Description = x.Description,
                ApartmentId = x.ApartmentId,
                DateFrom = x.DateFrom,
                DateTo = x.DateTo,
                PriceDay = x.PriceDay,
                PricePeriod = x.PricePeriod,
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
                Reviews = x.User.InReviews.Select(rev => new ReviewDTO()
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
                RelatedAdverts = _context.Adverts.Where(adv => adv.Id != x.Id && adv.ResidentGender == x.ResidentGender && adv.Apartment.Type == x.Apartment.Type).Take(5).Select(advert => new RelatedAdvertDTO()
                {
                    Id = advert.Id,
                    Name = advert.Name,
                    UserId = advert.UserId,
                    Description = advert.Description,
                    ApartmentId = advert.ApartmentId,
                    DateFrom = advert.DateFrom,
                    DateTo = advert.DateTo,
                    PriceDay = advert.PriceDay,
                    PricePeriod = advert.PricePeriod,
                    Cohabitation = advert.Cohabitation,
                    ResidentGender = advert.ResidentGender,
                    IsFavorite = advert.Favorites.Any(f => f.UserId == userId),
                    CreatedAt = advert.CreatedAt,
                    Lang = advert.Lang,


                    Apartment = new ApartmentDTO()
                    {
                        Id = advert.Apartment.Id,
                        Name = advert.Apartment.Name,
                        Type = advert.Apartment.Type,
                        Options = advert.Apartment.Options,
                        UserId = advert.Apartment.UserId,
                        Adress = advert.Apartment.Adress
                    },
                    User = new BaseUserDTO()
                    {
                        Id = advert.User.Profile.Id,
                        Email = advert.User.Email,
                        FirstName = advert.User.Profile.FirstName,
                        LastName = advert.User.Profile.LastName,
                        Rating = advert.User.Profile.Rating,
                        Gender = advert.User.Profile.Gender
                    }

                }).ToList()

            });
            return SingleResult.Create(result);
        }



    }
}