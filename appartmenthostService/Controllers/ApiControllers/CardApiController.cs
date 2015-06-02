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
    public class CardApiController : ApiController
    {
        public ApiServices Services { get; set; }
        private readonly apartmenthostContext _context = new apartmenthostContext();

        //[Route("api/Card")]
        //[HttpGet]
        //public HttpResponseMessage GetCard()
        //{

        //    var dic = new Dictionary<string, string>();
        //    var propsvals = context.PropVals.Where(p => p.CardItemId == "a1").Select(appdto => new PropValDTO()
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
        /// POST api/Card/48D68C86-6EA6-4C25-AA33-223FC9A27959
        /// </summary>
        [Route("api/Card")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage PostCard(CardDTO card)
        {
            try
            {
                var respList = new List<string>();
                ResponseDTO resp;

                // Check Card is not NULL 
                if (card == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_CARD_NULL));

                // Check CARD Name is not NULL
                resp = CheckHelper.isNull(card.Name, "Name", RespH.SRV_CARD_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check CARD not Already Exists
                resp = CheckHelper.isCardExist(_context, card.Name, RespH.SRV_CARD_EXISTS);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check CARD Cohabitation is not null
                //resp = CheckHelper.isNull(CARD.Cohabitation, "Cohabitation", RespH.SRV_CARD_REQUIRED);
                //if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);
                if (card.Cohabitation == null) card.Cohabitation = ConstVals.Any;
                // Check CARD Cohabitation Dictionary
                //resp = CheckHelper.isValidDicItem(context, CARD.Cohabitation, ConstDictionary.Cohabitation, "Cohabitation", RespH.SRV_CARD_INVALID_DICITEM);
                //if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check CARD Resident Gender is not null
                resp = CheckHelper.isNull(card.ResidentGender, "ResidentGender", RespH.SRV_CARD_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check CARD Resident Gender Dictionary
                //resp = CheckHelper.isValidDicItem(_context, card.ResidentGender, ConstDictionary.Gender, "ResidentGender", RespH.SRV_CARD_INVALID_DICITEM);
                //if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Dates
                resp = CheckHelper.isValidDates(card.DateFrom, card.DateTo, RespH.SRV_CARD_WRONG_DATE);
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
                if (card.Apartment == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NULL));

                // Check Apartment Adress is not NULL
                resp = CheckHelper.isNull(card.Apartment.Adress, "Adress", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Apartment Type is not NULL
                resp = CheckHelper.isNull(card.Apartment.Type, "Type", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Apartment Type Dictionary
                resp = CheckHelper.isValidDicItem(_context, card.Apartment.Type, ConstDictionary.ApartmentType, "Type", RespH.SRV_APARTMENT_INVALID_DICITEM);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Generate 
                string cardGuid = Guid.NewGuid().ToString();
                string apartmentGuid = Guid.NewGuid().ToString();
                _context.Set<Card>().Add(new Card()
                {
                    Id = cardGuid,
                    Name = card.Name,
                    UserId = account.UserId,
                    Description = card.Description,
                    ApartmentId = apartmentGuid,
                    DateFrom = card.DateFrom,
                    DateTo = card.DateTo,
                    PriceDay = card.PriceDay,
                    Cohabitation = card.Cohabitation,
                    ResidentGender = card.ResidentGender,
                    Lang = card.Lang,
                    Apartment = new Apartment()
                    {
                        Id = apartmentGuid,
                        Name = card.Name,
                        Type = card.Apartment.Type,
                        Options = card.Apartment.Options,
                        UserId = account.UserId,
                        Adress = card.Apartment.Adress,
                        Latitude = card.Apartment.Latitude,
                        Longitude = card.Apartment.Longitude,
                        Lang = card.Lang

                    }
                });

                _context.SaveChanges();
                respList.Add(cardGuid);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }


        /// <summary>
        /// PUT api/Card/48D68C86-6EA6-4C25-AA33-223FC9A27959
        /// </summary>
        /// <param name="id">The ID of the Card.</param>
        /// <param name="card">The CARD changed object.</param>
        [Route("api/Card/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPut]
        public HttpResponseMessage PutCard(string id, CardDTO card)
        {
            try
            {
                var respList = new List<string>();
                ResponseDTO resp;
                // Check CARD is not NULL 
                if (card == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_CARD_NULL));

                // Check Current CARD is not NULL
                var cardCurrent = _context.Cards.SingleOrDefault(a => a.Id == id);
                if (cardCurrent == null)
                {
                    respList.Add(id);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_CARD_NOTFOUND, respList));
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

                // Check CARD User
                if (cardCurrent.UserId != account.UserId)
                {
                    respList.Add(cardCurrent.UserId);
                    respList.Add(account.UserId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_CARD_WRONG_USER, respList));
                }

                // Check CARD Name is not NULL
                resp = CheckHelper.isNull(card.Name, "Name", RespH.SRV_CARD_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check CARD not Already Exists
                resp = CheckHelper.isCardExist(_context, card.Name, RespH.SRV_CARD_EXISTS);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check CARD Cohabitation is not null
                //resp = CheckHelper.isNull(CARD.Cohabitation, "Cohabitation", RespH.SRV_CARD_REQUIRED);
                //if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);
                if (card.Cohabitation == null) card.Cohabitation = ConstVals.Any;
                // Check CARD Cohabitation Dictionary
                //resp = CheckHelper.isValidDicItem(context, CARD.Cohabitation, ConstDictionary.Cohabitation, "Cohabitation", RespH.SRV_CARD_INVALID_DICITEM);
                //if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check CARD Resident Gender is not null
                resp = CheckHelper.isNull(card.ResidentGender, "ResidentGender", RespH.SRV_CARD_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check CARD Resident Gender Dictionary
                //resp = CheckHelper.isValidDicItem(_context, card.ResidentGender, ConstDictionary.Gender, "ResidentGender", RespH.SRV_CARD_INVALID_DICITEM);
                //if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Dates
                resp = CheckHelper.isValidDates(card.DateFrom, card.DateTo, RespH.SRV_CARD_WRONG_DATE);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                //Apartment
                if (card.Apartment == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NULL));

                // Check Apartment Exists
                if (String.IsNullOrWhiteSpace(card.ApartmentId))
                {
                    respList.Add("ApartmentId");
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_CARD_REQUIRED, respList));
                }
                var apartment = _context.Apartments.SingleOrDefault(a => a.Id == card.ApartmentId);
                if (apartment == null)
                {
                    respList.Add(card.ApartmentId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NOTFOUND, respList));
                }

                // Check Apartment Adress is not NULL
                resp = CheckHelper.isNull(card.Apartment.Adress, "Adress", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Apartment Type is not NULL
                resp = CheckHelper.isNull(card.Apartment.Type, "Type", RespH.SRV_APARTMENT_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Apartment Type Dictionary
                resp = CheckHelper.isValidDicItem(_context, card.Apartment.Type, ConstDictionary.ApartmentType, "Type", RespH.SRV_APARTMENT_INVALID_DICITEM);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Update CARD
                cardCurrent.Name = card.Name;
                cardCurrent.Description = card.Description;
                cardCurrent.DateFrom = card.DateFrom;
                cardCurrent.DateTo = card.DateTo;
                cardCurrent.PriceDay = cardCurrent.PriceDay;
                cardCurrent.Cohabitation = cardCurrent.Cohabitation;
                cardCurrent.ResidentGender = cardCurrent.ResidentGender;

                // Update Apartment
                cardCurrent.Apartment.Name = card.Name;
                cardCurrent.Apartment.Type = apartment.Type;
                cardCurrent.Apartment.Options = apartment.Options;
                cardCurrent.Apartment.Adress = apartment.Adress;
                cardCurrent.Apartment.Latitude = apartment.Latitude;
                cardCurrent.Apartment.Longitude = apartment.Longitude;
                cardCurrent.Apartment.Lang = apartment.Lang;

                _context.SaveChanges();

                respList.Add(card.Id);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }

        /// <summary>
        /// DELETE api/CARD/48D68C86-6EA6-4C25-AA33-223FC9A27959
        /// </summary>
        /// <param name="id">The ID of the Card.</param>
        [Route("api/Card/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpDelete]
        public HttpResponseMessage DeleteCard(string id)
        {
            try
            {
                var respList = new List<string>();
                var card = _context.Cards.SingleOrDefault(a => a.Id == id);

                // Check Card is not NULL
                if (card == null)
                {
                    respList.Add(id);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_CARD_NOTFOUND, respList));
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

                // Check CARD User
                if (card.UserId != account.UserId)
                {
                    respList.Add(card.UserId);
                    respList.Add(account.UserId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_CARD_WRONG_USER, respList));
                }

                var apartment = _context.Apartments.SingleOrDefault(a => a.Id == card.ApartmentId);

                // Check Apartment is not NULL
                if (apartment == null)
                {
                    respList.Add(id);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_APARTMENT_NOTFOUND, respList));
                }
                // Delete CARD
                _context.Cards.Remove(card);
                _context.SaveChanges();
                _context.Apartments.Remove(apartment);
                _context.SaveChanges();
                respList.Add(card.Id);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_DELETED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }
    }
}
