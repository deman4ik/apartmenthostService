using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class StandartLoginController : ApiController
    {
        public ApiServices Services { get; set; }
        public IServiceTokenHandler handler { get; set; }

        // POST api/CustomLogin
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        public HttpResponseMessage Post(LoginRequest loginRequest)
        {
           appartmenthostContext context = new appartmenthostContext();
            
            User user = context.Users.SingleOrDefault(a => a.Email == loginRequest.email);
            if (user != null)
            {
                byte[] incoming = AuthUtils.hash(loginRequest.password, user.Salt);

                if (AuthUtils.slowEquals(incoming, user.SaltedAndHashedPassword))
                {
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity();
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, loginRequest.email));
                    LoginResult loginResult = new StandartLoginProvider(handler).CreateLoginResult(claimsIdentity, Services.Settings.MasterKey);
                    return this.Request.CreateResponse(HttpStatusCode.OK, loginResult);
                }
            }
            return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.LOGIN_INVALID_EMAIL_PASS));
        }
    }
}
