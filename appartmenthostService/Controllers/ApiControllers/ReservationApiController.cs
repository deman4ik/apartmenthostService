using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using Itenso.TimePeriod;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class ReservationApiController : ApiController
    {
        public ApiServices Services { get; set; }
        private readonly apartmenthostContext _context = new apartmenthostContext();

        [Route("api/Reservations")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpGet]
        public HttpResponseMessage GetReservations()
        {
            try
            {
                var respList = new List<string>();
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


                var ownerReserv = _context.Reservations.Where(x => x.Card.UserId == account.UserId).Select(r => new ReservationDTO
                {
                    Id = r.Id,
                    Type = ConstVals.Owner,
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
                        RatingCount = r.User.Profile.RatingCount,
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
                        PricePeriod = r.Card.PriceDay * 7,
                        Cohabitation = r.Card.Cohabitation,
                        ResidentGender = r.Card.ResidentGender,
                        Lang = r.Card.Lang,
                        User = new UserDTO()
                        {
                            Id = r.Card.User.Profile.Id,
                            FirstName = r.Card.User.Profile.FirstName,
                            LastName = r.Card.User.Profile.LastName,
                            Rating = r.Card.User.Profile.Rating,
                            RatingCount = r.Card.User.Profile.RatingCount,
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
                }).ToList();

                var renterReserv = _context.Reservations.Where(x => x.UserId == account.UserId).Select(r => new ReservationDTO
                {
                    Id = r.Id,
                    Type = ConstVals.Renter,
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
                        RatingCount = r.User.Profile.RatingCount,
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
                        PricePeriod = r.Card.PriceDay * 7,
                        Cohabitation = r.Card.Cohabitation,
                        ResidentGender = r.Card.ResidentGender,
                        Lang = r.Card.Lang,
                        User = new UserDTO()
                        {
                            Id = r.Card.User.Profile.Id,
                            FirstName = r.Card.User.Profile.FirstName,
                            LastName = r.Card.User.Profile.LastName,
                            Rating = r.Card.User.Profile.Rating,
                            RatingCount = r.Card.User.Profile.RatingCount,
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
                }).ToList();

                List<ReservationDTO> result = new List<ReservationDTO>(ownerReserv.Count + renterReserv.Count);
                result.AddRange(ownerReserv);
                result.AddRange(renterReserv);
                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.ToString() }));
            }
        }

        [Route("api/Reservation/Make/{cardId}/{dateFrom:datetime:regex(\\d{4}-\\d{2}-\\d{2})}/{dateTo:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage MakeReservation(string cardId, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                var respList = new List<string>();

                // Check Reservation is not NULL 
                if (cardId == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_RESERVATION_NULL));

                var card = _context.Cards.SingleOrDefault(a => a.Id == cardId);
                // Check CARD is not NULL 
                if (card == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_CARD_NULL));

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
                // Check Reservation already exists
                var existedReservation = _context.Reservations.SingleOrDefault(x => x.UserId == account.UserId && x.CardId == cardId);
                if (existedReservation != null)
                {
                    respList.Add(existedReservation.Id);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_RESERVATION_EXISTS, respList));
                }

                // Check CARD User
                //if (CARD.UserId != account.UserId)
                //{
                //    respList.Add(CARD.UserId);
                //    respList.Add(account.UserId);
                //    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                //        RespH.Create(RespH.SRV_CARD_WRONG_USER, respList));
                //}

                // Check Dates
                if (DateTime.Compare(dateFrom, dateTo) >= 0)
                {
                    respList.Add(dateFrom.ToLocalTime().ToString(CultureInfo.InvariantCulture));
                    respList.Add(dateTo.ToLocalTime().ToString(CultureInfo.InvariantCulture));
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_CARD_WRONG_DATE, respList));

                }

                // Check Available Dates
                TimeRange reservationDates = new TimeRange(dateFrom, dateTo);

                TimeRange unavailableDates = new TimeRange(card.DateFrom, card.DateTo);
                if (unavailableDates.IntersectsWith(reservationDates))
                {
                    respList.Add(reservationDates.ToString());
                    respList.Add(unavailableDates.ToString());
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_RESERVATION_UNAVAILABLE_DATE, respList));
                }
                var currentReservations = _context.Reservations.Where(r => r.CardId == cardId && r.Status == ConstVals.Accepted);
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
                _context.Set<Reservation>().Add(new Reservation()
                {
                    Id = reservationGuid,
                    CardId = cardId,
                    UserId = account.UserId,
                    Status = ConstVals.Pending,
                    DateFrom = dateFrom,
                    DateTo = dateTo
                });

                // Create Notification
                _context.Set<Notification>().Add(new Notification()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = card.UserId,
                    Type = ConstVals.General,
                    ReservationId = reservationGuid,
                    Code = RespH.SRV_NOTIF_RESERV_PENDING,
                    Readed = false
                });

                _context.SaveChanges();
                respList.Add(reservationGuid);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));

            }
            catch (Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }


        [Route("api/Reservation/AcceptDecline/{reservId}/{status}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage AcceptDeclineReservation(string reservId, string status)
        {
            try
            {
                var respList = new List<string>();

                // Check status is not NULL 
                if (status == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_RESERVATION_NULL));

                // Check Status
                if (status != ConstVals.Accepted && status != ConstVals.Declined)
                {
                    respList.Add(status);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_RESERVATION_WRONG_STATUS, respList));
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

                //Check Reservation
                var currentReservation = _context.Reservations.SingleOrDefault(r => r.Id == reservId);
                // Check Reservation is not NULL 
                if (currentReservation == null)
                {
                    respList.Add(reservId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_RESERVATION_NOTFOUND, respList));
                }

                var card = _context.Cards.SingleOrDefault(a => a.Id == currentReservation.CardId);
                // Check CARD is not NULL 
                if (card == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_CARD_NULL));


                // Check CARD User
                if (card.UserId != account.UserId)
                {
                    respList.Add(card.UserId);
                    respList.Add(account.UserId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_CARD_WRONG_USER, respList));
                }


                string notifCode;
                //Check status
                if (status == ConstVals.Accepted)
                {
                    // Check Available Dates
                    TimeRange reservationDates = new TimeRange(currentReservation.DateFrom, currentReservation.DateTo);

                    TimeRange unavailableDates = new TimeRange(card.DateFrom, card.DateTo);
                    if (unavailableDates.IntersectsWith(reservationDates))
                    {
                        respList.Add(reservationDates.ToString());
                        respList.Add(unavailableDates.ToString());
                        return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                            RespH.Create(RespH.SRV_RESERVATION_UNAVAILABLE_DATE, respList));
                    }
                    var currentReservations = _context.Reservations.Where(r => r.CardId == currentReservation.CardId && currentReservation.Status == ConstVals.Accepted);
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
                    notifCode = RespH.SRV_NOTIF_RESERV_ACCEPTED;
                }
                else
                {
                    notifCode = RespH.SRV_NOTIF_RESERV_DECLINED;
                }

                currentReservation.Status = status;

                // Create Notification
                _context.Set<Notification>().Add(new Notification()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = currentReservation.UserId,
                    Type = ConstVals.General,
                    ReservationId = currentReservation.Id,
                    Code = notifCode,
                    Readed = false
                });

                _context.SaveChanges();

                respList.Add(reservId);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED, respList));

            }
            catch (Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }

    }
}
