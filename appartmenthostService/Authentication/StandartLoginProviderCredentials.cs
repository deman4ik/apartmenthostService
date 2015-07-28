using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Authentication
{
    public class StandartLoginProviderCredentials : ProviderCredentials
    {
        public StandartLoginProviderCredentials()
            : base(StandartLoginProvider.ProviderName)
        {
        }
    }
}