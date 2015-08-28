using System;

namespace apartmenthostService.DataObjects
{
    public class ArticleDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public string Lang { get; set; }
        public string PictureId { get; set; }
        public PictureDTO Picture { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}