using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class MailApiController : ApiController
    {
        private readonly IApartmenthostContext _context = new ApartmenthostContext();
        public ApiServices Services { get; set; }


        [HttpPost]
        [Route("api/Mail/Unsubscribe")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        public HttpResponseMessage Unsubscribe(string type = null)
        {
            try
            {
                var respList = new List<string>();
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_NULL));
                var account = _context.Accounts.AsNoTracking().SingleOrDefault(a => a.AccountId == currentUser.Id);
                if (account == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_NOTFOUND));
                var user = _context.Users.SingleOrDefault(u => u.Id == account.UserId);
                switch (type)
                {
                    case "newsletter":
                        user.EmailNewsletter = false;
                        break;
                    case "notifications":
                        user.EmailNotifications = false;
                        break;
                    default:
                        user.EmailNewsletter = false;
                        user.EmailNotifications = false;
                        break;
                }
                _context.MarkAsModified(user);
                _context.SaveChanges();
                respList.Add(user.Id);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED, respList));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> { ex.ToString() }));
            }


        }

        [HttpGet]
        [Route("api/Mail/List")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        public HttpResponseMessage GetMailList()
        {
            MailList mailList = new MailList
            {
                NewsletterList = _context.Users.Where(x => x.EmailNewsletter && x.Email != null).Select(u => u.Email).ToList(),
                AllUsersList = _context.Users.Where(x => x.Email != null).Select(u => u.Email).ToList()
            };
            return Request.CreateResponse(HttpStatusCode.OK, mailList);
        }
    }
}
