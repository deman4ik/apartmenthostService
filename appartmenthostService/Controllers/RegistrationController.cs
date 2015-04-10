using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using appartmenthostService.Authentication;
using appartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace appartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Anonymous)]
    public class StandartRegistrationController : ApiController
    {
        public ApiServices Services { get; set; }

        // POST api/CustomRegistration
        public HttpResponseMessage Post(RegistrationRequest registrationRequest)
        {
            if (!StandartLoginProviderUtils.IsEmailValid(registrationRequest.email))
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid email");
            }
            else if (registrationRequest.password.Length < 8)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid password (at least 8 chars required)");
            }

            appartmenthostContext context = new appartmenthostContext();
            User user = context.Users.SingleOrDefault(a => a.Email == registrationRequest.email);
            if (user != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "User with such email already exists");
            }
            else
            {
                byte[] salt = StandartLoginProviderUtils.generateSalt();
                User newUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = registrationRequest.email,
                    Salt = salt,
                    SaltedAndHashedPassword = StandartLoginProviderUtils.hash(registrationRequest.password, salt)
                };
                context.Users.Add(newUser);
                context.SaveChanges();
                return this.Request.CreateResponse(HttpStatusCode.Created);
            }
        }
    }
}
