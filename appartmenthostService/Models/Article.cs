using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Article : EntityData
    {
        public string Name { get; set; }
        public string Text { get; set; }
    }
}
