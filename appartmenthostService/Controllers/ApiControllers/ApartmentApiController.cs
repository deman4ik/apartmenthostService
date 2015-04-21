using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using appartmenthostService.Authentication;
using appartmenthostService.DataObjects;
using appartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace appartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class ApartmentApiController : ApiController
    {
        public ApiServices Services { get; set; }
        appartmenthostContext context = new appartmenthostContext();
        
        // POST api/Apartment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("api/Apartment")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost] 
        public HttpResponseMessage PostApartment(ApartmentDTO apartment)
        {
            try
            {
            // Check Apartment is not NULL 
            if (apartment == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Null object");

            // Check Apartment Name is not NULL
            if (String.IsNullOrWhiteSpace(apartment.Name)) return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Required Name");

            // Check Apartment Adress is not NULL
            if (String.IsNullOrWhiteSpace(apartment.Adress)) return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Required Adress");

            // Check Apartment not Exist
            var currentApartment = context.Apartments.SingleOrDefault(a => a.Name == apartment.Name);
            if (currentApartment != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, String.Format("Apartment already exists {0}", currentApartment.Name));
            // Check Current User
            var currentUser = User as ServiceUser;
            if (currentUser == null) return this.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            var account = AuthUtils.GetUserAccount(currentUser);
            if (account == null) return this.Request.CreateResponse(HttpStatusCode.Unauthorized, String.Format("Can't find user with Id: {0}",currentUser.Id));

            // Check Properties Exists
            foreach (var propVal in apartment.PropsVals)
            {
                var prop = context.Props.AsQueryable().SingleOrDefault(p => p.Id == propVal.PropId);
                if (prop == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, String.Format("Invalid Property {0}", propVal.Id));
                if (propVal.DictionaryItemId != null)
                {
                    var dicItem = context.DictionaryItems.SingleOrDefault(di => di.Id == propVal.DictionaryItemId);
                    if (dicItem == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, String.Format("Invalid Dictionary Item Id: {0}", propVal.DictionaryItemId));
                }
            }

            // Generate 
            string apartmentGuid = Guid.NewGuid().ToString();
            context.Set<Apartment>().Add(new Apartment()
            {
                Id = apartmentGuid,
                Name = apartment.Name,
                UserId = account.UserId,
                Price = apartment.Price,
                Adress = apartment.Adress,
                Latitude = apartment.Latitude,
                Longitude = apartment.Longitude,
                Rating = 0,
                Lang = apartment.Lang,
                PropVals = apartment.PropsVals.Select(pv => new PropVal()
                {
                    Id = Guid.NewGuid().ToString(),
                    PropId = pv.PropId,
                    TableItemId = apartmentGuid,
                    StrValue = pv.StrValue,
                    NumValue = pv.NumValue,
                    DateValue = pv.DateValue,
                    BoolValue = pv.BoolValue,
                    DictionaryItemId = pv.DictionaryItemId,
                    Lang = apartment.Lang
                }).ToList()

            });


           
                context.SaveChanges();
                return this.Request.CreateResponse(HttpStatusCode.Created, apartmentGuid);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, ex.InnerException);
            }
          
            
        }


        // PUT api/Apartment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("api/Apartment/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPut]
        public HttpResponseMessage PutApartment(string id, ApartmentDTO apartment)
        {
            return null;
        }

    }
}
