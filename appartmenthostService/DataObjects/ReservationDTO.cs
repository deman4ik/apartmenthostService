﻿using System;
using System.Collections.Generic;

namespace apartmenthostService.DataObjects
{
    public class ReservationDTO
    {
        public string Id { get; set; }

        public string CardId { get; set; }

        public string UserId { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public string Gender { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public virtual BaseUserDTO User { get; set; }

        public virtual CardDTO Card { get; set; }

        // Отзывы
        public ICollection<ReviewDTO> Reviews { get; set; }
    }
}