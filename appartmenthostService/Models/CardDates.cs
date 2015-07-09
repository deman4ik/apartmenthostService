using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    /*
     * Даты недоступности карточки объявления
     */
    public class CardDates : EntityData
    {
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string CardId { get; set; }

        public virtual Card Card { get; set; }

        // Системные поля: 
        // Id - Уникальный идентификатор записи
        // CreatedAt - Дата и время создания записи
        // UpdatedAt - Дата и время изменения записи
        // Version - Текущая версия записи
        // Deleted - Признак удаленной записи
    }
}