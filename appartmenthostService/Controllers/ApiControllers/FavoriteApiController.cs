using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.Authentication;
using apartmenthostService.Helpers;
using apartmenthostService.Messages;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class FavoriteApiController : ApiController
    {
        private readonly IApartmenthostContext _context = new ApartmenthostContext();
        public ApiServices Services { get; set; }

        public FavoriteApiController()
        {
        }

        public FavoriteApiController(IApartmenthostContext context)
        {
            _context = context;
        }

        [Route("api/IsFavorite/{cardId}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpGet]
        public HttpResponseMessage IsFavorite(string cardId)
        {
            var currentUser = User as ServiceUser;
            var account = AuthUtils.GetUserAccount(_context, currentUser);
            bool status;
            if (account == null)
            {
                status = false;
            }
            else
            {
                status = _context.Favorites.AsNoTracking().Any(f => f.CardId == cardId && f.UserId == account.UserId);
            }
            return Request.CreateResponse(HttpStatusCode.OK, RespH.CreateBool(RespH.SRV_DONE, new List<bool> {status}));
        }

        [Route("api/Favorite/{cardId}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage SetFavorite(string cardId)
        {
            try
            {
                var respList = new List<string>();

                // Check advertId is not NULL 
                if (cardId == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_FAVORITE_CARDID_NULL));

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

                var currentCard = _context.Cards.AsNoTracking().SingleOrDefault(a => a.Id == cardId);
                if (currentCard == null)
                {
                    respList.Add(cardId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_CARD_NOTFOUND, respList));
                }

                if (currentCard.UserId == account.UserId)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.CreateBool(RespH.SRV_FAVORITE_WRONG_USER));
                }
                bool status;
                var favorite =
                    _context.Favorites.SingleOrDefault(f => f.CardId == cardId && f.UserId == account.UserId);
                if (favorite == null)
                {
                    var favoriteGUID = SequentialGuid.NewGuid().ToString();
                    _context.Favorites.Add(new Favorite
                    {
                        Id = favoriteGUID,
                        CardId = cardId,
                        UserId = account.UserId
                    });
                    _context.SaveChanges();
                    // Create Notification
                    Notifications.Create(_context, currentCard.UserId, ConstVals.General, RespH.SRV_NOTIF_CARD_FAVORITED,
                        favoriteGUID, null, null);

                    var user = _context.Users.AsNoTracking().SingleOrDefault(x => x.Id == account.UserId);
                    var profile = _context.Profile.AsNoTracking().SingleOrDefault(x => x.Id == account.UserId);
                    if (user.EmailNotifications)
                    {
                        using (MailSender mailSender = new MailSender())
                        {
                            var bem = new BaseEmailMessage
                            {
                                Code = RespH.SRV_NOTIF_CARD_FAVORITED,
                                CardId = currentCard.Id,
                                FromUserName = profile.FirstName,
                                FromUserEmail = user.Email,
                                ToUserName = currentCard.User.Profile.FirstName,
                                ToUserEmail = currentCard.User.Email,
                                UnsubscrCode = currentCard.User.EmailSubCode
                            };
                            mailSender.Create(_context, bem);
                        }
                    }
                    status = true;
                }
                else
                {
                    var notif = _context.Notifications.SingleOrDefault(n => n.FavoriteId == favorite.Id);
                    if (notif != null) _context.Notifications.Remove(notif);
                    _context.SaveChanges();
                    _context.Favorites.Remove(favorite);
                    status = false;
                }
                _context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK,
                    RespH.CreateBool(RespH.SRV_DONE, new List<bool> {status}));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.InnerException.ToString()}));
            }
        }
    }
}