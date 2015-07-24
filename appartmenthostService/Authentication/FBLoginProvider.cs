using System;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.Owin.Security.Facebook;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json.Linq;
using Owin;

namespace apartmenthostService.Authentication
{
    public class FBLoginProvider : LoginProvider
    {
        internal const string ProviderName = "FB";

        public FBLoginProvider(IServiceTokenHandler tokenHandler)
            : base(tokenHandler)
        {
        }

        public override string Name
        {
            get { return ProviderName; }
        }

        public override void ConfigureMiddleware(IAppBuilder appBuilder,
            ServiceSettingsDictionary settings)
        {
            FacebookAuthenticationOptions options = new FacebookAuthenticationOptions
            {
                AppId = settings["FBAppId"],
                AppSecret = settings["FBAppSecret"],
                AuthenticationType = this.Name,
                Provider = new FBLoginAuthenticationProvider()
            };
            appBuilder.UseFacebookAuthentication(options);
        }

        public override ProviderCredentials CreateCredentials(
            ClaimsIdentity claimsIdentity)
        {

                Claim name = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                Claim providerAccessToken = claimsIdentity
                    .FindFirst(ServiceClaimTypes.ProviderAccessToken);
                string email = claimsIdentity.FindFirst(ClaimTypes.Name).ToString();
                string userId = this.TokenHandler.CreateUserId(this.Name, name != null
                    ? name.Value
                    : null);
                FBCredentials credentials = new FBCredentials
                {
                    UserId = userId,
                    AccessToken = providerAccessToken != null
                        ? providerAccessToken.Value
                        : null
                };
                AuthUtils.CreateAccount(this.Name, userId, name.Value, email);
                return credentials;

        }

        public override ProviderCredentials ParseCredentials(JObject serialized)
        {
            return serialized.ToObject<FBCredentials>();
        }
    }
}
