using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class User : EntityData
    {
        /*
         * Пользователи
         */

        public User()
        {
            Notifications = new HashSet<Notification>();
            Favorites = new HashSet<Favorite>();
            Accounts = new HashSet<Account>();
            Apartments = new HashSet<Apartment>();
            Cards = new HashSet<Card>();
            Reservations = new HashSet<Reservation>();
            OutReviews = new HashSet<Review>();
            InReviews = new HashSet<Review>();
        }

        public string Email { get; set; }
        public byte[] Salt { get; set; }
        public byte[] SaltedAndHashedPassword { get; set; }
        public byte[] SaltedAndHashedEmail { get; set; }
        public byte[] SaltedAndHashedCode { get; set; }
        public byte[] SaltedAndHashedSmsCode { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneStatus { get; set; }
        public DateTime? PhoneCodeRequestedAt { get; set; }
        public bool ResetRequested { get; set; }
        public bool Blocked { get; set; }
        public bool EmailNewsletter { get; set; }
        public bool EmailNotifications { get; set; }
        public virtual Profile Profile { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Account> Accounts { get; set; }
        public ICollection<Apartment> Apartments { get; set; }
        public ICollection<Card> Cards { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Review> OutReviews { get; set; }
        public ICollection<Review> InReviews { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<Feedback> Appeals { get; set; }
    }
}