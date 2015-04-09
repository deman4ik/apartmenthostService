using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using appartmenthostService.Models;

namespace appartmenthostService.DataObjects
{
    public class AdvertDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string UserId { get; set; }

        public string DefaultPictureId { get; set; }

        public string Type { get; set; }


        public string Description { get; set; }

        public string ApartmentId { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public virtual UserDTO User { get; set; }
        public virtual ApartmentDTO Apartment { get; set; }
    }
}