using System;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    /*
     * Даты недоступности карточки объявления
     */

    public class CardDates : EntityData
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string CardId { get; set; }
        public virtual Card Card { get; set; }
    }
}