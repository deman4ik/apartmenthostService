using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    /*
     * Словари
     */ 
    public class Dictionary : EntityData
    {
        public Dictionary()
        {
            this.DictionaryItems = new HashSet<DictionaryItem>();
            this.Props = new HashSet<Prop>();
        }
        public string Name { get; set; }

        public ICollection<DictionaryItem> DictionaryItems { get; set; }
        public ICollection<Prop> Props { get; set; }

        // Системные поля: 
        // Id - Уникальный идентификатор записи
        // CreatedAt - Дата и время создания записи
        // UpdatedAt - Дата и время изменения записи
        // Version - Текущая версия записи
        // Deleted - Признак удаленной записи

    }
}