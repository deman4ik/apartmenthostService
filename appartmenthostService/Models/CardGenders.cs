using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class CardGenders : EntityData
    {
        
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string CardId { get; set; }
        public virtual Card Card { get; set; }
    }
}
