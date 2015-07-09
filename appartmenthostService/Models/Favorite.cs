using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    /* 
     * Избранные карточки объявлений
     */
    public class Favorite : EntityData
    {
        public string UserId { get; set; }
        public string CardId { get; set; }

        public virtual User User { get; set; }
        public virtual Card Card { get; set; }

        public ICollection<Notification> Notifications { get; set; }

        // Системные поля: 
        // Id - Уникальный идентификатор записи
        // CreatedAt - Дата и время создания записи
        // UpdatedAt - Дата и время изменения записи
        // Version - Текущая версия записи
        // Deleted - Признак удаленной записи

    }
}
