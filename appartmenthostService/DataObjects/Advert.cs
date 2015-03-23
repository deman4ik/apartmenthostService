using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.DataObjects
{
    public class Advert : EntityData
    {
        public string Text { get; set; }
        public string UserId { get; set; }


    }
}
