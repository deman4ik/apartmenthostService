using System.Security.Claims;
using System.Threading.Tasks;
using KatanaContrib.Security.VK;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Authentication
{
    public class VKLoginAuthenticationProvider : VkAuthenticationProvider
    {
        public override Task Authenticated(VkAuthenticatedContext context)
        {
            context.Identity.AddClaim(
                new Claim(ServiceClaimTypes.ProviderAccessToken, context.AccessToken));
            return base.Authenticated(context);
        }
    }
}