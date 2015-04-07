using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using appartmenthostService.Authentication;
using appartmenthostService.Models;
using appartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace appartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Anonymous)]
    public class StandartLoginController : ApiController
    {
        public ApiServices Services { get; set; }
        public IServiceTokenHandler handler { get; set; }

        // POST api/CustomLogin
        public HttpResponseMessage Post(LoginRequest loginRequest)
        {
           appartmenthostContext context = new appartmenthostContext();
            
            User user = context.Users.Where(a => a.Email == loginRequest.email).SingleOrDefault();
            if (user != null)
            {
                byte[] incoming = StandartLoginProviderUtils.hash(loginRequest.password, user.Salt);

                if (StandartLoginProviderUtils.slowEquals(incoming, user.SaltedAndHashedPassword))
                {
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity();
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, loginRequest.email));
                    LoginResult loginResult = new StandartLoginProvider(handler).CreateLoginResult(claimsIdentity, Services.Settings.MasterKey);
                    return this.Request.CreateResponse(HttpStatusCode.OK, loginResult);
                }
            }
            return this.Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid email or password");
        }
    }
}
