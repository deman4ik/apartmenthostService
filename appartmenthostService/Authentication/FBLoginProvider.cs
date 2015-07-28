using System.Security.Claims;
using Microsoft.Owin.Security.Facebook;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json.Linq;
using Owin;

namespace apartmenthostService.Authentication
{
    /// <summary>
    /// </summary>
    public class FBLoginProvider : LoginProvider
    {
        internal const string ProviderName = "FB";

        /// <summary>
        /// </summary>
        /// <param name="tokenHandler"></param>
        public FBLoginProvider(IServiceTokenHandler tokenHandler)
            : base(tokenHandler)
        {
        }

        /// <summary>
        /// </summary>
        public override string Name
        {
            get { return ProviderName; }
        }

        /// <summary>
        /// </summary>
        /// <param name="appBuilder"></param>
        /// <param name="settings"></param>
        public override void ConfigureMiddleware(IAppBuilder appBuilder,
            ServiceSettingsDictionary settings)
        {
            var options = new FacebookAuthenticationOptions
            {
                AppId = settings["FBAppId"],
                AppSecret = settings["FBAppSecret"],
                AuthenticationType = Name,
                Provider = new FBLoginAuthenticationProvider()
            };
            appBuilder.UseFacebookAuthentication(options);
        }

        /// <summary>
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <returns></returns>
        public override ProviderCredentials CreateCredentials(
            ClaimsIdentity claimsIdentity)
        {
            var name = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var providerAccessToken = claimsIdentity
                .FindFirst(ServiceClaimTypes.ProviderAccessToken);
            var userId = TokenHandler.CreateUserId(Name, name?.Value);
            var credentials = new FBCredentials
            {
                UserId = userId,
                AccessToken = providerAccessToken?.Value
            };
            AuthUtils.CreateAccount(Name, userId, name.Value);
            return credentials;
        }

        /// <summary>
        /// </summary>
        /// <param name="serialized"></param>
        /// <returns></returns>
        public override ProviderCredentials ParseCredentials(JObject serialized)
        {
            return serialized.ToObject<FBCredentials>();
        }
    }
}