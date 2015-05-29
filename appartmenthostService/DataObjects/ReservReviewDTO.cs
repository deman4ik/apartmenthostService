using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apartmenthostService.DataObjects
{
    public class ReservReviewDTO
    {
        public ReservationDTO Reservation { get; set; }
        public ReviewDTO LessorReview { get; set; }
        public ReviewDTO RenterReview { get; set; }
    }
}