using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Review : EntityData
    {

        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string ReservationId { get; set; }
        public string Text { get; set; }
        public double Rating { get; set; }

        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
        public virtual Reservation Reservation { get; set; }



    }
}
