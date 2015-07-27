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
        public ApiServices Services { get; set; }
        readonly apartmenthostContext _context = new apartmenthostContext();
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
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_REG_INVALID_EMAIL, respList));
                }
                else if (registrationRequest.Password.Length < 8)
                {
                    respList.Add(registrationRequest.Password);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_REG_INVALID_PASSWORD, respList));
                }

                
                User user = _context.Users.SingleOrDefault(a => a.Email == registrationRequest.Email);
                if (user != null)
                {
                    respList.Add(registrationRequest.Email);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_REG_EXISTS_EMAIL, respList));
                }
                else
                {
                    byte[] salt = AuthUtils.generateSalt();
                    string confirmCode = AuthUtils.randomNumString(6);
                    User newUser = new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = registrationRequest.Email,
                        Salt = salt,
                        SaltedAndHashedPassword = AuthUtils.hash(registrationRequest.Password, salt),
                        SaltedAndHashedEmail = AuthUtils.hash(confirmCode, salt)
                    };
                    _context.Users.Add(newUser);
                    _context.SaveChanges();
                    AuthUtils.CreateAccount(StandartLoginProvider.ProviderName, registrationRequest.Email, newUser.Id,
                        registrationRequest.Email);
                    Notifications.SendEmail(_context, newUser.Id, ConstVals.General, ConstVals.Reg, null, null, null,
                        confirmCode);
                    respList.Add(newUser.Id);
                    return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.ToString() }));
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
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_REQUIRED, new List<string>() { "code" }));
                }
                var user = _context.Users.SingleOrDefault(x => x.Id == confirmRequest.UserId || x.Email == confirmRequest.Email);
                if (user == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_NOTFOUND, new List<string>() { confirmRequest.UserId ?? confirmRequest.Email }));
                }

                if (user.EmailConfirmed)
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_CONFIRMED, new List<string>() { confirmRequest.UserId }));  
                }
                byte[] incoming = AuthUtils.hash(confirmRequest.Code, user.Salt);

                if (AuthUtils.slowEquals(incoming, user.SaltedAndHashedEmail))
                {
                    user.EmailConfirmed = true;
                    user.SaltedAndHashedEmail = null;
                    _context.SaveChanges();
                    return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_USER_CONFIRMED, new List<string>() { user.Id }));
                }

                return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_WRONG_CODE, new List<string>() { confirmRequest.UserId ?? confirmRequest.Code }));
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.ToString() }));
            }
        }

        [Route("api/PasswordReset")]
        [AuthorizeLevel(AuthorizationLevel.Application)]
        [HttpPost]
        public HttpResponseMessage ResetPassword(ConfirmRequest resetRequest)
        {
            try
            {


                var user = _context.Users.SingleOrDefault(x => x.Id == resetRequest.UserId || x.Email == resetRequest.Email);
                if (user == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_NOTFOUND, new List<string>() { resetRequest.UserId ?? resetRequest.Email }));
                }
                string confirmCode = AuthUtils.randomNumString(8);
                user.SaltedAndHashedCode = AuthUtils.hash(confirmCode, user.Salt);
                user.ResetRequested = true;
                _context.SaveChanges();
                Notifications.SendEmail(_context, user.Id, ConstVals.General, ConstVals.Restore, null, null, null,
                        confirmCode);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_USER_RESET_REQUESTED, new List<string>() { user.Id }));

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.ToString() }));
            }
        }

        [Route("api/PasswordChange")]
        [AuthorizeLevel(AuthorizationLevel.Application)]
        [HttpPost]
        public HttpResponseMessage ChangePassword(ConfirmRequest resetRequest)
        {
            try
            {

   
                if (resetRequest.Code == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_REQUIRED, new List<string>() { "code" }));
                }
                if (string.IsNullOrWhiteSpace(resetRequest.Password))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_REQUIRED, new List<string>() { "password" }));
                }
                var user = _context.Users.SingleOrDefault(x => x.Id == resetRequest.UserId || x.Email == resetRequest.Email);
                if (user == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_NOTFOUND, new List<string>() { resetRequest.UserId ?? resetRequest.Email }));
                }
                if (!user.ResetRequested)
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_RESET_NOT_REQUESTED, new List<string>() { resetRequest.UserId ?? resetRequest.Email }));
                }
                byte[] incoming = AuthUtils.hash(resetRequest.Code, user.Salt);

                if (AuthUtils.slowEquals(incoming, user.SaltedAndHashedCode))
                {
                    byte[] salt = AuthUtils.generateSalt();
                    user.Salt = salt;
                    user.SaltedAndHashedPassword = AuthUtils.hash(resetRequest.Password, salt);
                    user.SaltedAndHashedCode = null;
                    user.ResetRequested = false;
                    _context.SaveChanges();
                    return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_USER_RESETED, new List<string>() { user.Id }));
                }

                return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_WRONG_CODE, new List<string>() { resetRequest.UserId ?? resetRequest.Email, resetRequest.Code }));
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.ToString() }));
            }
        }
    }
}
