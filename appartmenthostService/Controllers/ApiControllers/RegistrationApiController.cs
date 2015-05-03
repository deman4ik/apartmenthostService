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
    public class StandartRegistrationController : ApiController
    {
        public ApiServices Services { get; set; }

        // POST api/CustomRegistration
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        public HttpResponseMessage Post(RegistrationRequest registrationRequest)
        {
            var respList = new List<string>();
            if (!AuthUtils.IsEmailValid(registrationRequest.email))
            {
                respList.Add(registrationRequest.email);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_REG_INVALID_EMAIL, respList));
            }
            else if (registrationRequest.password.Length < 8)
            {
                respList.Add(registrationRequest.password);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_REG_INVALID_PASSWORD, respList));
            }

            apartmenthostContext context = new apartmenthostContext();
            User user = context.Users.SingleOrDefault(a => a.Email == registrationRequest.email);
            if (user != null)
            {
                respList.Add(registrationRequest.email);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_REG_EXISTS_EMAIL, respList));
            }
            else
            {
                byte[] salt = AuthUtils.generateSalt();
                User newUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = registrationRequest.email,
                    Salt = salt,
                    SaltedAndHashedPassword = AuthUtils.hash(registrationRequest.password, salt)
                };
                context.Users.Add(newUser);
                context.SaveChanges();
                respList.Add(newUser.Id);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
            }
        }
    }
}
