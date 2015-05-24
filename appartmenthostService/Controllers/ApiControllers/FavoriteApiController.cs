using System;
using System.Collections.Generic;
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
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class FavoriteApiController : ApiController
    {
        public ApiServices Services { get; set; }
        private readonly apartmenthostContext _context = new apartmenthostContext();

        [Route("api/IsFavorite/{advertId}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpGet]
        public HttpResponseMessage IsFavorite(string advertId)
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
                status = _context.Favorites.Any(f => f.AdvertId == advertId && f.UserId == account.UserId);
            }
            return this.Request.CreateResponse(HttpStatusCode.OK, RespH.CreateBool(RespH.SRV_DONE, new List<bool>() { status }));
           
        }

        [Route("api/Favorite/{advertId}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage SetFavorite(string advertId)
        {
            try
            {
                var respList = new List<string>();

                // Check advertId is not NULL 
                if (advertId == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_FAVORITE_ADVERTID_NULL));

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

                var currentAdvertCount = _context.Adverts.Count(a => a.Id == advertId);
                if (currentAdvertCount == 0)
                {
                    respList.Add(advertId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ADVERT_NOTFOUND,respList));
                }

                bool status;
                var favorite =
                    _context.Favorites.SingleOrDefault(f => f.AdvertId == advertId && f.UserId == account.UserId);
                if (favorite == null)
                {
                    _context.Set<Favorite>().Add(new Favorite()
                    {
                        Id = Guid.NewGuid().ToString(),
                        AdvertId = advertId,
                        UserId = account.UserId
                    });
                    status = true;
                }
                else
                {
                    _context.Favorites.Remove(favorite);
                    status = false;
                }
                _context.SaveChanges();
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.CreateBool(RespH.SRV_DONE, new List<bool>(){status}));
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
