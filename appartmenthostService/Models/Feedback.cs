using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Feedback : EntityData
    {
        public string UserId { get; set; }
        public string AbuserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public bool AnswerByEmail { get; set; }

        public virtual User User { get; set; }
        public virtual User Abuser { get; set; }
    }
}