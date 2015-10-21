using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    public class NotificationApiController : ApiController
    {
        private readonly IApartmenthostContext _context = new ApartmenthostContext();
        public ApiServices Services { get; set; }

        public NotificationApiController()
        {
        }

        public NotificationApiController(IApartmenthostContext context)
        {
            _context = context;
        }

        // GET api/Notifications
        [Route("api/Notifications")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpGet]
        public HttpResponseMessage GetNotifications()
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

                var notifications = _context.Notifications.Where(n => n.UserId == account.UserId && n.Readed == false);
                var result = new List<NotificationDTO>();
                foreach (var notification in notifications)
                {
                    var notif = new NotificationDTO();
                    notif.Id = notification.Id;
                    notif.Type = notification.Type;
                    notif.UserId = notification.UserId;
                    notif.Code = notification.Code;
                    notif.Readed = notification.Readed;
                    notif.CreatedAt = notification.CreatedAt;
                    notif.Data = new NotificationData();

                    switch (notification.Code)
                    {
                        case RespH.SRV_NOTIF_RESERV_PENDING:
                            notif.Data.CardId = notification.Reservation.CardId;
                            notif.Data.CardName = notification.Reservation.Card.Name;
                            notif.Data.ReservationId = notification.ReservationId;
                            notif.Data.ResrvationStatus = notification.Reservation.Status;
                            notif.Data.UserId = notification.Reservation.UserId;
                            notif.Data.FirstName = notification.Reservation.User.Profile.FirstName;
                            notif.Data.LastName = notification.Reservation.User.Profile.LastName;
                            break;
                        case RespH.SRV_NOTIF_RESERV_ACCEPTED:
                            notif.Data.CardId = notification.Reservation.CardId;
                            notif.Data.CardName = notification.Reservation.Card.Name;
                            notif.Data.ReservationId = notification.ReservationId;
                            notif.Data.ResrvationStatus = notification.Reservation.Status;
                            notif.Data.UserId = notification.Reservation.Card.UserId;
                            notif.Data.FirstName = notification.Reservation.Card.User.Profile.FirstName;
                            notif.Data.LastName = notification.Reservation.Card.User.Profile.LastName;
                            break;

                        case RespH.SRV_NOTIF_RESERV_DECLINED:
                            notif.Data.CardId = notification.Reservation.CardId;
                            notif.Data.CardName = notification.Reservation.Card.Name;
                            notif.Data.ReservationId = notification.ReservationId;
                            notif.Data.ResrvationStatus = notification.Reservation.Status;
                            notif.Data.UserId = notification.Reservation.Card.UserId;
                            notif.Data.FirstName = notification.Reservation.Card.User.Profile.FirstName;
                            notif.Data.LastName = notification.Reservation.Card.User.Profile.LastName;
                            break;
                        case RespH.SRV_NOTIF_CARD_FAVORITED:
                            notif.Data.CardId = notification.Favorite.CardId;
                            notif.Data.CardName = notification.Favorite.Card.Name;
                            notif.Data.UserId = notification.Favorite.UserId;
                            notif.Data.FirstName = notification.Favorite.User.Profile.FirstName;
                            notif.Data.LastName = notification.Favorite.User.Profile.LastName;
                            break;
                        case RespH.SRV_NOTIF_REVIEW_ADDED:

                            notif.Data.ReviewId = notification.ReviewId;
                            notif.Data.ReviewText = notification.Review.Text;
                            notif.Data.UserId = notification.Review.FromUserId;
                            notif.Data.FirstName = notification.Review.FromUser.Profile.FirstName;
                            notif.Data.LastName = notification.Review.FromUser.Profile.LastName;
                            break;
                        case RespH.SRV_NOTIF_REVIEW_RATING_ADDED:

                            notif.Data.ReviewId = notification.ReviewId;
                            notif.Data.ReviewText = notification.Review.Text;
                            notif.Data.ReviewRating = notification.Review.Rating;
                            notif.Data.UserId = notification.Review.FromUserId;
                            notif.Data.FirstName = notification.Review.FromUser.Profile.FirstName;
                            notif.Data.LastName = notification.Review.FromUser.Profile.LastName;
                            break;

                        case RespH.SRV_NOTIF_REVIEW_AVAILABLE:
                            notif.Data.CardId = notification.Reservation.CardId;
                            notif.Data.CardName = notification.Reservation.Card.Name;
                            notif.Data.ReservationId = notification.ReservationId;
                            break;
                    }
                    result.Add(notif);
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
            }
        }

        // POST api/Notification/ClearAll
        [Route("api/Notification/ClearAll")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage ClearAll()
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

                var notifications = _context.Notifications.Where(x => x.UserId == account.UserId && x.Readed == false);

                foreach (var notification in notifications)
                {
                    notification.Readed = true;
                }

                _context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.InnerException.ToString()}));
            }
        }

        // POST api/Notification/Clear/{id}
        [Route("api/Notification/Clear/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage Clear(string id)
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

                var notification = _context.Notifications.SingleOrDefault(x => x.Id == id);
                if (notification == null)
                {
                    respList.Add(id);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                        RespH.Create(RespH.SRV_NOTIFICATION_NOTFOUND, respList));
                }

                if (notification.UserId != account.UserId)
                {
                    respList.Add(notification.UserId);
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                        RespH.Create(RespH.SRV_NOTIFICATION_WRONG_USER, respList));
                }

                notification.Readed = true;
                _context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED, respList));
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