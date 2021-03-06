﻿using Newtonsoft.Json;

namespace apartmenthostService.DataObjects
{
    public class ReservReviewDTO
    {
        public string Type { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool CanResponse { get; set; }

        public ReservationDTO Reservation { get; set; }
        public ReviewDTO OwnerReview { get; set; }
        public ReviewDTO RenterReview { get; set; }
    }
}