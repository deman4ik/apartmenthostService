using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Facebook;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace appartmenthostService.Authentication
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
