using System;
using System.Security.Claims;
using apartmenthostService.Models;
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
        public override string Name => ProviderName;

        /// <summary>
        /// </summary>
        /// <param name="appBuilder"></param>
        /// <param name="settings"></param>
        public override void ConfigureMiddleware(IAppBuilder appBuilder,
            ServiceSettingsDictionary settings)
        {
            var options = new FacebookAuthenticationOptions
            {
                AppId = Environment.GetEnvironmentVariable("FBAppId"),
                AppSecret = Environment.GetEnvironmentVariable("FBAppSecret"),
                AuthenticationType = Name,
                Provider = new FBLoginAuthenticationProvider(),
                Scope = {"email"}
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
            try
            {
                var providerAccessToken = claimsIdentity
                    .FindFirst(ServiceClaimTypes.ProviderAccessToken);
                if (providerAccessToken == null) return null;
                var name = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (name == null) return null;
                var userId = TokenHandler.CreateUserId(Name, name?.Value);
                if (userId == null) return null;
                var emailClaim = claimsIdentity.FindFirst(ClaimTypes.Email);
                var nameClaim = claimsIdentity.FindFirst(ClaimTypes.Name);
                var credentials = new FBCredentials
                {
                    UserId = userId,
                    AccessToken = providerAccessToken?.Value
                };
                AuthUtils.CreateAccount(new ApartmenthostContext(), Name, name?.Value, userId, emailClaim?.Value,
                    nameClaim?.Value);
                return credentials;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
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