using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
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
         apartmenthostContext context = new apartmenthostContext();
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            
            DomainManager = new EntityDomainManager<Advert>(context, Request, Services);
        }

        // GET tables/Advert
       // [QueryableExpand("Apartments")]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        public IQueryable<AdvertDTO> GetAllAdverts()
        {
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
                Lang = x.Lang,
                User = new UserDTO()
                {
                    Id = x.User.Profile.Id,
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
                    Adress = x.Apartment.Adress,
                    Latitude = x.Apartment.Latitude,
                    Longitude = x.Apartment.Longitude,
                },
                ApprovedReservations = x.Reservations.Where(r => r.Status == ConstVals.Accepted).Select(rv => new ReservationDTO()
                {
                    DateFrom = rv.DateFrom,
                    DateTo = rv.DateTo,
                    UserId = rv.UserId,
                    //User = new UserDTO()
                    //{
                    //    Email = rv.User.Email,
                    //    FirstName = rv.User.Profile.FirstName,
                    //    LastName = rv.User.Profile.LastName,
                    //    Phone = rv.User.Profile.Phone
                    //}
                }).ToList()
                

            });
        }

        // GET tables/Advert/48D68C86-6EA6-4C25-AA33-223FC9A27959
       // [QueryableExpand("Apartments")]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        public SingleResult<AdvertDTO> GetAdvert(string id)
        {
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
                Lang = x.Lang,
                User = new UserDTO()
                {
                    Id = x.User.Profile.Id,
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
                    Adress = x.Apartment.Adress,
                    Latitude = x.Apartment.Latitude,
                    Longitude = x.Apartment.Longitude,
                },
                ApprovedReservations = x.Reservations.Where(r => r.Status == ConstVals.Accepted).Select(rv => new ReservationDTO()
                {
                    DateFrom = rv.DateFrom,
                    DateTo = rv.DateTo,
                    UserId = rv.UserId,
                    //User = new UserDTO()
                    //{
                    //    Email = rv.User.Email,
                    //    FirstName = rv.User.Profile.FirstName,
                    //    LastName = rv.User.Profile.LastName,
                    //    Phone = rv.User.Profile.Phone
                    //}
                }).ToList()

            });
            return SingleResult.Create(result);
        }

    }
}