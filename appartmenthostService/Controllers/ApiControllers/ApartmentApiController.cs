using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using Microsoft.Owin.Security;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class ApartmentApiController : ApiController
    {
        public ApiServices Services { get; set; }
        apartmenthostContext context = new apartmenthostContext();
        
        // POST api/Apartment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("api/Apartment")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost] 
        public HttpResponseMessage PostApartment(ApartmentDTO apartment)
        {
            try
            {
                var respList = new List<string>();
            // Check Apartment is not NULL 
                if (apartment == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.APARTMENT_NULL));
                

            // Check Apartment Name is not NULL
                if (String.IsNullOrWhiteSpace(apartment.Name))
                {
                    respList.Add("Name");
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.APARTMENT_REQUIRED, respList));
                }

            // Check Apartment Adress is not NULL
                if (String.IsNullOrWhiteSpace(apartment.Adress))
                {
                    respList.Add("Adress");
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.APARTMENT_REQUIRED, respList));
                }

            // Check Apartment not Exist
            var currentApartmentCount = context.Apartments.Count(a => a.Name == apartment.Name);
                if (currentApartmentCount > 0)
                {
                    respList.Add(apartment.Name);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.APARTMENT_EXISTS, respList));
                }
            // Check Current User
            var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.UNAUTH));
            var account = AuthUtils.GetUserAccount(currentUser);
                if (account == null)
                {
                    respList.Add(currentUser.Id);
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.USER_NOTFOUND, respList));
                }

            // Check Properties Exists
            foreach (var propVal in apartment.PropsVals)
            {
                var prop = context.Props.AsQueryable().SingleOrDefault(p => p.Id == propVal.PropId);
                if (prop == null)
                {
                    respList.Add(propVal.PropId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.APARTMENT_PROP_NOTFOUND, respList));
                }
                if (propVal.DictionaryItemId != null)
                {
                    var dicItem = context.DictionaryItems.SingleOrDefault(di => di.Id == propVal.DictionaryItemId);
                    if (dicItem == null)
                    {
                        respList.Add(propVal.DictionaryItemId);
                        return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.DICTIONARYITEM_NOTFOUND, respList));
                    }
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
                respList.Add(apartmentGuid);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.CREATED, respList));
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
                 var respList = new List<string>();
                 // Check Apartment is not NULL 
                 if (apartment == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.APARTMENT_NULL));

                 // Check Current Apartment is not NULL
                 var apartmentCurrent = context.Apartments.SingleOrDefault(a => a.Id == id);
                 if (apartmentCurrent == null) 
                 {
                 respList.Add(id);
                 return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.APARTMENT_NOTFOUND, respList));
                 }

                 // Check Apartment Name is not NULL
                 if (String.IsNullOrWhiteSpace(apartment.Name)) 
                 {
                    respList.Add("Name");
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.APARTMENT_REQUIRED, respList));
                 }

                 // Check Apartment Adress is not NULL
                 if (String.IsNullOrWhiteSpace(apartment.Adress))
                 {
                     respList.Add("Adress");
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.APARTMENT_REQUIRED, respList));
                 }

                 // Check Current User
                 var currentUser = User as ServiceUser;
                 if (currentUser == null) return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.UNAUTH));
                 var account = AuthUtils.GetUserAccount(currentUser);
                 if (account == null)
                 {
                     respList.Add(currentUser.Id);
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.USER_NOTFOUND, respList));
                 }

                 // Check Apartment User
                 if (apartment.UserId != account.UserId)
                 {
                     respList.Add(apartment.UserId);
                     respList.Add(account.UserId);
                     return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                         RespH.Create(RespH.APARTMENT_WRONG_USER,respList));
                 }
                 if (apartmentCurrent.UserId != account.UserId)
                 {
                     respList.Add(apartmentCurrent.UserId);
                     respList.Add(account.UserId);
                     return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                         RespH.Create(RespH.APARTMENT_WRONG_USER,respList));
                 }

                 // Check Apartment not Exist
                 var currentApartmentCount = context.Apartments.Count(a => a.Name == apartment.Name && a.Id != id);
                 if (currentApartmentCount > 0)
                 {
                     respList.Add(apartment.Name);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.APARTMENT_EXISTS, respList));
                 }

                  
                 // Check Properties Exists
                 foreach (var propVal in apartment.PropsVals)
                 {
                     var prop = context.Props.AsQueryable().SingleOrDefault(p => p.Id == propVal.PropId);
                     if (prop == null)
                     {
                         respList.Add(propVal.PropId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.APARTMENT_PROP_NOTFOUND, respList));
                      }

                     if (propVal.DictionaryItemId != null)
                     {
                         var dicItem = context.DictionaryItems.SingleOrDefault(di => di.Id == propVal.DictionaryItemId);
                         if (dicItem == null)
                         {
                             respList.Add(propVal.DictionaryItemId);
                             return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.DICTIONARYITEM_NOTFOUND, respList));
                         }
                     }
                 }
                     // Update PropVals
                 foreach (var propVal in apartment.PropsVals)
                 {
                     var propValCurrent = context.PropVals.SingleOrDefault(pv => pv.Id == propVal.Id);
                     if (propValCurrent == null)
                     {
                         respList.Add(propVal.Id);
                         return
                         this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.APARTMENT_PROPVAL_NOTFOUND, respList));
                    }
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

                 context.SaveChanges();

                 respList.Add(apartment.Id);
                 return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.UPDATED, respList));
             }
             catch (Exception ex)
             {
                 System.Diagnostics.Debug.WriteLine(ex.InnerException);
                 return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
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
                 var respList = new List<string>();
                 var apartment = context.Apartments.SingleOrDefault(a => a.Id == id);
                 
                 // Check Apartment is not NULL
                 if (apartment == null)
                 {
                     respList.Add(id);
                     return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.APARTMENT_NOTFOUND, respList));
                 }

                 // Check Current User
                 var currentUser = User as ServiceUser;
                 if (currentUser == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.UNAUTH));
                 var account = AuthUtils.GetUserAccount(currentUser);
                 if (account == null)
                 {
                     respList.Add(currentUser.Id);
                     return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.USER_NOTFOUND, respList));
                 }

                 // Check Apartment User
                 if (apartment.UserId != account.UserId)
                 {
                     respList.Add(apartment.UserId);
                     respList.Add(account.UserId);
                     return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                         RespH.Create(RespH.APARTMENT_WRONG_USER, respList));
                  }

                 // Check Adverts with such Apartment
                 var adverts = context.Adverts.Where(adv => adv.ApartmentId == id).Select(a => a.Id);
                 if (adverts.Any())
                 {
                     foreach (var advert in adverts)
                     {
                         respList.Add(advert);
                     }
                     return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                         RespH.Create(RespH.APARTMENT_DEPENDENCY, respList));
                 }

                // Delete Apartment with PropVals
                 context.Apartments.Remove(apartment);
                 context.SaveChanges();
                 respList.Add(apartment.Id);
                 return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.DELETED, respList));

             }
             catch (Exception ex)
             {
                 System.Diagnostics.Debug.WriteLine(ex.InnerException);
                 return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
             }
        }
    }
}
