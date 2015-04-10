using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json.Linq;
using Owin;


namespace appartmenthostService.Authentication
{
    public class StandartLoginProvider : LoginProvider
    {
        public const string ProviderName = "standart";

        public override string Name
        {
            get { return ProviderName; }
        }

        public StandartLoginProvider(IServiceTokenHandler tokenHandler)
            : base(tokenHandler)
        {
            this.TokenLifetime = new TimeSpan(30, 0, 0, 0);
        }

        public override void ConfigureMiddleware(IAppBuilder appBuilder, ServiceSettingsDictionary settings)
        {
            // Not Applicable - used for federated identity flows
            return;
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

            string email = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            string userId = this.TokenHandler.CreateUserId(this.Name, email);
            //
            //User user = context.Users.SingleOrDefault(u => u.Email == email);
            StandartLoginProviderCredentials credentials = new StandartLoginProviderCredentials
            {
                UserId = userId
            };
            AuthUtils.CreateAccount(this.Name, email, userId, email);
            return credentials;
        }

    }
}
