using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using Itenso.TimePeriod;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    public class ReservationApiController : ApiController
    {
        public ApiServices Services { get; set; }
        private apartmenthostContext context = new apartmenthostContext();

        [Route("api/Reservation/Make")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage MakeReservation(ReservationDTO reservation)
        {
            try
            {
                var respList = new List<string>();

                // Check Reservation is not NULL 
                if (reservation == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_RESERVATION_NULL));

                var advert = context.Adverts.SingleOrDefault(a => a.Id == reservation.AdvertId);
                // Check Advert is not NULL 
                if (advert == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_NULL));

                // Check Current User
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
                var account = AuthUtils.GetUserAccount(currentUser);
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

                // Check Dates
                if (DateTime.Compare(reservation.DateFrom, reservation.DateTo) >= 0)
                {
                    respList.Add(reservation.DateFrom.ToLocalTime().ToString());
                    respList.Add(reservation.DateTo.ToLocalTime().ToString());
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_WRONG_DATE, respList));

                }

                // Check Available Dates
                TimeRange reservationDates = new TimeRange(reservation.DateFrom,reservation.DateTo);

                TimeRange unavailableDates = new TimeRange(advert.DateFrom, advert.DateTo);
                if (unavailableDates.IntersectsWith(reservationDates))
                {
                    respList.Add(reservationDates.ToString());
                    respList.Add(unavailableDates.ToString());
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_RESERVATION_UNAVAILABLE_DATE, respList));
                }
                var currentReservations = context.Reservations.Where(r => r.AdvertId == reservation.AdvertId && reservation.Status == ConstVals.Accepted);
                foreach (var currentReservation in currentReservations)
                {
                    TimeRange reservedDates = new TimeRange(currentReservation.DateFrom, currentReservation.DateTo);
                    if (reservedDates.IntersectsWith(reservationDates))
                    {
                        respList.Add(reservationDates.ToString());
                        respList.Add(reservedDates.ToString());
                        return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                            RespH.Create(RespH.SRV_RESERVATION_UNAVAILABLE_DATE, respList));
                    }
                }

                string reservationGuid = Guid.NewGuid().ToString();
                context.Set<Reservation>().Add(new Reservation()
                {
                    Id = reservationGuid,
                    AdvertId = reservation.AdvertId,
                    UserId = account.UserId,
                    Status = ConstVals.Pending,
                    DateFrom = reservation.DateFrom,
                    DateTo = reservation.DateTo
                });

                context.SaveChanges();
                respList.Add(reservationGuid);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }

       
        [Route("api/Reservation/AcceptDecline/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage AcceptDeclineReservation(string id, string status)
        {
            try
            {
                var respList = new List<string>();

                // Check status is not NULL 
                if (status == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_RESERVATION_NULL));

                // Check Status
                if (status != ConstVals.Accepted || status != ConstVals.Declined)
                {
                    respList.Add(status);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_RESERVATION_WRONG_STATUS, respList));
                }

                // Check Current User
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
                var account = AuthUtils.GetUserAccount(currentUser);
                if (account == null)
                {
                    respList.Add(currentUser.Id);
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                //Check Reservation
                var currentReservation = context.Reservations.SingleOrDefault(r => r.Id == id);
                // Check Reservation is not NULL 
                if (currentReservation == null)
                {
                    respList.Add(id);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_RESERVATION_NOTFOUND, respList));
                }

                var advert = context.Adverts.SingleOrDefault(a => a.Id == currentReservation.AdvertId);
                // Check Advert is not NULL 
                if (advert == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_NULL));


                // Check Advert User
                if (advert.UserId != account.UserId)
                {
                    respList.Add(advert.UserId);
                    respList.Add(account.UserId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_ADVERT_WRONG_USER, respList));
                }
                
                

                //Check status
                if (status == ConstVals.Accepted)
                {
                    // Check Available Dates
                    TimeRange reservationDates = new TimeRange(currentReservation.DateFrom, currentReservation.DateTo);

                    TimeRange unavailableDates = new TimeRange(advert.DateFrom, advert.DateTo);
                    if (unavailableDates.IntersectsWith(reservationDates))
                    {
                        respList.Add(reservationDates.ToString());
                        respList.Add(unavailableDates.ToString());
                        return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                            RespH.Create(RespH.SRV_RESERVATION_UNAVAILABLE_DATE, respList));
                    }
                    var currentReservations = context.Reservations.Where(r => r.AdvertId == currentReservation.AdvertId && currentReservation.Status == ConstVals.Accepted);
                    foreach (var currentReserv in currentReservations)
                    {
                        TimeRange reservedDates = new TimeRange(currentReserv.DateFrom, currentReserv.DateTo);
                        if (reservedDates.IntersectsWith(reservationDates))
                        {
                            respList.Add(reservationDates.ToString());
                            respList.Add(reservedDates.ToString());
                            return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                                RespH.Create(RespH.SRV_RESERVATION_UNAVAILABLE_DATE, respList));
                        }
                    } 
                }

                currentReservation.Status = status;

                context.SaveChanges();

                respList.Add(id);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED, respList));

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
