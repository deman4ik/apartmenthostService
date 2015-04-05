﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.DataObjects
{
    public class User : EntityData
    {
        public User() 
        {
            this.Apartments = new List<Apartment>();
            this.Adverts = new List<Advert>();
            this.Reservations = new List<Reservation>();
        }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] Salt { get; set; }
        public byte[] SaltedAndHashedPassword { get; set; }

        public virtual Profile Profile { get; set; }
        public ICollection<SocialAccount> SocialAccounts { get; set; } 
        public ICollection<Apartment> Apartments { get; set; }
        public ICollection<Advert> Adverts { get; set; }
        public ICollection<Reservation> Reservations { get; set; } 
    }
}
