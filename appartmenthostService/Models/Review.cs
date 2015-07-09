using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    /*
     * Отзывы и рейтинги
     */
    public class Review : EntityData
    {

        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string ReservationId { get; set; }
        public string Text { get; set; }
        public decimal Rating { get; set; }

        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
        public virtual Reservation Reservation { get; set; }

        public ICollection<Notification> Notifications { get; set; }

        // Системные поля: 
        // Id - Уникальный идентификатор записи
        // CreatedAt - Дата и время создания записи
        // UpdatedAt - Дата и время изменения записи
        // Version - Текущая версия записи
        // Deleted - Признак удаленной записи
    }
}
