using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    /*
     * Статьи
     */

    public class Article : EntityData
    {
        // Уникальный код статьи (например "HOW_IT_WORKS")
        public string Name { get; set; }
        // Заголовок статьи
        public string Title { get; set; }
        // Содержимое статьи
        public string Text { get; set; }
        // Теги статьи (не используется)
        public string Tag { get; set; }
        // Язык статьи "RU", "EN"
        public string Lang { get; set; }
    }
}