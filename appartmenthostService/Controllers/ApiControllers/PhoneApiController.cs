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
                var profile = _context.Profile.AsNoTracking().SingleOrDefault(x => x.Id == account.UserId);


                resp = CheckHelper.IsNull(profile.Phone, "Phone", RespH.SRV_USER_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                if (user.PhoneStatus == ConstVals.PConf)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_PHONE_CONFIRMED, new List<string> {user.PhoneStatus}));
                }
                if (user.PhoneCodeRequestedAt.HasValue && user.PhoneStatus == ConstVals.PPending)
                {
                    if (user.PhoneCodeRequestedAt.Value.AddMinutes(3) > DateTime.Now)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            RespH.Create(RespH.SRV_USER_PHONE_CONFIRM_REQUESTED,
                                new List<string> {user.PhoneCodeRequestedAt.ToString()}));
                    }
                }
                var confirmCode = AuthUtils.RandomNumString(4);
                user.SaltedAndHashedSmsCode = AuthUtils.Hash(confirmCode, user.Salt);
                user.PhoneCodeRequestedAt = DateTime.Now;
                user.PhoneStatus = ConstVals.PPending;
                _context.MarkAsModified(user);
                _context.SaveChanges();

                var regArt = _context.Article.SingleOrDefault(x => x.Name == ConstVals.Reg && x.Type == ConstVals.Sms);
                string smstext;
                if (regArt != null)
                {
                    smstext = confirmCode + " " + regArt.Text;
                }
                else
                {
                    smstext = confirmCode;
                }
                using (SmsSender sender = new SmsSender())
                {
                    sender.Send(profile.Phone, smstext);
                }
                return Request.CreateResponse(HttpStatusCode.OK,
                    RespH.Create(RespH.SRV_DONE, new List<string> {user.PhoneStatus}));
            }
            catch (Exception ex)
            {
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
                if (user.PhoneStatus == ConstVals.PConf)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_PHONE_CONFIRMED, new List<string> {user.Id}));
                }
                if (confirmRequest.Code == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_REQUIRED, new List<string> {"code"}));
                }
                var incoming = AuthUtils.Hash(confirmRequest.Code, user.Salt);

                if (!AuthUtils.SlowEquals(incoming, user.SaltedAndHashedSmsCode))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_WRONG_CODE,
                            new List<string> {user.Id, confirmRequest.Code}));
                }
                user.SaltedAndHashedSmsCode = null;
                user.PhoneStatus = ConstVals.PConf;
                user.PhoneCodeRequestedAt = null;
                _context.MarkAsModified(user);
                _context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK,
                    RespH.Create(RespH.SRV_USER_PHONE_CONFIRMED, new List<string> {user.PhoneStatus}));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.InnerException.ToString()}));
            }
        }
    }
}