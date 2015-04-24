using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using appartmenthostService.Authentication;
using appartmenthostService.DataObjects;
using appartmenthostService.Helpers;
using appartmenthostService.Models;
using Microsoft.Owin.Security;
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
                if (apartment == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.APARTMENT_NULL));

            // Check Apartment Name is not NULL
                if (String.IsNullOrWhiteSpace(apartment.Name)) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.APARTMENT_REQUIRED, new List<string>() { "Name" }));

            // Check Apartment Adress is not NULL
                if (String.IsNullOrWhiteSpace(apartment.Adress)) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.APARTMENT_REQUIRED, new List<string>() { "Adress" }));

            // Check Apartment not Exist
            var currentApartmentCount = context.Apartments.Count(a => a.Name == apartment.Name);
            if (currentApartmentCount > 0) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.APARTMENT_EXISTS, new List<string>() { apartment.Name}));
            // Check Current User
            var currentUser = User as ServiceUser;
            if (currentUser == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.UNAUTH));
            var account = AuthUtils.GetUserAccount(currentUser);
            if (account == null) return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.USER_NOTFOUND, new List<string>() { currentUser.Id }));

            // Check Properties Exists
            foreach (var propVal in apartment.PropsVals)
            {
                var prop = context.Props.AsQueryable().SingleOrDefault(p => p.Id == propVal.PropId);
                if (prop == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.APARTMENT_PROP_NOTFOUND, new List<string>(){ propVal.Id}));
                if (propVal.DictionaryItemId != null)
                {
                    var dicItem = context.DictionaryItems.SingleOrDefault(di => di.Id == propVal.DictionaryItemId);
                    if (dicItem == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.DICTIONARYITEM_NOTFOUND, new List<string>(){ propVal.DictionaryItemId}));
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
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.CREATED, new List<string>(){ apartmentGuid}));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,  RespH.Create(RespH.EXCEPTION, new List<string>(){ ex.InnerException.ToString()}));
            }
          
            
        }


        // PUT api/Apartment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("api/Apartment/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPut]
        public HttpResponseMessage PutApartment(string id, ApartmentDTO apartment)
        {
             try
             {

                 // Check Apartment is not NULL 
                 if (apartment == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Null object");

                 // Check Current Apartment is not NULL
                 var apartmentCurrent = context.Apartments.SingleOrDefault(a => a.Id == id);
                 if (apartmentCurrent == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, String.Format("Apartment Not Found Id: {0}", id));

                 // Check Apartment Name is not NULL
                 if (String.IsNullOrWhiteSpace(apartment.Name)) return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Required Name");

                 // Check Apartment Adress is not NULL
                 if (String.IsNullOrWhiteSpace(apartment.Adress)) return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Required Adress");

                 // Check Current User
                 var currentUser = User as ServiceUser;
                 if (currentUser == null) return this.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
                 var account = AuthUtils.GetUserAccount(currentUser);
                 if (account == null) return this.Request.CreateResponse(HttpStatusCode.Unauthorized, String.Format("Can't find user with Id: {0}", currentUser.Id));

                 // Check Apartment User
                 if (apartment.UserId != account.UserId)
                     return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                         String.Format("Can't update apartment with UserId: {0} for Currenr User Id: {1}",
                             apartment.UserId, account.UserId));
                 if (apartmentCurrent.UserId != account.UserId)
                     return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                         String.Format("Can't update apartment with UserId: {0} for Currenr User Id: {1}",
                             apartmentCurrent.UserId, account.UserId));

                 // Check Apartment not Exist
                 var currentApartmentCount = context.Apartments.Count(a => a.Name == apartment.Name && a.Id != id);
                 if (currentApartmentCount > 0) return this.Request.CreateResponse(HttpStatusCode.BadRequest, String.Format("Apartment already exists with Name {0}", apartment.Name));

                  
                 // Check Properties Exists
                 foreach (var propVal in apartment.PropsVals)
                 {
                     var prop = context.Props.AsQueryable().SingleOrDefault(p => p.Id == propVal.PropId);
                     if (prop == null)
                         return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                             String.Format("Invalid Property PropId: {0}", propVal.PropId));
                     if (propVal.DictionaryItemId != null)
                     {
                         var dicItem = context.DictionaryItems.SingleOrDefault(di => di.Id == propVal.DictionaryItemId);
                         if (dicItem == null)
                             return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                                 String.Format("Invalid Dictionary Item Id: {0}", propVal.DictionaryItemId));
                     }
                 }
                     // Update PropVals
                 foreach (var propVal in apartment.PropsVals)
                 {
                     var propValCurrent = context.PropVals.SingleOrDefault(pv => pv.Id == propVal.Id);
                     if (propValCurrent == null)
                         return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                             String.Format("Invalid Property Value Id: {0}", propVal.Id));
                     propValCurrent.StrValue = propVal.StrValue;
                     propValCurrent.NumValue = propVal.NumValue;
                     propValCurrent.DateValue = propVal.DateValue;
                     propValCurrent.BoolValue = propVal.BoolValue;
                     propValCurrent.DictionaryItemId = propVal.DictionaryItemId;
                     propValCurrent.Lang = apartment.Lang;
                     context.SaveChanges();
                 }

                 // Update Apartment
                 apartmentCurrent.Name = apartment.Name;
                 apartmentCurrent.Price = apartment.Price;
                 apartmentCurrent.Adress = apartment.Adress;
                 apartmentCurrent.Latitude = apartment.Latitude;
                 apartmentCurrent.Longitude = apartment.Longitude;
                 apartmentCurrent.Rating = apartment.Rating;
                 apartmentCurrent.Lang = apartment.Lang;


                 //foreach (var propValCurrent in apartmentCurrent.PropVals)
                 //{
                 //    var propVal = apartmentCurrent.PropVals.SingleOrDefault(pv => pv.Id == propValCurrent.Id);
                 //    if (propVal == null) continue;
                 //    propValCurrent.StrValue = propVal.StrValue;
                 //    propValCurrent.NumValue = propVal.NumValue;
                 //    propValCurrent.DateValue = propVal.DateValue;
                 //    propValCurrent.BoolValue = propVal.BoolValue;
                 //    propValCurrent.DictionaryItemId = propVal.DictionaryItemId;
                 //    propValCurrent.Lang = apartment.Lang;
                 //}

                 context.SaveChanges();
                 return this.Request.CreateResponse(HttpStatusCode.OK, apartment.Id); 
             }
             catch (Exception ex)
             {
                 System.Diagnostics.Debug.WriteLine(ex.InnerException);
                 return this.Request.CreateResponse(HttpStatusCode.BadRequest, ex.InnerException);
             }
        }

        
        // DELETE api/Apartment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("api/Apartment/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpDelete]
        public HttpResponseMessage DeleteApartment(string id)
        {
             try
             {
                 var apartment = context.Apartments.SingleOrDefault(a => a.Id == id);
                 
                 // Check Apartment is not NULL
                 if (apartment == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, String.Format("Apartment Not Found Id: {0}",id));

                 // Check Current User
                 var currentUser = User as ServiceUser;
                 if (currentUser == null) return this.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
                 var account = AuthUtils.GetUserAccount(currentUser);
                 if (account == null) return this.Request.CreateResponse(HttpStatusCode.Unauthorized, String.Format("Can't find user with Id: {0}", currentUser.Id));

                 // Check Apartment User
                 if (apartment.UserId != account.UserId)
                     return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                         String.Format("Can't delete apartment with UserId: {0} for Currenr User Id: {1}",
                             apartment.UserId, account.UserId));

                 // Check Adverts with such Apartment
                 var advertsCount = context.Adverts.Count(adv => adv.ApartmentId == id);
                 if (advertsCount > 0)
                     return this.Request.CreateResponse(HttpStatusCode.Conflict,
                         String.Format("Can't delete Apartment because it is attached to {0} Adverts", advertsCount));

                // Delete Apartment with PropVals
                 context.Apartments.Remove(apartment);
                 context.SaveChanges();
                 return this.Request.CreateResponse(HttpStatusCode.OK);

             }
             catch (Exception ex)
             {
                 System.Diagnostics.Debug.WriteLine(ex.InnerException);
                 return this.Request.CreateResponse(HttpStatusCode.BadRequest, ex.InnerException);
             }
        }
    }
}
