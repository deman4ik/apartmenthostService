using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Admin : EntityData
    {
        public string Email { get; set; }
        public byte[] Salt { get; set; }
        public byte[] SaltedAndHashedPassword { get; set; }
    }
}