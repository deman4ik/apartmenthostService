using System;
using System.Diagnostics;
using apartmenthostService.Models;

namespace apartmenthostService.Messages
{
    public static class Notifications
    {
        public static void Create(IApartmenthostContext context, string userId, string type, string code,
            string favoriteId = null, string reservationId = null, string reviewId = null)
        {
            try
            {
                var notif = new Notification
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    Type = type,
                    Code = code,
                    Readed = false
                };
                if (!string.IsNullOrEmpty(favoriteId))
                    notif.FavoriteId = favoriteId;
                if (!string.IsNullOrEmpty(reservationId))
                    notif.ReservationId = reservationId;
                if (!string.IsNullOrEmpty(reviewId))
                    notif.ReviewId = reviewId;
                context.Notifications.Add(notif);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine("!!!!!CREATING NOTIFICATION EXCEPTION!!!!!!");
                Debug.WriteLine(e);
            }
        }
    }
}