using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class StandartRegistrationController : ApiController
    {
        private readonly IApartmenthostContext _context = new ApartmenthostContext();
        public ApiServices Services { get; set; }

        public StandartRegistrationController()
        {
        }

        public StandartRegistrationController(IApartmenthostContext context)
        {
            _context = context;
        }

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


                var user = _context.Users.AsNoTracking().SingleOrDefault(a => a.Email == registrationRequest.Email);
                if (user != null)
                {
                    respList.Add(registrationRequest.Email);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_REG_EXISTS_EMAIL, respList));
                }
                var salt = AuthUtils.GenerateSalt();
                var confirmCode = AuthUtils.RandomNumString(6);
                var newUser = new User
                {
                    Id = SequentialGuid.NewGuid().ToString(),
                    Email = registrationRequest.Email,
                    Salt = salt,
                    SaltedAndHashedPassword = AuthUtils.Hash(registrationRequest.Password, salt),
                    SaltedAndHashedEmail = AuthUtils.Hash(confirmCode, salt)
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();
                AuthUtils.CreateAccount(_context, StandartLoginProvider.ProviderName, registrationRequest.Email,
                    StandartLoginProvider.ProviderName + ":" + registrationRequest.Email,
                    registrationRequest.Email, registrationRequest.FirstName);

                using (MailSender mailSender = new MailSender())
                {
                    var bem = new BaseEmailMessage
                    {
                        Code = ConstVals.Reg,
                        ToUserId = newUser.Id,
                        ToUserEmail = registrationRequest.Email,
                        ToUserName = registrationRequest.FirstName,
                        ConfirmCode = confirmCode
                    };
                    mailSender.Create(_context, bem);
                }

                respList.Add(newUser.Id);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
            }
        }

        [Route("api/AdminReg")]
        [AuthorizeLevel(AuthorizationLevel.Admin)]
        [HttpPost]
        public HttpResponseMessage AdminRegistration(RegistrationRequest registrationRequest)
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


                var admin = _context.Admins.AsNoTracking().SingleOrDefault(a => a.Email == registrationRequest.Email);
                if (admin != null)
                {
                    respList.Add(registrationRequest.Email);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_REG_EXISTS_EMAIL, respList));
                }
                var salt = AuthUtils.GenerateSalt();

                var newUser = new Admin
                {
                    Id = SequentialGuid.NewGuid().ToString(),
                    Email = registrationRequest.Email,
                    Salt = salt,
                    SaltedAndHashedPassword = AuthUtils.Hash(registrationRequest.Password, salt)
                };
                _context.Admins.Add(newUser);
                _context.SaveChanges();

                respList.Add(newUser.Id);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
            }
            catch (Exception ex)
            {
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

                var user = !string.IsNullOrEmpty(confirmRequest.UserId)
                    ? _context.Users.SingleOrDefault(x => x.Id == confirmRequest.UserId)
                    : _context.Users.SingleOrDefault(x => x.Email == confirmRequest.Email);
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
                var incoming = AuthUtils.Hash(confirmRequest.Code, user.Salt);

                if (AuthUtils.SlowEquals(incoming, user.SaltedAndHashedEmail))
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
                var user = !string.IsNullOrEmpty(resetRequest.UserId)
                    ? _context.Users.SingleOrDefault(x => x.Id == resetRequest.UserId)
                    : _context.Users.SingleOrDefault(x => x.Email == resetRequest.Email);
                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_NOTFOUND,
                            new List<string> {resetRequest.UserId ?? resetRequest.Email}));
                }
                var confirmCode = AuthUtils.RandomNumString(8);
                user.SaltedAndHashedCode = AuthUtils.Hash(confirmCode, user.Salt);
                user.ResetRequested = true;
                _context.SaveChanges();

                var profile = _context.Profile.AsNoTracking().SingleOrDefault(x => x.Id == user.Id);
                using (MailSender mailSender = new MailSender())
                {
                    var bem = new BaseEmailMessage
                    {
                        Code = ConstVals.Restore,
                        ToUserId = user.Id,
                        ToUserName = profile.FirstName,
                        ToUserEmail = user.Email,
                        ConfirmCode = confirmCode
                    };
                    mailSender.Create(_context, bem);
                }
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

                    user = !string.IsNullOrEmpty(resetRequest.UserId)
                        ? _context.Users.SingleOrDefault(x => x.Id == resetRequest.UserId)
                        : _context.Users.SingleOrDefault(x => x.Email == resetRequest.Email);
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
                    var incoming = AuthUtils.Hash(resetRequest.Code, user.Salt);

                    if (!AuthUtils.SlowEquals(incoming, user.SaltedAndHashedCode))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            RespH.Create(RespH.SRV_USER_WRONG_CODE,
                                new List<string> {resetRequest.UserId ?? resetRequest.Email, resetRequest.Code}));
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
                        !AuthUtils.SlowEquals(AuthUtils.Hash(resetRequest.CurrentPassword, user.Salt),
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
                        RespH.Create(RespH.SRV_REG_INVALID_PASSWORD, new List<string> {resetRequest.Password}));
                }
                var salt = AuthUtils.GenerateSalt();
                user.Salt = salt;
                user.SaltedAndHashedPassword = AuthUtils.Hash(resetRequest.Password, salt);
                _context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK,
                    RespH.Create(RespH.SRV_USER_RESETED, new List<string> {user.Id}));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
            }
        }
    }
}