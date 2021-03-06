﻿using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    /* 
     * Бронирования
     */

    public class Reservation : EntityData
    {
        public string CardId { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public string Gender { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public virtual Card Card { get; set; }
        public virtual User User { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}