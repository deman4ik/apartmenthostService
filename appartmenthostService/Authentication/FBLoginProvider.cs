using System;
using System.Security.Claims;
using Microsoft.Owin.Security.Facebook;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json.Linq;
using Owin;

namespace appartmenthostService.Authentication
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
            Console.WriteLine(name.Value);
            Console.WriteLine(claimsIdentity.FindFirst(ClaimTypes.Name));
            Console.WriteLine(claimsIdentity.FindFirst(ClaimTypes.Email));
            FBCredentials credentials = new FBCredentials
            {
                UserId = this.TokenHandler.CreateUserId(this.Name, name != null ?
                    name.Value : null),
                AccessToken = providerAccessToken != null ?
                    providerAccessToken.Value : null
            };

            return credentials;
        }

        public override ProviderCredentials ParseCredentials(JObject serialized)
        {
            return serialized.ToObject<FBCredentials>();
        }
    }
}
