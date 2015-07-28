namespace apartmenthostService.DataObjects
{
    public class NotificationData
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CardId { get; set; }
        public string CardName { get; set; }
        public string ReservationId { get; set; }
        public string ResrvationStatus { get; set; }
        public string ReviewId { get; set; }
        public decimal ReviewRating { get; set; }
        public string ReviewText { get; set; }
    }
}