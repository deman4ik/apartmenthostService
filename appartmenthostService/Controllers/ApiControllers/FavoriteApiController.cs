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
    public class FavoriteApiController : ApiController
    {
        public ApiServices Services { get; set; }
        private readonly apartmenthostContext _context = new apartmenthostContext();

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
                status = _context.Favorites.Any(f => f.CardId == cardId && f.UserId == account.UserId);
            }
            return this.Request.CreateResponse(HttpStatusCode.OK, RespH.CreateBool(RespH.SRV_DONE, new List<bool>() { status }));
           
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
                if (cardId == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_FAVORITE_CARDID_NULL));

                // Check Current User
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
                var account = AuthUtils.GetUserAccount(_context,currentUser);
                if (account == null)
                {
                    respList.Add(currentUser.Id);
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                var currentCard = _context.Cards.SingleOrDefault(a => a.Id == cardId);
                if (currentCard == null)
                {
                    respList.Add(cardId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_CARD_NOTFOUND,respList));
                }

                bool status;
                var favorite =
                    _context.Favorites.SingleOrDefault(f => f.CardId == cardId && f.UserId == account.UserId);
                if (favorite == null)
                {
                    string favoriteGUID = Guid.NewGuid().ToString();
                    _context.Set<Favorite>().Add(new Favorite()
                    {
                        Id = favoriteGUID,
                        CardId = cardId,
                        UserId = account.UserId
                    });
                    _context.SaveChanges();
                    // Create Notification
                    Notifications.Create(_context, currentCard.UserId, ConstVals.General, RespH.SRV_NOTIF_CARD_FAVORITED,
                        favoriteGUID, null, null, true);
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
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.CreateBool(RespH.SRV_DONE, new List<bool>(){status}));
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
