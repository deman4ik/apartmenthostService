using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using apartmenthostService.DataObjects;
using Microsoft.WindowsAzure.Mobile.Service;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class ReservationController : TableController<Reservation>
    {
        private apartmenthostContext _context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            _context = new apartmenthostContext();
            DomainManager = new EntityDomainManager<Reservation>(_context, Request, Services);
        }

        // GET tables/Reservation
        [AuthorizeLevel(AuthorizationLevel.User)]
        public IQueryable<ReservationDTO> GetAllReservation()
        {
            return Query().Select(r => new ReservationDTO
            {
                Id = r.Id,
                AdvertId = r.AdvertId,
                UserId = r.UserId,
                Status = r.Status,
                DateFrom = r.DateFrom,
                DateTo = r.DateTo,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                User = new UserDTO()
                {
                    FirstName = r.User.Profile.FirstName,
                    LastName = r.User.Profile.LastName,
                    Gender = r.User.Profile.Gender,
                    Phone = r.User.Profile.Phone
                },
                Advert = new AdvertDTO()
            {
                Name = r.Advert.Name,
                UserId = r.Advert.UserId,
                Description = r.Advert.Description,
                ApartmentId = r.Advert.ApartmentId,
                DateFrom = r.Advert.DateFrom,
                DateTo = r.Advert.DateTo,
                PriceDay = r.Advert.PriceDay,
                PricePeriod = r.Advert.PricePeriod,
                Cohabitation = r.Advert.Cohabitation,
                ResidentGender = r.Advert.ResidentGender,
                Lang = r.Advert.Lang,
                User = new UserDTO()
                {
                    Id = r.Advert.User.Profile.Id,
                    FirstName = r.Advert.User.Profile.FirstName,
                    LastName = r.Advert.User.Profile.LastName,
                    Gender = r.Advert.User.Profile.Gender,
                    Phone = r.Advert.User.Profile.Phone
                },
                Apartment = new ApartmentDTO()
                {
                    Id = r.Advert.Apartment.Id,
                    Name = r.Advert.Apartment.Name,
                    Type = r.Advert.Apartment.Type,
                    Options = r.Advert.Apartment.Options,
                    UserId = r.Advert.Apartment.UserId,
                    Adress = r.Advert.Apartment.Adress,
                    Latitude = r.Advert.Apartment.Latitude,
                    Longitude = r.Advert.Apartment.Longitude,
                }

            }
            });
        }

        // GET tables/Reservation/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [AuthorizeLevel(AuthorizationLevel.User)]
        public SingleResult<ReservationDTO> GetReservation(string id)
        {
            var result = Lookup(id).Queryable.Select(r => new ReservationDTO
            {
                Id = r.Id,
                AdvertId = r.AdvertId,
                UserId = r.UserId,
                Status = r.Status,
                DateFrom = r.DateFrom,
                DateTo = r.DateTo,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                User = new UserDTO()
                {
                    FirstName = r.User.Profile.FirstName,
                    LastName = r.User.Profile.LastName,
                    Gender = r.User.Profile.Gender,
                    Phone = r.User.Profile.Phone
                },
                Advert = new AdvertDTO()
                {
                    Name = r.Advert.Name,
                    UserId = r.Advert.UserId,
                    Description = r.Advert.Description,
                    ApartmentId = r.Advert.ApartmentId,
                    DateFrom = r.Advert.DateFrom,
                    DateTo = r.Advert.DateTo,
                    PriceDay = r.Advert.PriceDay,
                    PricePeriod = r.Advert.PricePeriod,
                    Cohabitation = r.Advert.Cohabitation,
                    ResidentGender = r.Advert.ResidentGender,
                    Lang = r.Advert.Lang,
                    User = new UserDTO()
                    {
                        Id = r.Advert.User.Profile.Id,
                        FirstName = r.Advert.User.Profile.FirstName,
                        LastName = r.Advert.User.Profile.LastName,
                        Gender = r.Advert.User.Profile.Gender,
                        Phone = r.Advert.User.Profile.Phone
                    },
                    Apartment = new ApartmentDTO()
                    {
                        Id = r.Advert.Apartment.Id,
                        Name = r.Advert.Apartment.Name,
                        Type = r.Advert.Apartment.Type,
                        Options = r.Advert.Apartment.Options,
                        UserId = r.Advert.Apartment.UserId,
                        Adress = r.Advert.Apartment.Adress,
                        Latitude = r.Advert.Apartment.Latitude,
                        Longitude = r.Advert.Apartment.Longitude,
                    }

                }
            });
            return SingleResult.Create(result);
        }


    }
}