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
using Microsoft.Owin.Security;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    public class ReviewApiController : ApiController
    {
        public ApiServices Services { get; set; }
        readonly apartmenthostContext _context = new apartmenthostContext();

        // GET api/Reviews/{type}
        [Route("api/Reviews/{type?}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpGet]
        public HttpResponseMessage GetReviews(string type = null)
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

            List<ReservReviewDTO> ownerReviews = new List<ReservReviewDTO>();
            List<ReservReviewDTO> renterReviews = new List<ReservReviewDTO>();

            if (type == ConstVals.Owner || string.IsNullOrWhiteSpace(type))
            {
                ownerReviews =
                    _context.Reservations.Where(
                        r => r.Card.UserId == account.UserId &&
                    r.Status == ConstVals.Accepted
                    )
                        .Select(
                            x => new ReservReviewDTO()
                            {
                                Type = ConstVals.Owner,
                                Reservation = new ReservationDTO()
                                {
                                    Id = x.Id,
                                    CardId = x.CardId,
                                    UserId = x.UserId,
                                    Status = x.Status,
                                    DateFrom = x.DateFrom,
                                    DateTo = x.DateTo,
                                    CreatedAt = x.CreatedAt,
                                    UpdatedAt = x.UpdatedAt,
                                    User = new BaseUserDTO()
                                    {
                                        Id = x.User.Profile.Id,
                                        Email = x.User.Email,
                                        FirstName = x.User.Profile.FirstName,
                                        LastName = x.User.Profile.LastName,
                                        Rating = x.User.Profile.Rating,
                                        RatingCount = x.User.Profile.RatingCount,
                                        Gender = x.User.Profile.Gender
                                    }
                                },
                                OwnerReview =
                                    x.Reviews.Where(rev => rev.ReservationId == x.Id && rev.FromUserId == currentUser.Id)
                                        .Select(owrev => new ReviewDTO()
                                        {
                                            Id = owrev.Id,
                                            FromUserId = owrev.FromUserId,
                                            ToUserId = owrev.ToUserId,
                                            ReservationId = owrev.ReservationId,
                                            Rating = owrev.Rating,
                                            Text = owrev.Text,
                                            CanResponse = x.DateTo <= DateTime.Now,
                                            CreatedAt = owrev.CreatedAt,
                                            UpdatedAt = owrev.UpdatedAt
                                        }).FirstOrDefault(),
                                RenterReview =
                                    x.Reviews.Where(rev => rev.ReservationId == x.Id && rev.ToUserId == currentUser.Id)
                                        .Select(renrev => new ReviewDTO()
                                        {
                                            Id = renrev.Id,
                                            FromUserId = renrev.FromUserId,
                                            ToUserId = renrev.ToUserId,
                                            ReservationId = renrev.ReservationId,
                                            Rating = renrev.Rating,
                                            Text = renrev.Text,
                                            CanResponse = x.DateTo <= DateTime.Now,
                                            CreatedAt = renrev.CreatedAt,
                                            UpdatedAt = renrev.UpdatedAt
                                        }).FirstOrDefault(),
                            }
                        ).ToList();
            }
            if (type == ConstVals.Renter || string.IsNullOrWhiteSpace(type))
            {
                renterReviews =
                    _context.Reservations.Where(r => r.UserId == account.UserId && r.Status == ConstVals.Accepted)
                        .Select(
                            x => new ReservReviewDTO()
                            {
                                Type = ConstVals.Renter,
                                Reservation = new ReservationDTO()
                                {
                                    Id = x.Id,
                                    CardId = x.CardId,
                                    UserId = x.UserId,
                                    Status = x.Status,
                                    DateFrom = x.DateFrom,
                                    DateTo = x.DateTo,
                                    CreatedAt = x.CreatedAt,
                                    UpdatedAt = x.UpdatedAt,
                                    Card = new CardDTO()
                                    {
                                        Id = x.Card.Id,
                                        Name = x.Card.Name,
                                        UserId = x.Card.UserId,
                                        PriceDay = x.Card.PriceDay,
                                        PricePeriod = x.Card.PriceDay * 7,
                                        IsFavorite = x.Card.Favorites.Any(f => f.UserId == currentUser.Id),
                                        Apartment = new ApartmentDTO()
                                        {
                                            Id = x.Card.Apartment.Id,
                                            Name = x.Card.Apartment.Name,
                                            Adress = x.Card.Apartment.Adress
                                        },
                                        User = new UserDTO()
                                        {
                                            Id = x.Card.User.Id,
                                            Email = x.Card.User.Email,
                                            FirstName = x.Card.User.Profile.FirstName,
                                            LastName = x.Card.User.Profile.LastName,
                                            Rating = x.Card.User.Profile.Rating,
                                            RatingCount = x.Card.User.Profile.RatingCount,
                                            Gender = x.Card.User.Profile.Gender
                                        }


                                    }
                                },
                                OwnerReview =
                                    x.Reviews.Where(rev => rev.ReservationId == x.Id && rev.FromUserId == currentUser.Id)
                                        .Select(owrev => new ReviewDTO()
                                        {
                                            Id = owrev.Id,
                                            FromUserId = owrev.FromUserId,
                                            ToUserId = owrev.ToUserId,
                                            ReservationId = owrev.ReservationId,
                                            Rating = owrev.Rating,
                                            Text = owrev.Text,
                                            CanResponse = x.DateTo <= DateTime.Now,
                                            CreatedAt = owrev.CreatedAt,
                                            UpdatedAt = owrev.UpdatedAt
                                        }).FirstOrDefault(),
                                RenterReview =
                                    x.Reviews.Where(rev => rev.ReservationId == x.Id && rev.ToUserId == currentUser.Id)
                                        .Select(renrev => new ReviewDTO()
                                        {
                                            Id = renrev.Id,
                                            FromUserId = renrev.FromUserId,
                                            ToUserId = renrev.ToUserId,
                                            ReservationId = renrev.ReservationId,
                                            Rating = renrev.Rating,
                                            Text = renrev.Text,
                                            CanResponse = x.DateTo <= DateTime.Now,
                                            CreatedAt = renrev.CreatedAt,
                                            UpdatedAt = renrev.UpdatedAt
                                        }).FirstOrDefault(),
                            }
                        ).ToList();
            }
            List<ReservReviewDTO> result = new List<ReservReviewDTO>(ownerReviews.Count + renterReviews.Count);
            result.AddRange(ownerReviews);
            result.AddRange(renterReviews);
            return this.Request.CreateResponse(HttpStatusCode.OK, result);
        }


        // POST api/Review/{resId}
        [Route("api/Review/{resId}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage PostReview(string resId, ReviewDTO review)
        {
            try
            {
                var respList = new List<string>();
                ResponseDTO resp;

                // Check Card is not NULL 
                if (review == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_REVIEW_NULL));

                // Check Reservation is not NULL
                Reservation reservation = _context.Reservations.SingleOrDefault(x => x.Id == resId);
                if (reservation == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_RESERVATION_NOTFOUND));

                // Check Reservation Status is Accepted
                if (reservation.Status != ConstVals.Accepted)
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_REVIEW_WRONG_RESERV_STATUS));

                // Check Reservation Dates
                if (reservation.DateTo >= DateTime.Now)
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_REVIEW_WRONG_DATE));

                // Check Review Text is not NULL
                resp = CheckHelper.isNull(review.Text, "Text", RespH.SRV_REVIEW_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Review Rating is not NULL
                //resp = CheckHelper.isNull(review.Rating, "Rating", RespH.SRV_REVIEW_REQUIRED);
                //if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

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
                Review newReview = new Review();
                // Set FromUserId
                newReview.FromUserId = account.UserId;

                // Set ToUserId
                if (reservation.UserId == account.UserId)
                {
                    newReview.ToUserId = reservation.Card.UserId;
                }
                if (reservation.Card.UserId == account.UserId)
                {
                    newReview.ToUserId = reservation.UserId;
                }

                // Check Review doesn't already exist
                var currentReview =
                    _context.Reviews.SingleOrDefault(
                        r => r.ReservationId == reservation.Id && r.FromUserId == newReview.FromUserId && r.ToUserId == newReview.ToUserId);
                if (currentReview != null)
                {
                    respList.Add(currentReview.Id);
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_REVIEW_EXISTS, respList));
                }
                string reviewGuid = Guid.NewGuid().ToString();
                newReview.Id = reviewGuid;
                newReview.ReservationId = reservation.Id;
                newReview.Text = review.Text;
                

                _context.Set<Review>().Add(newReview);

                string notifCode;
                // Rating Calculation
                if (review.Rating > 0)
                {
                    var profile = _context.Profile.SingleOrDefault(x => x.Id == newReview.ToUserId);
                    if (profile == null)
                    {
                        respList.Add(newReview.ToUserId);
                        return this.Request.CreateResponse(HttpStatusCode.Unauthorized,
                            RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                    }
                    newReview.Rating = review.Rating;
                    notifCode = RespH.SRV_NOTIF_REVIEW_RATING_ADDED;
                    profile.RatingCount += 1;
                    profile.Score += newReview.Rating;
                    profile.Rating = (Decimal) profile.Score/profile.RatingCount;
                }
                else
                {
                    
                    newReview.Rating = 0;
                    notifCode = RespH.SRV_NOTIF_REVIEW_ADDED;
                }

                // Create Notification
                _context.Set<Notification>().Add(new Notification()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = newReview.ToUserId,
                    Type = ConstVals.General,
                    ReviewId = reviewGuid,
                    Code = notifCode,
                    Readed = false
                });

                _context.SaveChanges();
                respList.Add(reviewGuid);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }

        // PUT api/Review/{revId}
        [Route("api/Review/{revId}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPut]
        public HttpResponseMessage PutReview(string revId, ReviewDTO review)
        {
            try
            {
                var respList = new List<string>();
                ResponseDTO resp;

                // Check Review  exist
                var currentReview =
                    _context.Reviews.SingleOrDefault(
                        r => r.Id == revId);
                if (currentReview == null)
                {
                    respList.Add(revId);
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_REVIEW_NOTFOUND, respList));
                }

                // Check Reservation is not NULL
                Reservation reservation = _context.Reservations.SingleOrDefault(x => x.Id == currentReview.ReservationId);
                if (reservation == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_RESERVATION_NOTFOUND));

                // Check Reservation Status is Accepted
                if (reservation.Status != ConstVals.Accepted)
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_REVIEW_WRONG_RESERV_STATUS));

                // Check Reservation Dates
                if (reservation.DateTo >= DateTime.Now)
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_REVIEW_WRONG_DATE));

                // Check Review Text is not NULL
                resp = CheckHelper.isNull(review.Text, "Text", RespH.SRV_REVIEW_REQUIRED);
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

                // Check Review User
                if (currentReview.FromUserId != account.UserId)
                {
                    respList.Add(currentReview.FromUserId);
                    respList.Add(account.UserId);
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_REVIEW_WRONG_USER, respList));
                }

                currentReview.Text = review.Text;

                _context.SaveChanges();
                respList.Add(currentReview.Id);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED, respList));
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
