using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json.Linq;

namespace apartmenthostService.Controllers
{
    public class UserSocialInfoApiController : ApiController
    {
        public ApiServices Services { get; set; }

        [AuthorizeLevel(AuthorizationLevel.User)]
        public async Task<JObject> GetUserInfo()
        {
            var user = User as ServiceUser;
            if (user == null)
            {
                throw new InvalidOperationException("This can only be called by authenticated clients");
            }

            var identities = await user.GetIdentitiesAsync();
            var result = new JObject();
            var fb = identities.OfType<FacebookCredentials>().FirstOrDefault();
            if (fb != null)
            {
                var accessToken = fb.AccessToken;
                result.Add("facebook",
                    await GetProviderInfo("https://graph.facebook.com/me?access_token=" + accessToken));
            }

            return result;
        }

        private async Task<JToken> GetProviderInfo(string url)
        {
            var c = new HttpClient();
            var resp = await c.GetAsync(url);
            resp.EnsureSuccessStatusCode();
            return JToken.Parse(await resp.Content.ReadAsStringAsync());
        }
    }
}