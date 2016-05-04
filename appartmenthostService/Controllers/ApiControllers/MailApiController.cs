using System;
using System.Collections.Generic;
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
    public class MailApiController : ApiController
    {
        private readonly IApartmenthostContext _context = new ApartmenthostContext();
        public ApiServices Services { get; set; }


        [HttpPost]
        [Route("api/Mail/Unsubscribe")]
        public HttpResponseMessage Unsubscribe(UnsubscribeRequest req)
        {
            try
            {
                var respList = new List<string>();
                var user = _context.Users.SingleOrDefault(u => u.EmailSubCode == req.Code);
                if (user == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_NOTFOUND));
                switch (req.Type)
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
            MailLists mailLists = new MailLists
            {
                NewsletterList = new List<MailList>(),
                NorificationsList = new List<MailList>(),
                AllUsersList = new List<MailList>()
            };
            foreach (var newsletter in _context.Users.Where(x => x.EmailNewsletter && x.Email != null).ToList())
            {
                var user = new MailList
                {
                    Email = newsletter.Email,
                    FirstName = newsletter.Profile.FirstName,
                    LastName = newsletter.Profile.LastName
                };
                mailLists.NewsletterList.Add(user);
            }
            foreach (var notif in _context.Users.Where(x => x.EmailNotifications && x.Email != null).ToList())
            {
                var user = new MailList
                {
                    Email = notif.Email,
                    FirstName = notif.Profile.FirstName,
                    LastName = notif.Profile.LastName
                };
                mailLists.NorificationsList.Add(user);
            }
            foreach (var allusers in _context.Users.Where(x => x.Email != null).ToList())
            {
                var user = new MailList
                {
                    Email = allusers.Email,
                    FirstName = allusers.Profile.FirstName,
                    LastName = allusers.Profile.LastName
                };
                mailLists.AllUsersList.Add(user);
            }
           
            return Request.CreateResponse(HttpStatusCode.OK, mailLists);
        }

        [HttpPost]
        public string Generate()
        {
            var users = _context.Users.ToList();

            foreach (var user in users)
            {
                var u = _context.Users.SingleOrDefault(x => x.Id == user.Id);
                u.EmailSubCode = SequentialGuid.NewGuid().ToString();
                _context.SaveChanges();
            }
            return "ok";
        }
    }
}
