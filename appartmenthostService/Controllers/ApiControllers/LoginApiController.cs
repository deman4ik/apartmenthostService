using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using apartmenthostService.Authentication;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
   // [AuthorizeLevel(AuthorizationLevel.Application)]
    public class StandartLoginController : ApiController
    {
        private readonly IApartmenthostContext _context = new ApartmenthostContext();

        public StandartLoginController()
        {
        }

        public StandartLoginController(IApartmenthostContext context)
        {
            _context = context;
        }

        public ApiServices Services { get; set; }
        public IServiceTokenHandler Handler { get; set; }
        // POST api/CustomLogin
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        public HttpResponseMessage Post(LoginRequest loginRequest)
        {
            var user = _context.Users.AsNoTracking().SingleOrDefault(a => a.Email == loginRequest.email);
            if (user != null)
            {
                if (user.Blocked)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_USER_BLOCKED));

                var incoming = AuthUtils.Hash(loginRequest.password, user.Salt);

                if (AuthUtils.SlowEquals(incoming, user.SaltedAndHashedPassword))
                {
                    var claimsIdentity = new ClaimsIdentity();
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, loginRequest.email));
                    var loginResult = new StandartLoginProvider(Handler).CreateLoginResult(claimsIdentity,
                        Services.Settings.MasterKey);
                    return Request.CreateResponse(HttpStatusCode.OK, loginResult);
                }
                return Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_LOGIN_INVALID_PASS));
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_LOGIN_INVALID_EMAIL));
        }

        [Route("api/AdminLogin")]
        [AuthorizeLevel(AuthorizationLevel.Admin)]
        public HttpResponseMessage LoginAdmin(LoginRequest loginRequest)
        {
            var admin = _context.Admins.AsNoTracking().SingleOrDefault(a => a.Email == loginRequest.email);
            if (admin != null)
            {
                var incoming = AuthUtils.Hash(loginRequest.password, admin.Salt);

                if (AuthUtils.SlowEquals(incoming, admin.SaltedAndHashedPassword))
                {
                    var claimsIdentity = new ClaimsIdentity();
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, loginRequest.email));
                    var loginResult = new StandartLoginProvider(Handler).CreateLoginResult(claimsIdentity,
                        Services.Settings.MasterKey);
                    return Request.CreateResponse(HttpStatusCode.OK, loginResult);
                }
                return Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_LOGIN_INVALID_PASS));
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_LOGIN_INVALID_EMAIL));
        }
    }
}