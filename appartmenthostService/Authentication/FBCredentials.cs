using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Authentication
{
    public class FBCredentials : ProviderCredentials
    {
        public FBCredentials()
            : base(FBLoginProvider.ProviderName)
        {
        }

        public string AccessToken { get; set; }
    }
}
