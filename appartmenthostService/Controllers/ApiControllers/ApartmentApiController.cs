using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class ApartmentApiController : ApiController
    {
        private readonly apartmenthostContext _context = new apartmenthostContext();
        public ApiServices Services { get; set; }
        // POST api/Apartment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("api/Apartment")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage PostApartment(ApartmentDTO apartment)
        {
            try
            {
                var respList = new List<string>();
                ResponseDTO resp;
                // Check Apartment is not NULL 
                if (apartment == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NULL));


                // Check Apartment Name is not NULL
                resp = CheckHelper.isNull(apartment.Name, "Name", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Apartment Adress is not NULL
                resp = CheckHelper.isNull(apartment.Adress, "Adress", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);


                // Check Apartment Type is not NULL
                resp = CheckHelper.isNull(apartment.Type, "Type", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Apartment Type Dictionary
                resp = CheckHelper.isValidDicItem(_context, apartment.Type, ConstDictionary.ApartmentType, "Type",
                    RespH.SRV_APARTMENT_INVALID_DICITEM);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

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

                // Generate 
                var apartmentGuid = Guid.NewGuid().ToString();
                _context.Set<Apartment>().Add(new Apartment
                {
                    Id = apartmentGuid,
                    Name = apartment.Name,
                    Type = apartment.Type,
                    Options = apartment.Options,
                    UserId = account.UserId,
                    Adress = apartment.Adress,
                    Latitude = apartment.Latitude,
                    Longitude = apartment.Longitude,
                    Lang = apartment.Lang
                });


                _context.SaveChanges();
                respList.Add(apartmentGuid);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.InnerException.ToString()}));
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
                ResponseDTO resp;
                // Check Apartment is not NULL 
                if (apartment == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NULL));

                // Check Current Apartment is not NULL
                var apartmentCurrent = _context.Apartments.SingleOrDefault(a => a.Id == id);
                if (apartmentCurrent == null)
                {
                    respList.Add(id);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_APARTMENT_NOTFOUND, respList));
                }

                // Check Apartment Name is not NULL
                resp = CheckHelper.isNull(apartment.Name, "Name", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Apartment Adress is not NULL
                resp = CheckHelper.isNull(apartment.Adress, "Adress", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);


                // Check Apartment Type is not NULL
                resp = CheckHelper.isNull(apartment.Type, "Type", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Apartment Type Dictionary
                resp = CheckHelper.isValidDicItem(_context, apartment.Type, ConstDictionary.ApartmentType, "Type",
                    RespH.SRV_APARTMENT_INVALID_DICITEM);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

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

                // Check Apartment User
                if (apartment.UserId != account.UserId)
                {
                    respList.Add(apartment.UserId);
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_APARTMENT_WRONG_USER, respList));
                }
                if (apartmentCurrent.UserId != account.UserId)
                {
                    respList.Add(apartmentCurrent.UserId);
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_APARTMENT_WRONG_USER, respList));
                }

                // Update Apartment
                apartmentCurrent.Name = apartment.Name;
                apartmentCurrent.Adress = apartment.Adress;
                apartmentCurrent.Type = apartment.Type;
                apartmentCurrent.Options = apartment.Options;
                apartmentCurrent.Latitude = apartment.Latitude;
                apartmentCurrent.Longitude = apartment.Longitude;
                apartmentCurrent.Lang = apartment.Lang;

                _context.SaveChanges();

                respList.Add(apartment.Id);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.InnerException.ToString()}));
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
                var apartment = _context.Apartments.SingleOrDefault(a => a.Id == id);

                // Check Apartment is not NULL
                if (apartment == null)
                {
                    respList.Add(id);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_APARTMENT_NOTFOUND, respList));
                }

                // Check Current User
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_UNAUTH));
                var account = AuthUtils.GetUserAccount(_context, currentUser);
                if (account == null)
                {
                    respList.Add(currentUser.Id);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                        RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                // Check Apartment User
                if (apartment.UserId != account.UserId)
                {
                    respList.Add(apartment.UserId);
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_APARTMENT_WRONG_USER, respList));
                }

                // Check Adverts with such Apartment
                var adverts = _context.Cards.Where(adv => adv.ApartmentId == id).Select(a => a.Id);
                if (adverts.Any())
                {
                    foreach (var advert in adverts)
                    {
                        respList.Add(advert);
                    }
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_APARTMENT_DEPENDENCY, respList));
                }

                // Delete Apartment with PropVals
                _context.Apartments.Remove(apartment);
                _context.SaveChanges();
                respList.Add(apartment.Id);
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