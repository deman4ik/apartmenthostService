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
using apartmenthostService.Messages;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class PhoneApiController : ApiController
    {
        private readonly IApartmenthostContext _context = new ApartmenthostContext();
        public ApiServices Services { get; set; }

        /// <summary>
        ///     POST api/Phone/ConfirmRequest
        /// </summary>
        [Route("api/Phone/ConfirmRequest")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage ReqeustCode()
        {
            try
            {
                var respList = new List<string>();
                ResponseDTO resp;
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

                var user = _context.Users.SingleOrDefault(x => x.Id == account.UserId);
                if (user == null)
                {
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                        RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }
                var profile = _context.Profile.SingleOrDefault(x => x.Id == account.UserId);


                resp = CheckHelper.IsNull(profile.Phone, "Phone", RespH.SRV_USER_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                if (user.PhoneConfirmed)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_PHONE_CONFIRMED, new List<string> {user.Id}));
                }
                var confirmCode = AuthUtils.randomNumString(4);
                user.SaltedAndHashedSmsCode = AuthUtils.hash(confirmCode, user.Salt);
                _context.MarkAsModified(user);
                _context.SaveChanges();


                using (SmsSender sender = new SmsSender())
                {
                    var result = sender.Send(profile.Phone, confirmCode);
                    if (result != "success")
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            RespH.Create(RespH.SRV_EXCEPTION, new List<string> {result}));
                }
                return Request.CreateResponse(HttpStatusCode.OK,
                    RespH.Create(RespH.SRV_DONE, new List<string> {user.Id}));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.InnerException.ToString()}));
            }
        }

        /// <summary>
        ///     POST api/Phone/Confirm
        /// </summary>
        [Route("api/Phone/Confirm")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage ConfirmCode(ConfirmRequest confirmRequest)
        {
            try
            {
                var respList = new List<string>();
                ResponseDTO resp;
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

                var user = _context.Users.SingleOrDefault(x => x.Id == account.UserId);
                if (user == null)
                {
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                        RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }
                if (user.PhoneConfirmed)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_PHONE_CONFIRMED, new List<string> {user.Id}));
                }
                if (confirmRequest.Code == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_REQUIRED, new List<string> {"code"}));
                }
                var incoming = AuthUtils.hash(confirmRequest.Code, user.Salt);

                if (!AuthUtils.slowEquals(incoming, user.SaltedAndHashedSmsCode))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_WRONG_CODE,
                            new List<string> {user.Id, confirmRequest.Code}));
                }
                user.SaltedAndHashedSmsCode = null;
                user.PhoneConfirmed = true;
                _context.MarkAsModified(user);
                _context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK,
                    RespH.Create(RespH.SRV_USER_PHONE_CONFIRMED, new List<string> {user.Id}));
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