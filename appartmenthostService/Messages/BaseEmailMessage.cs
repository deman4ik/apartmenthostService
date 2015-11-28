using System;

namespace apartmenthostService.Messages
{
    public class BaseEmailMessage
    {
        public string Code { get; set; }
        public string ToUserId { get; set; }
        public string ToUserName { get; set; }
        public string ToUserEmail { get; set; }
        public string FromUserId { get; set; }
        public string FromUserName { get; set; }
        public string FromUserEmail { get; set; }
        public string ReviewText { get; set; }
        public decimal ReviewRating { get; set; }
        public string ConfirmCode { get; set; }
        public DateTimeOffset? DateFrom { get; set; }
        public DateTimeOffset? DateTo { get; set; }
        public string CardId { get; set; }
        public string CardName { get; set; }
        public string Text { get; set; }
        public bool AnswerByEmail { get; set; }
    }
}