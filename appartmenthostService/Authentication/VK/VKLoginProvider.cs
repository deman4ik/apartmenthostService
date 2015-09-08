using System;
using System.Security.Claims;
using KatanaContrib.Security.VK;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json.Linq;
using Owin;

namespace apartmenthostService.Authentication
{
    public class VKLoginProvider : LoginProvider
    {
        internal const string ProviderName = "VK";

        /// <summary>
        /// </summary>
        /// <param name="tokenHandler"></param>
        public VKLoginProvider(IServiceTokenHandler tokenHandler)
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
            var options = new VkAuthenticationOptions
            {
                ClientId = settings["VKClientId"],
                ClientSecret = settings["VKClientSecret"],
                Provider = new VKLoginAuthenticationProvider(),
                AuthenticationType = this.Name,
                Scope = {"email"}
            };
            appBuilder.
                UseVkontakteAuthentication(options);
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
                if (name == null ) return null;
                var userId = TokenHandler.CreateUserId(Name, name?.Value);
                if (userId == null) return null;
                var emailClaim = claimsIdentity.FindFirst(ClaimTypes.Email);
                var nameClaim = claimsIdentity.FindFirst(ClaimTypes.Name);

                var credentials = new VKCredentials
                {
                    UserId = userId,
                    AccessToken = providerAccessToken?.Value
                };
                if (name != null)
                    AuthUtils.CreateAccount(Name, name.Value, userId, emailClaim?.Value, nameClaim?.Value
                        );
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
            return serialized.ToObject<VKCredentials>();
        }
    }
}