using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using LinqKit;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class AdvertApiController : ApiController
    {
        public ApiServices Services { get; set; }
        private readonly apartmenthostContext _context = new apartmenthostContext();

        //[Route("api/Advert")]
        //[HttpGet]
        //public HttpResponseMessage GetAdvert()
        //{

        //    var dic = new Dictionary<string, string>();
        //    var propsvals = context.PropVals.Where(p => p.AdvertItemId == "a1").Select(appdto => new PropValDTO()
        //              {

        //                  Name = appdto.Prop.Name,

        //                  DictionaryItem = new DictionaryItemDTO()
        //                  {
        //                      StrValue = appdto.DictionaryItem.StrValue,
        //                  }
        //              });


        //   var multiprops = propsvals.GroupBy(p => p.Name)
        //        .Where(g => g.Count() > 1)
        //        .Select(y => y.Key)
        //        .ToList();

        //   var singleprops = propsvals.GroupBy(p => p.Name)
        //       .Where(g => g.Count() == 1)
        //       .Select(y => y.Key)
        //       .ToList();
        //    var vals = new List<string>();
        //    foreach (var prop in multiprops)
        //    {
        //        var pvs = propsvals.Where(p => p.Name == prop);
        //        foreach (var pv in pvs)
        //        {
        //           vals.Add(pv.DictionaryItem.StrValue); 
        //        }
        //        dic.Add(prop,JsonConvert.SerializeObject(vals));
        //    }

        //    foreach (var prop in singleprops)
        //    {
        //        var pv = propsvals.SingleOrDefault(p => p.Name == prop);
        //        dic.Add(prop, pv.DictionaryItem.StrValue);
        //    }
        //    //foreach (var propval in propsvals)
        //    //{
        //    //   dic.Add(propval.Name, propval.DictionaryItem.StrValue); 
        //    //}
        //    var resp = JsonConvert.SerializeObject(dic, Formatting.Indented);
        //    return this.Request.CreateResponse(HttpStatusCode.OK, resp); 
        //}
        /// <summary>
        /// POST api/Advert/48D68C86-6EA6-4C25-AA33-223FC9A27959
        /// </summary>
        [Route("api/Advert")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage PostAdvert(AdvertDTO advert)
        {
            try
            {
                var respList = new List<string>();
                ResponseDTO resp;

                // Check Advert is not NULL 
                if (advert == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_NULL));

                // Check Advert Name is not NULL
                resp = CheckHelper.isNull(advert.Name, "Name", RespH.SRV_ADVERT_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Advert not Already Exists
                resp = CheckHelper.isAdvertExist(_context, advert.Name, RespH.SRV_ADVERT_EXISTS);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Advert Cohabitation is not null
                //resp = CheckHelper.isNull(advert.Cohabitation, "Cohabitation", RespH.SRV_ADVERT_REQUIRED);
                //if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);
                if (advert.Cohabitation == null) advert.Cohabitation = ConstVals.Any;
                // Check Advert Cohabitation Dictionary
                //resp = CheckHelper.isValidDicItem(context, advert.Cohabitation, ConstDictionary.Cohabitation, "Cohabitation", RespH.SRV_ADVERT_INVALID_DICITEM);
                //if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Advert Resident Gender is not null
                resp = CheckHelper.isNull(advert.ResidentGender, "ResidentGender", RespH.SRV_ADVERT_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Advert Resident Gender Dictionary
                resp = CheckHelper.isValidDicItem(_context, advert.ResidentGender, ConstDictionary.Gender, "ResidentGender", RespH.SRV_ADVERT_INVALID_DICITEM);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Dates
                resp = CheckHelper.isValidDates(advert.DateFrom, advert.DateTo, RespH.SRV_ADVERT_WRONG_DATE);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Current User
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
                var account = AuthUtils.GetUserAccount(_context, currentUser);
                if (account == null)
                {
                    respList.Add(currentUser.Id);
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                //Apartment
                if (advert.Apartment == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NULL));

                // Check Apartment Adress is not NULL
                resp = CheckHelper.isNull(advert.Apartment.Adress, "Adress", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Apartment Type is not NULL
                resp = CheckHelper.isNull(advert.Apartment.Type, "Type", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Apartment Type Dictionary
                resp = CheckHelper.isValidDicItem(_context, advert.Apartment.Type, ConstDictionary.ApartmentType, "Type", RespH.SRV_APARTMENT_INVALID_DICITEM);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Generate 
                string advertGuid = Guid.NewGuid().ToString();
                string apartmentGuid = Guid.NewGuid().ToString();
                _context.Set<Advert>().Add(new Advert()
                {
                    Id = advertGuid,
                    Name = advert.Name,
                    UserId = account.UserId,
                    Description = advert.Description,
                    ApartmentId = apartmentGuid,
                    DateFrom = advert.DateFrom,
                    DateTo = advert.DateTo,
                    PriceDay = advert.PriceDay,
                    PricePeriod = advert.PricePeriod,
                    Cohabitation = advert.Cohabitation,
                    ResidentGender = advert.ResidentGender,
                    Lang = advert.Lang,
                    Apartment = new Apartment()
                    {
                        Id = apartmentGuid,
                        Name = advert.Name,
                        Type = advert.Apartment.Type,
                        Options = advert.Apartment.Options,
                        UserId = account.UserId,
                        Adress = advert.Apartment.Adress,
                        Latitude = advert.Apartment.Latitude,
                        Longitude = advert.Apartment.Longitude,
                        Lang = advert.Lang

                    }
                });

                _context.SaveChanges();
                respList.Add(advertGuid);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }


        /// <summary>
        /// PUT api/Advert/48D68C86-6EA6-4C25-AA33-223FC9A27959
        /// </summary>
        /// <param name="id">The ID of the Advert.</param>
        /// <param name="advert">The Advert changed object.</param>
        [Route("api/Advert/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPut]
        public HttpResponseMessage PutAdvert(string id, AdvertDTO advert)
        {
            try
            {
                var respList = new List<string>();
                ResponseDTO resp;
                // Check Advert is not NULL 
                if (advert == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_NULL));

                // Check Current Advert is not NULL
                var advertCurrent = _context.Adverts.SingleOrDefault(a => a.Id == id);
                if (advertCurrent == null)
                {
                    respList.Add(id);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_NOTFOUND, respList));
                }

                // Check Current User
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
                var account = AuthUtils.GetUserAccount(_context, currentUser);
                if (account == null)
                {
                    respList.Add(currentUser.Id);
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                // Check Advert User
                if (advertCurrent.UserId != account.UserId)
                {
                    respList.Add(advertCurrent.UserId);
                    respList.Add(account.UserId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_ADVERT_WRONG_USER, respList));
                }

                // Check Advert Name is not NULL
                resp = CheckHelper.isNull(advert.Name, "Name", RespH.SRV_ADVERT_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Advert not Already Exists
                resp = CheckHelper.isAdvertExist(_context, advert.Name, RespH.SRV_ADVERT_EXISTS);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Advert Cohabitation is not null
                //resp = CheckHelper.isNull(advert.Cohabitation, "Cohabitation", RespH.SRV_ADVERT_REQUIRED);
                //if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);
                if (advert.Cohabitation == null) advert.Cohabitation = ConstVals.Any;
                // Check Advert Cohabitation Dictionary
                //resp = CheckHelper.isValidDicItem(context, advert.Cohabitation, ConstDictionary.Cohabitation, "Cohabitation", RespH.SRV_ADVERT_INVALID_DICITEM);
                //if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Advert Resident Gender is not null
                resp = CheckHelper.isNull(advert.ResidentGender, "ResidentGender", RespH.SRV_ADVERT_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Advert Resident Gender Dictionary
                resp = CheckHelper.isValidDicItem(_context, advert.ResidentGender, ConstDictionary.Gender, "ResidentGender", RespH.SRV_ADVERT_INVALID_DICITEM);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Dates
                resp = CheckHelper.isValidDates(advert.DateFrom, advert.DateTo, RespH.SRV_ADVERT_WRONG_DATE);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                //Apartment
                if (advert.Apartment == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NULL));

                // Check Apartment Exists
                if (String.IsNullOrWhiteSpace(advert.ApartmentId))
                {
                    respList.Add("ApartmentId");
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_REQUIRED, respList));
                }
                var apartment = _context.Apartments.SingleOrDefault(a => a.Id == advert.ApartmentId);
                if (apartment == null)
                {
                    respList.Add(advert.ApartmentId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NOTFOUND, respList));
                }

                // Check Apartment Adress is not NULL
                resp = CheckHelper.isNull(advert.Apartment.Adress, "Adress", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Apartment Type is not NULL
                resp = CheckHelper.isNull(advert.Apartment.Type, "Type", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Apartment Type Dictionary
                resp = CheckHelper.isValidDicItem(_context, advert.Apartment.Type, ConstDictionary.ApartmentType, "Type", RespH.SRV_APARTMENT_INVALID_DICITEM);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Update Advert
                advertCurrent.Name = advert.Name;
                advertCurrent.Description = advert.Description;
                advertCurrent.DateFrom = advert.DateFrom;
                advertCurrent.DateTo = advert.DateTo;
                advertCurrent.PriceDay = advertCurrent.PriceDay;
                advertCurrent.PricePeriod = advertCurrent.PricePeriod;
                advertCurrent.Cohabitation = advertCurrent.Cohabitation;
                advertCurrent.ResidentGender = advertCurrent.ResidentGender;

                // Update Apartment
                advertCurrent.Apartment.Name = advert.Name;
                advertCurrent.Apartment.Type = apartment.Type;
                advertCurrent.Apartment.Options = apartment.Options;
                advertCurrent.Apartment.Adress = apartment.Adress;
                advertCurrent.Apartment.Latitude = apartment.Latitude;
                advertCurrent.Apartment.Longitude = apartment.Longitude;
                advertCurrent.Apartment.Lang = apartment.Lang;

                _context.SaveChanges();

                respList.Add(advert.Id);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED, respList));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }

        /// <summary>
        /// DELETE api/Advert/48D68C86-6EA6-4C25-AA33-223FC9A27959
        /// </summary>
        /// <param name="id">The ID of the Advert.</param>
        [Route("api/Advert/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpDelete]
        public HttpResponseMessage DeleteAdvert(string id)
        {
            try
            {
                var respList = new List<string>();
                var advert = _context.Adverts.SingleOrDefault(a => a.Id == id);

                // Check Advert is not NULL
                if (advert == null)
                {
                    respList.Add(id);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_NOTFOUND, respList));
                }

                // Check Current User
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
                var account = AuthUtils.GetUserAccount(_context, currentUser);
                if (account == null)
                {
                    respList.Add(currentUser.Id);
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                // Check Advert User
                if (advert.UserId != account.UserId)
                {
                    respList.Add(advert.UserId);
                    respList.Add(account.UserId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_ADVERT_WRONG_USER, respList));
                }

                var apartment = _context.Apartments.SingleOrDefault(a => a.Id == advert.ApartmentId);

                // Check Apartment is not NULL
                if (apartment == null)
                {
                    respList.Add(id);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NOTFOUND, respList));
                }
                // Delete Advert
                _context.Adverts.Remove(advert);
                _context.SaveChanges();
                _context.Apartments.Remove(apartment);
                _context.SaveChanges();
                respList.Add(advert.Id);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_DELETED, respList));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }
    }
}
