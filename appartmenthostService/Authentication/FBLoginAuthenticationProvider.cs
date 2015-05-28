using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Facebook;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Authentication
{
    public class FBLoginAuthenticationProvider : FacebookAuthenticationProvider
    {
        public override Task Authenticated(FacebookAuthenticatedContext context)
        {
            context.Identity.AddClaim(
                new Claim(ServiceClaimTypes.ProviderAccessToken, context.AccessToken));
            return base.Authenticated(context);
        }
    }
}
