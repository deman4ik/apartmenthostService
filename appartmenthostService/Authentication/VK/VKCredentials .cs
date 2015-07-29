using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
