using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Messages;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    public class ReviewApiController : ApiController
    {
        private readonly IApartmenthostContext _context = new ApartmenthostContext();

        public ReviewApiController()
        {
        }

        public ReviewApiController(IApartmenthostContext context)
        {
            _context = context;
        }

        public ApiServices Services { get; set; }
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
                return Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
            var account = AuthUtils.GetUserAccount(_context, currentUser);
            if (account == null)
            {
                respList.Add(currentUser.Id);
                return Request.CreateResponse(HttpStatusCode.Unauthorized,
                    RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
            }

            var ownerReviews = new List<ReservReviewDTO>();
            var renterReviews = new List<ReservReviewDTO>();
            var result = new List<ReservReviewDTO>();
            if (type == ConstVals.Owner || string.IsNullOrWhiteSpace(type))
            {
                ownerReviews =
                    _context.Reservations.AsNoTracking().Where(
                        r => r.Card.UserId == account.UserId &&
                             r.Status == ConstVals.Accepted
                        ).OrderByDescending(r => r.CreatedAt)
                        .Select(
                            x => new ReservReviewDTO
                            {
                                Reservation = new ReservationDTO
                                {
                                    Id = x.Id,
                                    CardId = x.CardId,
                                    UserId = x.UserId,
                                    Status = x.Status,
                                    Gender = x.Gender,
                                    DateFrom = x.DateFrom,
                                    DateTo = x.DateTo,
                                    CreatedAt = x.CreatedAt,
                                    UpdatedAt = x.UpdatedAt
                                }
                            }).ToList();

                if (ownerReviews.Any())
                {
                    foreach (var owrev in ownerReviews)
                    {
                        owrev.Type = ConstVals.Owner;
                        owrev.CanResponse = owrev.Reservation.DateTo <= DateTime.Now;
                        owrev.Reservation.User =
                            _context.Users.AsNoTracking()
                                .Where(u => u.Id == owrev.Reservation.UserId)
                                .Select(x => new BaseUserDTO
                                {
                                    Id = x.Profile.Id,
                                    Email = x.Email,
                                    FirstName = x.Profile.FirstName,
                                    LastName = x.Profile.LastName,
                                    Rating = x.Profile.Rating,
                                    RatingCount = x.Profile.RatingCount,
                                    Gender = x.Profile.Gender,
                                    Picture = new PictureDTO
                                    {
                                        //  Id = x.Profile.Picture.Id,
                                        //  Name = x.Profile.Picture.Name,
                                        //  Description = x.Profile.Picture.Description,
                                        Url = x.Profile.Picture.Url,
                                        Xsmall = x.Profile.Picture.Xsmall,
                                        Small = x.Profile.Picture.Small,
                                        Mid = x.Profile.Picture.Mid,
                                        Large = x.Profile.Picture.Large,
                                        Xlarge = x.Profile.Picture.Xlarge,
                                        Default = x.Profile.Picture.Default
                                        //  CreatedAt = x.Profile.Picture.CreatedAt
                                    }
                                }).FirstOrDefault();

                        owrev.OwnerReview =
                            _context.Reviews.AsNoTracking().Where(
                                rev => rev.ReservationId == owrev.Reservation.Id && rev.FromUserId == account.UserId)
                                .OrderByDescending(r => r.CreatedAt)
                                .Select(owreview => new ReviewDTO
                                {
                                    Id = owreview.Id,
                                    FromUserId = owreview.FromUserId,
                                    ToUserId = owreview.ToUserId,
                                    ReservationId = owreview.ReservationId,
                                    Rating = owreview.Rating,
                                    Text = owreview.Text,
                                    CreatedAt = owreview.CreatedAt,
                                    UpdatedAt = owreview.UpdatedAt
                                }).FirstOrDefault();

                        owrev.RenterReview =
                            _context.Reviews.AsNoTracking()
                                .Where(
                                    rev => rev.ReservationId == owrev.Reservation.Id && rev.ToUserId == account.UserId)
                                .OrderByDescending(r => r.CreatedAt)
                                .Select(renrev => new ReviewDTO
                                {
                                    Id = renrev.Id,
                                    FromUserId = renrev.FromUserId,
                                    ToUserId = renrev.ToUserId,
                                    ReservationId = renrev.ReservationId,
                                    Rating = renrev.Rating,
                                    Text = renrev.Text,
                                    CreatedAt = renrev.CreatedAt,
                                    UpdatedAt = renrev.UpdatedAt
                                }).FirstOrDefault();
                        result.Add(owrev);
                    }
                }
            }
            if (type == ConstVals.Renter || string.IsNullOrWhiteSpace(type))
            {
                renterReviews =
                    _context.Reservations.AsNoTracking()
                        .Where(r => r.UserId == account.UserId && r.Status == ConstVals.Accepted)
                        .OrderByDescending(r => r.CreatedAt)
                        .Select(
                            x => new ReservReviewDTO
                            {
                                Reservation = new ReservationDTO
                                {
                                    Id = x.Id,
                                    CardId = x.CardId,
                                    UserId = x.UserId,
                                    Status = x.Status,
                                    Gender = x.Gender,
                                    DateFrom = x.DateFrom,
                                    DateTo = x.DateTo,
                                    CreatedAt = x.CreatedAt,
                                    UpdatedAt = x.UpdatedAt
                                }
                            }).ToList();

                if (renterReviews.Any())
                {
                    foreach (var rerev in renterReviews)
                    {
                        rerev.Type = ConstVals.Renter;
                        rerev.CanResponse = rerev.Reservation.DateTo <= DateTime.Now;
                        rerev.Reservation.Card =
                            _context.Cards.AsNoTracking().Where(c => c.Id == rerev.Reservation.CardId).Select(x =>
                                new CardDTO
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    UserId = x.UserId,
                                    PriceDay = x.Genders.FirstOrDefault(ge => ge.Name == rerev.Reservation.Gender).Price,
                                    Cohabitation = x.Cohabitation,
                                    IsFavorite = x.Favorites.Any(f => f.UserId == account.UserId),
                                    Apartment = new ApartmentDTO
                                    {
                                        Id = x.Apartment.Id,
                                        Name = x.Apartment.Name,
                                        Type = x.Apartment.Type,
                                        Options = x.Apartment.Options,
                                        Adress = x.Apartment.Adress,
                                        FormattedAdress = x.Apartment.FormattedAdress,
                                        Latitude = x.Apartment.Latitude,
                                        Longitude = x.Apartment.Longitude,
                                        PlaceId = x.Apartment.PlaceId,
                                        DefaultPicture =
                                            x.Apartment.Pictures.Where(ap => ap.Default)
                                                .Select(apic => new PictureDTO
                                                {
                                                    //  Id = apic.Id,
                                                    //  Name = apic.Name,
                                                    //  Description = apic.Description,
                                                    Url = apic.Url,
                                                    Xsmall = apic.Xsmall,
                                                    Small = apic.Small,
                                                    Mid = apic.Mid,
                                                    Large = apic.Large,
                                                    Xlarge = apic.Xlarge,
                                                    Default = apic.Default
                                                    //  CreatedAt = apic.CreatedAt
                                                }).FirstOrDefault()
                                    },
                                    User = new UserDTO
                                    {
                                        Id = x.User.Id,
                                        Email = x.User.Email,
                                        FirstName = x.User.Profile.FirstName,
                                        LastName = x.User.Profile.LastName,
                                        Rating = x.User.Profile.Rating,
                                        RatingCount = x.User.Profile.RatingCount,
                                        Gender = x.User.Profile.Gender,
                                        Picture = new PictureDTO
                                        {
                                            //  Id = x.User.Profile.Picture.Id,
                                            //  Name = x.User.Profile.Picture.Name,
                                            //  Description = x.User.Profile.Picture.Description,
                                            Url = x.User.Profile.Picture.Url,
                                            Xsmall = x.User.Profile.Picture.Xsmall,
                                            Small = x.User.Profile.Picture.Small,
                                            Mid = x.User.Profile.Picture.Mid,
                                            Large = x.User.Profile.Picture.Large,
                                            Xlarge = x.User.Profile.Picture.Xlarge,
                                            Default = x.User.Profile.Picture.Default
                                            //  CreatedAt = x.User.Profile.Picture.CreatedAt
                                        }
                                    }
                                }).FirstOrDefault();

                        rerev.OwnerReview =
                            _context.Reviews.AsNoTracking()
                                .Where(
                                    rev => rev.ReservationId == rerev.Reservation.Id && rev.ToUserId == account.UserId)
                                .OrderByDescending(r => r.CreatedAt)
                                .Select(owrev => new ReviewDTO
                                {
                                    Id = owrev.Id,
                                    FromUserId = owrev.FromUserId,
                                    ToUserId = owrev.ToUserId,
                                    ReservationId = owrev.ReservationId,
                                    Rating = owrev.Rating,
                                    Text = owrev.Text,
                                    CreatedAt = owrev.CreatedAt,
                                    UpdatedAt = owrev.UpdatedAt
                                }).FirstOrDefault();

                        rerev.RenterReview =
                            _context.Reviews.AsNoTracking()
                                .Where(
                                    rev => rev.ReservationId == rerev.Reservation.Id && rev.FromUserId == account.UserId)
                                .OrderByDescending(r => r.CreatedAt)
                                .Select(renrev => new ReviewDTO
                                {
                                    Id = renrev.Id,
                                    FromUserId = renrev.FromUserId,
                                    ToUserId = renrev.ToUserId,
                                    ReservationId = renrev.ReservationId,
                                    Rating = renrev.Rating,
                                    Text = renrev.Text,
                                    CreatedAt = renrev.CreatedAt,
                                    UpdatedAt = renrev.UpdatedAt
                                }).FirstOrDefault();

                        result.Add(rerev);
                    }
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
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
                if (review == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_REVIEW_NULL));

                // Check Reservation is not NULL
                var reservation = _context.Reservations.AsNoTracking().SingleOrDefault(x => x.Id == resId);
                if (reservation == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_RESERVATION_NOTFOUND));

                // Check Reservation Status is Accepted
                if (reservation.Status != ConstVals.Accepted)
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_REVIEW_WRONG_RESERV_STATUS));

                // Check Reservation Dates
                if (reservation.DateTo >= DateTime.Now)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_REVIEW_WRONG_DATE));

                // Check Review Text is not NULL
                resp = CheckHelper.IsNull(review.Text, "Text", RespH.SRV_REVIEW_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Review Rating is not NULL
                //resp = CheckHelper.IsNull(review.Rating, "Rating", RespH.SRV_REVIEW_REQUIRED);
                //if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

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
                resp = CheckHelper.IsProfileFill(_context, account.UserId);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                var newReview = new Review();
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
                    _context.Reviews.AsNoTracking().SingleOrDefault(
                        r =>
                            r.ReservationId == reservation.Id && r.FromUserId == newReview.FromUserId &&
                            r.ToUserId == newReview.ToUserId);
                if (currentReview != null)
                {
                    respList.Add(currentReview.Id);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                        RespH.Create(RespH.SRV_REVIEW_EXISTS, respList));
                }
                var reviewGuid = SequentialGuid.NewGuid().ToString();
                newReview.Id = reviewGuid;
                newReview.ReservationId = reservation.Id;
                newReview.Text = review.Text;


                _context.Reviews.Add(newReview);

                string notifCode;
                // Rating Calculation
                if (review.Rating > 0)
                {
                    var profile = _context.Profile.SingleOrDefault(x => x.Id == newReview.ToUserId);
                    if (profile == null)
                    {
                        respList.Add(newReview.ToUserId);
                        return Request.CreateResponse(HttpStatusCode.Unauthorized,
                            RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                    }
                    newReview.Rating = review.Rating;
                    notifCode = RespH.SRV_NOTIF_REVIEW_RATING_ADDED;
                    profile.RatingCount += 1;
                    profile.Score += newReview.Rating;
                    profile.Rating = profile.Score/profile.RatingCount;
                }
                else
                {
                    newReview.Rating = 0;
                    notifCode = RespH.SRV_NOTIF_REVIEW_ADDED;
                }
                _context.SaveChanges();
                // Create Notification
                Notifications.Create(_context, newReview.ToUserId, ConstVals.General, notifCode, null, null, reviewGuid);
                var fromUser = _context.Users.AsNoTracking().SingleOrDefault(x => x.Id == account.UserId);
                var fromProfile = _context.Profile.AsNoTracking().SingleOrDefault(x => x.Id == account.UserId);
                var toUser = _context.Users.AsNoTracking().SingleOrDefault(x => x.Id == newReview.ToUserId);
                var toProfile = _context.Profile.AsNoTracking().SingleOrDefault(x => x.Id == newReview.ToUserId);
                using (MailSender mailSender = new MailSender())
                {
                    var bem = new BaseEmailMessage
                    {
                        Code = notifCode,
                        FromUserName = fromProfile.FirstName,
                        FromUserEmail = fromProfile.ContactEmail ?? fromUser.Email,
                        ToUserName = toProfile.FirstName,
                        ToUserEmail = toProfile.ContactEmail ?? toUser.Email,
                        ReviewText = newReview.Text,
                        ReviewRating = newReview.Rating
                    };
                    mailSender.Create(_context, bem);
                }

                respList.Add(reviewGuid);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.InnerException.ToString()}));
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
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                        RespH.Create(RespH.SRV_REVIEW_NOTFOUND, respList));
                }

                // Check Reservation is not NULL
                var reservation =
                    _context.Reservations.AsNoTracking().SingleOrDefault(x => x.Id == currentReview.ReservationId);
                if (reservation == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_RESERVATION_NOTFOUND));

                // Check Reservation Status is Accepted
                if (reservation.Status != ConstVals.Accepted)
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_REVIEW_WRONG_RESERV_STATUS));

                // Check Reservation Dates
                if (reservation.DateTo >= DateTime.Now)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_REVIEW_WRONG_DATE));

                // Check Review Text is not NULL
                resp = CheckHelper.IsNull(review.Text, "Text", RespH.SRV_REVIEW_REQUIRED);
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
                resp = CheckHelper.IsProfileFill(_context, account.UserId);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Review User
                if (currentReview.FromUserId != account.UserId)
                {
                    respList.Add(currentReview.FromUserId);
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                        RespH.Create(RespH.SRV_REVIEW_WRONG_USER, respList));
                }


                currentReview.Text = review.Text;
                _context.MarkAsModified(currentReview);
                _context.SaveChanges();
                respList.Add(currentReview.Id);
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