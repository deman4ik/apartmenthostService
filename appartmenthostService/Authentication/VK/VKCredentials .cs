using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Authentication
{
    public class VKCredentials : ProviderCredentials
    {
        public VKCredentials()
            : base(VKLoginProvider.ProviderName)
        {
        }

        public string AccessToken { get; set; }
    }
}