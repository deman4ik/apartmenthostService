using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    /* 
     * Значения словарей
     */
    public class DictionaryItem : EntityData
    {
        public DictionaryItem()
        {
            this.PropVals = new HashSet<PropVal>();
        }
        public string DictionaryId { get; set; }
        public string StrValue { get; set; }
        public decimal? NumValue { get; set; }
        public DateTime? DateValue { get; set; }
        public bool? BoolValue { get; set; }

        public string Lang { get; set; }

        public virtual Dictionary Dictionary { get; set; }
        public ICollection<PropVal> PropVals { get; set; }

        // Системные поля: 
        // Id - Уникальный идентификатор записи
        // CreatedAt - Дата и время создания записи
        // UpdatedAt - Дата и время изменения записи
        // Version - Текущая версия записи
        // Deleted - Признак удаленной записи
    }
}