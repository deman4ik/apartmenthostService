using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using apartmenthostService.DataObjects;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
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
                CardId = r.CardId,
                UserId = r.UserId,
                Status = r.Status,
                DateFrom = r.DateFrom,
                DateTo = r.DateTo,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                User = new BaseUserDTO()
                {
                    Id = r.User.Profile.Id,
                    Email =  r.User.Email,
                    FirstName = r.User.Profile.FirstName,
                    LastName = r.User.Profile.LastName,
                    Rating = r.User.Profile.Rating,
                    Gender = r.User.Profile.Gender
                    

                },
                Card = new CardDTO()
            {
                Name = r.Card.Name,
                UserId = r.Card.UserId,
                Description = r.Card.Description,
                ApartmentId = r.Card.ApartmentId,
                DateFrom = r.Card.DateFrom,
                DateTo = r.Card.DateTo,
                PriceDay = r.Card.PriceDay,
                PricePeriod = r.Card.PricePeriod,
                Cohabitation = r.Card.Cohabitation,
                ResidentGender = r.Card.ResidentGender,
                Lang = r.Card.Lang,
                User = new UserDTO()
                {
                    Id = r.Card.User.Profile.Id,
                    FirstName = r.Card.User.Profile.FirstName,
                    LastName = r.Card.User.Profile.LastName,
                    Gender = r.Card.User.Profile.Gender,
                    Phone = r.Card.User.Profile.Phone
                },
                Apartment = new ApartmentDTO()
                {
                    Id = r.Card.Apartment.Id,
                    Name = r.Card.Apartment.Name,
                    Type = r.Card.Apartment.Type,
                    Options = r.Card.Apartment.Options,
                    UserId = r.Card.Apartment.UserId,
                    Adress = r.Card.Apartment.Adress,
                    Latitude = r.Card.Apartment.Latitude,
                    Longitude = r.Card.Apartment.Longitude,
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
                CardId = r.CardId,
                UserId = r.UserId,
                Status = r.Status,
                DateFrom = r.DateFrom,
                DateTo = r.DateTo,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                User = new BaseUserDTO()
                {
                    Id = r.User.Profile.Id,
                    Email = r.User.Email,
                    FirstName = r.User.Profile.FirstName,
                    LastName = r.User.Profile.LastName,
                    Rating = r.User.Profile.Rating,
                    Gender = r.User.Profile.Gender
                },
                Card = new CardDTO()
                {
                    Name = r.Card.Name,
                    UserId = r.Card.UserId,
                    Description = r.Card.Description,
                    ApartmentId = r.Card.ApartmentId,
                    DateFrom = r.Card.DateFrom,
                    DateTo = r.Card.DateTo,
                    PriceDay = r.Card.PriceDay,
                    PricePeriod = r.Card.PricePeriod,
                    Cohabitation = r.Card.Cohabitation,
                    ResidentGender = r.Card.ResidentGender,
                    Lang = r.Card.Lang,
                    User = new UserDTO()
                    {
                        Id = r.Card.User.Profile.Id,
                        FirstName = r.Card.User.Profile.FirstName,
                        LastName = r.Card.User.Profile.LastName,
                        Gender = r.Card.User.Profile.Gender,
                        Phone = r.Card.User.Profile.Phone
                    },
                    Apartment = new ApartmentDTO()
                    {
                        Id = r.Card.Apartment.Id,
                        Name = r.Card.Apartment.Name,
                        Type = r.Card.Apartment.Type,
                        Options = r.Card.Apartment.Options,
                        UserId = r.Card.Apartment.UserId,
                        Adress = r.Card.Apartment.Adress,
                        Latitude = r.Card.Apartment.Latitude,
                        Longitude = r.Card.Apartment.Longitude,
                    }

                }
            });
            return SingleResult.Create(result);
        }


    }
}