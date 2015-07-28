using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    /*
     * Оповещения
     */

    public class Notification : EntityData
    {
        public string UserId { get; set; }
        public string ReservationId { get; set; }
        public string CardId { get; set; }
        public string ReviewId { get; set; }
        public string FavoriteId { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public bool Emailed { get; set; }
        public bool Readed { get; set; }
        public virtual User User { get; set; }
        public virtual Reservation Reservation { get; set; }
        public virtual Card Card { get; set; }
        public virtual Review Review { get; set; }
        public virtual Favorite Favorite { get; set; }
    }
}