using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    public class Article : EntityData
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Tag { get; set; }
        public string Lang { get; set; }
        public string PictureId { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
