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
    public class StandartRegistrationController : ApiController
    {
        private readonly apartmenthostContext _context = new apartmenthostContext();
        public ApiServices Services { get; set; }
        // POST api/CustomRegistration
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        [HttpPost]
        public HttpResponseMessage Post(RegistrationRequest registrationRequest)
        {
            try
            {
                var respList = new List<string>();
                if (!AuthUtils.IsEmailValid(registrationRequest.Email))
                {
                    respList.Add(registrationRequest.Email);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_REG_INVALID_EMAIL, respList));
                }
                if (registrationRequest.Password.Length < 8)
                {
                    respList.Add(registrationRequest.Password);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_REG_INVALID_PASSWORD, respList));
                }


                var user = _context.Users.SingleOrDefault(a => a.Email == registrationRequest.Email);
                if (user != null)
                {
                    respList.Add(registrationRequest.Email);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_REG_EXISTS_EMAIL, respList));
                }
                var salt = AuthUtils.generateSalt();
                var confirmCode = AuthUtils.randomNumString(6);
                var newUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = registrationRequest.Email,
                    Salt = salt,
                    SaltedAndHashedPassword = AuthUtils.hash(registrationRequest.Password, salt),
                    SaltedAndHashedEmail = AuthUtils.hash(confirmCode, salt)
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();
                AuthUtils.CreateAccount(StandartLoginProvider.ProviderName, registrationRequest.Email,
                    StandartLoginProvider.ProviderName + ":" + registrationRequest.Email,
                    registrationRequest.Email);
                Notifications.SendEmail(_context, newUser.Id, ConstVals.General, ConstVals.Reg, null, null, null,
                    confirmCode);
                respList.Add(newUser.Id);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
            }
        }

        [Route("api/EmailConfirm")]
        [AuthorizeLevel(AuthorizationLevel.Application)]
        [HttpPost]
        public HttpResponseMessage Confirm(ConfirmRequest confirmRequest)
        {
            try
            {
                if (confirmRequest.Code == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_REQUIRED, new List<string> {"code"}));
                }
                var user =
                    _context.Users.SingleOrDefault(x => x.Id == confirmRequest.UserId || x.Email == confirmRequest.Email);
                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_NOTFOUND,
                            new List<string> {confirmRequest.UserId ?? confirmRequest.Email}));
                }

                if (user.EmailConfirmed)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_CONFIRMED, new List<string> {confirmRequest.UserId}));
                }
                var incoming = AuthUtils.hash(confirmRequest.Code, user.Salt);

                if (AuthUtils.slowEquals(incoming, user.SaltedAndHashedEmail))
                {
                    user.EmailConfirmed = true;
                    user.SaltedAndHashedEmail = null;
                    _context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK,
                        RespH.Create(RespH.SRV_USER_CONFIRMED, new List<string> {user.Id}));
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_USER_WRONG_CODE,
                        new List<string> {confirmRequest.UserId ?? confirmRequest.Code}));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
            }
        }

        [Route("api/PasswordReset")]
        [AuthorizeLevel(AuthorizationLevel.Application)]
        [HttpPost]
        public HttpResponseMessage ResetPassword(ConfirmRequest resetRequest)
        {
            try
            {
                var user =
                    _context.Users.SingleOrDefault(x => x.Id == resetRequest.UserId || x.Email == resetRequest.Email);
                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_NOTFOUND,
                            new List<string> {resetRequest.UserId ?? resetRequest.Email}));
                }
                var confirmCode = AuthUtils.randomNumString(8);
                user.SaltedAndHashedCode = AuthUtils.hash(confirmCode, user.Salt);
                user.ResetRequested = true;
                _context.SaveChanges();
                Notifications.SendEmail(_context, user.Id, ConstVals.General, ConstVals.Restore, null, null, null,
                    confirmCode);
                return Request.CreateResponse(HttpStatusCode.OK,
                    RespH.Create(RespH.SRV_USER_RESET_REQUESTED, new List<string> {user.Id}));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
            }
        }

        [Route("api/PasswordChange")]
        [AuthorizeLevel(AuthorizationLevel.Application)]
        [HttpPost]
        public HttpResponseMessage ChangePassword(ConfirmRequest resetRequest)
        {
            try
            {
                User user;
                if (!string.IsNullOrEmpty(resetRequest.UserId) || !string.IsNullOrEmpty(resetRequest.Email))
                {
                    if (resetRequest.Code == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            RespH.Create(RespH.SRV_USER_REQUIRED, new List<string> {"code"}));
                    }

                    user =
                        _context.Users.SingleOrDefault(x => x.Id == resetRequest.UserId || x.Email == resetRequest.Email);
                    if (user == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            RespH.Create(RespH.SRV_USER_NOTFOUND,
                                new List<string> {resetRequest.UserId ?? resetRequest.Email}));
                    }
                    if (!user.ResetRequested)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            RespH.Create(RespH.SRV_USER_RESET_NOT_REQUESTED,
                                new List<string> {resetRequest.UserId ?? resetRequest.Email}));
                    }
                    var incoming = AuthUtils.hash(resetRequest.Code, user.Salt);

                    if (!AuthUtils.slowEquals(incoming, user.SaltedAndHashedCode))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_USER_WRONG_CODE,
                        new List<string> { resetRequest.UserId ?? resetRequest.Email, resetRequest.Code }));
                      
                    }

                    user.SaltedAndHashedCode = null;
                    user.EmailConfirmed = true;
                    user.ResetRequested = false;
                }
                else
                {
                    // Check Current User
                    var currentUser = User as ServiceUser;
                    if (currentUser == null)
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
                    var account = AuthUtils.GetUserAccount(_context, currentUser);
                    if (account == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized,
                            RespH.Create(RespH.SRV_USER_NOTFOUND, new List<string> {currentUser.Id}));
                    }
                    user =
                        _context.Users.SingleOrDefault(x => x.Id == account.UserId);
                    if (user == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            RespH.Create(RespH.SRV_USER_NOTFOUND,
                                new List<string> {account.UserId}));
                    }

                    if (string.IsNullOrWhiteSpace(resetRequest.CurrentPassword))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            RespH.Create(RespH.SRV_USER_REQUIRED, new List<string> {"current password"}));
                    }

                    if (
                        !AuthUtils.slowEquals(AuthUtils.hash(resetRequest.CurrentPassword, user.Salt),
                            user.SaltedAndHashedPassword))
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized,
                            RespH.Create(RespH.SRV_LOGIN_INVALID_PASS));
                    }
                }
                if (string.IsNullOrWhiteSpace(resetRequest.Password))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_REQUIRED, new List<string> {"password"}));
                }
                if (resetRequest.Password.Length < 8)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_REG_INVALID_PASSWORD, new List<string> { resetRequest.Password }));
                }
                var salt = AuthUtils.generateSalt();
                    user.Salt = salt;
                    user.SaltedAndHashedPassword = AuthUtils.hash(resetRequest.Password, salt);
                    _context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK,
                        RespH.Create(RespH.SRV_USER_RESETED, new List<string> {user.Id}));
               

                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
            }
        }
    }
}