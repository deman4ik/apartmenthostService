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

        // Уникальный идентификатор изображения
        public string PictureId { get; set; }

        // Ссылка на связанное изображение
        public virtual Picture Picture { get; set; }

        // Системные поля: 
        // Id - Уникальный идентификатор записи
        // CreatedAt - Дата и время создания записи
        // UpdatedAt - Дата и время изменения записи
        // Version - Текущая версия записи
        // Deleted - Признак удаленной записи
    }
}
