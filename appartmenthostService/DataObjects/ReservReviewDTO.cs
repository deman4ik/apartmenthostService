using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apartmenthostService.DataObjects
{
    public class ReservReviewDTO
    {
        public string Type { get; set; }
        public bool CanResponse { get; set; }
        public ReservationDTO Reservation { get; set; }
        public ReviewDTO OwnerReview { get; set; }
        public ReviewDTO RenterReview { get; set; }
    }
}