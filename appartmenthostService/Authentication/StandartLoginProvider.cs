using System;
using System.Security.Claims;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json.Linq;
using Owin;

namespace apartmenthostService.Authentication
{
    public class StandartLoginProvider : LoginProvider
    {
        public const string ProviderName = "standart";

        public StandartLoginProvider(IServiceTokenHandler tokenHandler)
            : base(tokenHandler)
        {
            TokenLifetime = new TimeSpan(30, 0, 0, 0);
        }

        public override string Name
        {
            get { return ProviderName; }
        }

        public override void ConfigureMiddleware(IAppBuilder appBuilder, ServiceSettingsDictionary settings)
        {
            // Not Applicable - used for federated identity flows
        }

        public override ProviderCredentials ParseCredentials(JObject serialized)
        {
            if (serialized == null)
            {
                throw new ArgumentNullException("serialized");
            }

            return serialized.ToObject<StandartLoginProviderCredentials>();
        }

        public override ProviderCredentials CreateCredentials(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException("claimsIdentity");
            }

            var email = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = TokenHandler.CreateUserId(Name, email);
            //
            //User user = context.Users.SingleOrDefault(u => u.Email == email);
            var credentials = new StandartLoginProviderCredentials
            {
                UserId = userId
            };

            return credentials;
        }
    }
}