using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apartmenthostService.DataObjects
{
    public class CardResponseDTO
    {
        public int Count { get; set; }
        public List<CardDTO> Cards { get; set; } 
    }
}