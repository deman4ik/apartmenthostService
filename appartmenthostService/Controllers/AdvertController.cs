using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using appartmenthostService.DataObjects;
using Microsoft.WindowsAzure.Mobile.Service;
using appartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace appartmenthostService.Controllers
{
     [AuthorizeLevel(AuthorizationLevel.Anonymous)]
    public class AdvertController : TableController<Advert>
    {
         appartmenthostContext context = new appartmenthostContext();
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            
            DomainManager = new EntityDomainManager<Advert>(context, Request, Services);
        }

        // GET tables/Advert
       // [QueryableExpand("Apartments")]
        public IQueryable<AdvertDTO> GetAllAdverts()
        {
            return Query().Select(x => new AdvertDTO()
            {
                Name = x.Name,
                UserId = x.UserId,
                DefaultPictureId = x.DefaultPictureId,
                Type = x.Type,
                Description = x.Description,
                ApartmentId = x.ApartmentId,
                DateFrom = x.DateFrom,
                DateTo = x.DateTo,
                User =  new UserDTO()
                {
                    Email = x.User.Email,
                    FirstName = x.User.Profile.FirstName,
                    LastName = x.User.Profile.LastName,
                    Gender = x.User.Profile.Gender,
                    Phone = x.User.Profile.ContactKind,
                    PictureId = x.User.Profile.PictureId
                },
                Apartment = new ApartmentDTO()
                {
                    Name = x.Apartment.Name,
                    UserId = x.Apartment.UserId,
                    Price = x.Apartment.Price,
                    PriceTotal = x.Apartment.PriceTotal,
                    Cohabitation = x.Apartment.Сohabitation,
                    Adress = x.Apartment.Adress,
                    Latitude = x.Apartment.Latitude,
                    Longitude = x.Apartment.Longitude,
                    Rating = x.Apartment.Rating
                }

            });
        }

        // GET tables/Advert/48D68C86-6EA6-4C25-AA33-223FC9A27959
       // [QueryableExpand("Apartments")]
        public SingleResult<Advert> GetAdvert(string id)
        {
            return Lookup(id);
        }

    }
}