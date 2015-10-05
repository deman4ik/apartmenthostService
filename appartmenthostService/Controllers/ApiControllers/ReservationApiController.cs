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
        private readonly apartmenthostContext _context = new apartmenthostContext();
        public ApiServices Services { get; set; }

        [Route("api/Reservations/{type?}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpGet]
        public HttpResponseMessage GetReservations(string type = null)
        {
            try
            {
                var respList = new List<string>();
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
                var ownerReserv = new List<ReservationDTO>();
                var renterReserv = new List<ReservationDTO>();
                if (type == ConstVals.Owner || string.IsNullOrWhiteSpace(type))
                {
                    ownerReserv =
                        _context.Reservations.Where(x => x.Card.UserId == account.UserId)
                            .OrderByDescending(r => r.CreatedAt)
                            .Select(r => new ReservationDTO
                            {
                                Id = r.Id,
                                Type = ConstVals.Owner,
                                CardId = r.CardId,
                                UserId = r.UserId,
                                Status = r.Status,
                                Gender = r.Gender,
                                DateFrom = r.DateFrom,
                                DateTo = r.DateTo,
                                CreatedAt = r.CreatedAt,
                                UpdatedAt = r.UpdatedAt,
                                User = new BaseUserDTO
                                {
                                    Id = r.User.Profile.Id,
                                    Email = r.User.Email,
                                    FirstName = r.User.Profile.FirstName,
                                    LastName = r.User.Profile.LastName,
                                    Rating = r.User.Profile.Rating,
                                    RatingCount = r.User.Profile.RatingCount,
                                    Gender = r.User.Profile.Gender,
                                    Picture = new PictureDTO
                                    {
                                        Id = r.User.Profile.Picture.Id,
                                        Name = r.User.Profile.Picture.Name,
                                        Description = r.User.Profile.Picture.Description,
                                        Url = r.User.Profile.Picture.Url,
                                        Xsmall = r.User.Profile.Picture.Xsmall,
                                        Small = r.User.Profile.Picture.Small,
                                        Mid = r.User.Profile.Picture.Mid,
                                        Large = r.User.Profile.Picture.Large,
                                        Xlarge = r.User.Profile.Picture.Xlarge,
                                        Default = r.User.Profile.Picture.Default,
                                        CreatedAt = r.User.Profile.Picture.CreatedAt
                                    }
                                },
                                Card = new CardDTO
                                {
                                    Name = r.Card.Name,
                                    UserId = r.Card.UserId,
                                    Description = r.Card.Description,
                                    ApartmentId = r.Card.ApartmentId,
                                    PriceDay = r.Card.Genders.FirstOrDefault(ge => ge.Name == r.Gender).Price,
                                    Cohabitation = r.Card.Cohabitation,
                                    Lang = r.Card.Lang,
                                    Dates = r.Card.Dates.Select(d => new DatesDTO
                                    {
                                        DateFrom = d.DateFrom,
                                        DateTo = d.DateTo
                                    })
                                        .Union(
                                            r.Card.Reservations.Where(reserv => reserv.Status == ConstVals.Accepted)
                                                .Select(rv => new DatesDTO
                                                {
                                                    DateFrom = rv.DateFrom,
                                                    DateTo = rv.DateTo
                                                }).ToList()).ToList(),
                                    User = new UserDTO
                                    {
                                        Id = r.Card.User.Profile.Id,
                                        FirstName = r.Card.User.Profile.FirstName,
                                        LastName = r.Card.User.Profile.LastName,
                                        Rating = r.Card.User.Profile.Rating,
                                        RatingCount = r.Card.User.Profile.RatingCount,
                                        Gender = r.Card.User.Profile.Gender,
                                        Phone = r.Card.User.Profile.Phone,
                                        Picture = new PictureDTO
                                        {
                                            Id = r.Card.User.Profile.Picture.Id,
                                            Name = r.Card.User.Profile.Picture.Name,
                                            Description = r.Card.User.Profile.Picture.Description,
                                            Url = r.Card.User.Profile.Picture.Url,
                                            Xsmall = r.Card.User.Profile.Picture.Xsmall,
                                            Small = r.Card.User.Profile.Picture.Small,
                                            Mid = r.Card.User.Profile.Picture.Mid,
                                            Large = r.Card.User.Profile.Picture.Large,
                                            Xlarge = r.Card.User.Profile.Picture.Xlarge,
                                            Default = r.Card.User.Profile.Picture.Default,
                                            CreatedAt = r.Card.User.Profile.Picture.CreatedAt
                                        }
                                    },
                                    Apartment = new ApartmentDTO
                                    {
                                        Id = r.Card.Apartment.Id,
                                        Name = r.Card.Apartment.Name,
                                        Type = r.Card.Apartment.Type,
                                        Options = r.Card.Apartment.Options,
                                        UserId = r.Card.Apartment.UserId,
                                        Adress = r.Card.Apartment.Adress,
                                        Latitude = r.Card.Apartment.Latitude,
                                        Longitude = r.Card.Apartment.Longitude
                                    }
                                }
                            }).ToList();
                }
                if (type == ConstVals.Renter || string.IsNullOrWhiteSpace(type))
                {
                    renterReserv =
                        _context.Reservations.Where(x => x.UserId == account.UserId)
                            .OrderByDescending(r => r.CreatedAt)
                            .Select(r => new ReservationDTO
                            {
                                Id = r.Id,
                                Type = ConstVals.Renter,
                                CardId = r.CardId,
                                UserId = r.UserId,
                                Status = r.Status,
                                Gender = r.Gender,
                                DateFrom = r.DateFrom,
                                DateTo = r.DateTo,
                                CreatedAt = r.CreatedAt,
                                UpdatedAt = r.UpdatedAt,
                                User = new BaseUserDTO
                                {
                                    Id = r.User.Profile.Id,
                                    Email = r.User.Email,
                                    FirstName = r.User.Profile.FirstName,
                                    LastName = r.User.Profile.LastName,
                                    Rating = r.User.Profile.Rating,
                                    RatingCount = r.User.Profile.RatingCount,
                                    Gender = r.User.Profile.Gender,
                                    Picture = new PictureDTO
                                    {
                                        Id = r.User.Profile.Picture.Id,
                                        Name = r.User.Profile.Picture.Name,
                                        Description = r.User.Profile.Picture.Description,
                                        Url = r.User.Profile.Picture.Url,
                                        Xsmall = r.User.Profile.Picture.Xsmall,
                                        Small = r.User.Profile.Picture.Small,
                                        Mid = r.User.Profile.Picture.Mid,
                                        Large = r.User.Profile.Picture.Large,
                                        Xlarge = r.User.Profile.Picture.Xlarge,
                                        Default = r.User.Profile.Picture.Default,
                                        CreatedAt = r.User.Profile.Picture.CreatedAt
                                    }
                                },
                                Card = new CardDTO
                                {
                                    Name = r.Card.Name,
                                    UserId = r.Card.UserId,
                                    Description = r.Card.Description,
                                    ApartmentId = r.Card.ApartmentId,
                                    PriceDay = r.Card.Genders.FirstOrDefault(ge => ge.Name == r.Gender).Price,
                                    Cohabitation = r.Card.Cohabitation,
                                    Lang = r.Card.Lang,
                                    Dates = r.Card.Dates.Select(d => new DatesDTO
                                    {
                                        DateFrom = d.DateFrom,
                                        DateTo = d.DateTo
                                    })
                                        .Union(
                                            r.Card.Reservations.Where(reserv => reserv.Status == ConstVals.Accepted)
                                                .Select(rv => new DatesDTO
                                                {
                                                    DateFrom = rv.DateFrom,
                                                    DateTo = rv.DateTo
                                                }).ToList()).ToList(),
                                    User = new UserDTO
                                    {
                                        Id = r.Card.User.Profile.Id,
                                        FirstName = r.Card.User.Profile.FirstName,
                                        LastName = r.Card.User.Profile.LastName,
                                        Rating = r.Card.User.Profile.Rating,
                                        RatingCount = r.Card.User.Profile.RatingCount,
                                        Gender = r.Card.User.Profile.Gender,
                                        Phone = r.Card.User.Profile.Phone,
                                        Picture = new PictureDTO
                                        {
                                            Id = r.Card.User.Profile.Picture.Id,
                                            Name = r.Card.User.Profile.Picture.Name,
                                            Description = r.Card.User.Profile.Picture.Description,
                                            Url = r.Card.User.Profile.Picture.Url,
                                            Xsmall = r.Card.User.Profile.Picture.Xsmall,
                                            Small = r.Card.User.Profile.Picture.Small,
                                            Mid = r.Card.User.Profile.Picture.Mid,
                                            Large = r.Card.User.Profile.Picture.Large,
                                            Xlarge = r.Card.User.Profile.Picture.Xlarge,
                                            Default = r.Card.User.Profile.Picture.Default,
                                            CreatedAt = r.Card.User.Profile.Picture.CreatedAt
                                        }
                                    },
                                    Apartment = new ApartmentDTO
                                    {
                                        Id = r.Card.Apartment.Id,
                                        Name = r.Card.Apartment.Name,
                                        Type = r.Card.Apartment.Type,
                                        Options = r.Card.Apartment.Options,
                                        UserId = r.Card.Apartment.UserId,
                                        Adress = r.Card.Apartment.Adress,
                                        Latitude = r.Card.Apartment.Latitude,
                                        Longitude = r.Card.Apartment.Longitude
                                    }
                                }
                            }).ToList();
                }
                var result = new List<ReservationDTO>(ownerReserv.Count + renterReserv.Count);
                result.AddRange(ownerReserv);
                result.AddRange(renterReserv);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
            }
        }

        [Route(
            "api/Reservation/Make/{cardId}/{dateFrom:datetime:regex(\\d{4}-\\d{2}-\\d{2})}/{dateTo:datetime:regex(\\d{4}-\\d{2}-\\d{2})}"
            )]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage MakeReservation(string cardId, string gender, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                var respList = new List<string>();

                // Check Reservation is not NULL 
                if (cardId == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_RESERVATION_NULL));

                var card = _context.Cards.SingleOrDefault(a => a.Id == cardId);
                // Check CARD is not NULL 
                if (card == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_CARD_NULL));

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
                ResponseDTO resp = CheckHelper.IsProfileFill(_context, account.UserId);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);


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
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_CARD_WRONG_DATE, respList));
                }

                // Check Reservation already exists
                if (
                    _context.Reservations.Any(
                        x =>
                            x.UserId == account.UserId && x.CardId == cardId && x.DateFrom == dateFrom &&
                            x.DateTo == dateTo))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_RESERVATION_EXISTS));
                }

                // Check Available Dates
                var reservationDates = new TimeRange(dateFrom, dateTo);

                var unavailableDates =
                    _context.Dates.Where(x => x.CardId == card.Id)
                        .ToList()
                        .Select(unDate => new TimeRange(unDate.DateFrom, unDate.DateTo))
                        .ToList();

                if (unavailableDates.Any(unavailableDate => unavailableDate.IntersectsWith(reservationDates)))
                {
                    respList.Add(reservationDates.ToString());
                    respList.Add(unavailableDates.ToString());
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_RESERVATION_UNAVAILABLE_DATE, respList));
                }

                var currentReservations =
                    _context.Reservations.Where(r => r.CardId == cardId && r.Status == ConstVals.Accepted);
                foreach (var currentReservation in currentReservations)
                {
                    var reservedDates = new TimeRange(currentReservation.DateFrom, currentReservation.DateTo);
                    if (reservedDates.IntersectsWith(reservationDates))
                    {
                        respList.Add(reservationDates.ToString());
                        respList.Add(reservedDates.ToString());
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            RespH.Create(RespH.SRV_RESERVATION_UNAVAILABLE_DATE, respList));
                    }
                }

                var reservationGuid = Guid.NewGuid().ToString();
                _context.Set<Reservation>().Add(new Reservation
                {
                    Id = reservationGuid,
                    CardId = cardId,
                    UserId = account.UserId,
                    Status = ConstVals.Pending,
                    Gender = gender,
                    DateFrom = dateFrom,
                    DateTo = dateTo
                });
                _context.SaveChanges();
                // Create Notification
                Notifications.Create(_context, card.UserId, ConstVals.General, RespH.SRV_NOTIF_RESERV_PENDING, null,
                    reservationGuid, null, true);


                respList.Add(reservationGuid);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.InnerException.ToString()}));
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
                if (status == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_RESERVATION_NULL));

                // Check Status
                if (status != ConstVals.Accepted && status != ConstVals.Declined)
                {
                    respList.Add(status);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_RESERVATION_WRONG_STATUS, respList));
                }

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
                ResponseDTO resp = CheckHelper.IsProfileFill(_context, account.UserId);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                //Check Reservation
                var currentReservation = _context.Reservations.SingleOrDefault(r => r.Id == reservId);
                // Check Reservation is not NULL 
                if (currentReservation == null)
                {
                    respList.Add(reservId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_RESERVATION_NOTFOUND, respList));
                }

                var card = _context.Cards.SingleOrDefault(a => a.Id == currentReservation.CardId);
                // Check CARD is not NULL 
                if (card == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_CARD_NULL));


                // Check CARD User
                if (card.UserId != account.UserId)
                {
                    respList.Add(card.UserId);
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_CARD_WRONG_USER, respList));
                }


                string notifCode;
                //Check status
                if (status == ConstVals.Accepted)
                {
                    // Check Available Dates
                    var reservationDates = new TimeRange(currentReservation.DateFrom, currentReservation.DateTo);

                    var unavailableDates = new List<TimeRange>();

                    var cardDates = _context.Dates.Where(x => x.CardId == card.Id);
                    if (cardDates.Count() > 0)
                    {
                        foreach (var unDate in cardDates)
                        {
                            unavailableDates.Add(new TimeRange(unDate.DateFrom, unDate.DateTo));
                        }
                        foreach (var unavailableDate in unavailableDates)
                        {
                            if (unavailableDate.IntersectsWith(reservationDates))
                            {
                                respList.Add(reservationDates.ToString());
                                respList.Add(unavailableDates.ToString());
                                return Request.CreateResponse(HttpStatusCode.BadRequest,
                                    RespH.Create(RespH.SRV_RESERVATION_UNAVAILABLE_DATE, respList));
                            }
                        }
                    }
                    var currentReservations =
                        _context.Reservations.Where(
                            r =>
                                r.CardId == currentReservation.CardId && currentReservation.Status == ConstVals.Accepted);
                    foreach (var currentReserv in currentReservations)
                    {
                        var reservedDates = new TimeRange(currentReserv.DateFrom, currentReserv.DateTo);
                        if (reservedDates.IntersectsWith(reservationDates))
                        {
                            respList.Add(reservationDates.ToString());
                            respList.Add(reservedDates.ToString());
                            return Request.CreateResponse(HttpStatusCode.BadRequest,
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
                _context.SaveChanges();
                // Create Notification
                Notifications.Create(_context, currentReservation.UserId, ConstVals.General, notifCode, null,
                    currentReservation.Id, null, true);


                respList.Add(reservId);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED, respList));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.InnerException.ToString()}));
            }
        }
    }
}