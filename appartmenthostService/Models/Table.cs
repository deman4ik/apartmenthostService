using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    /* 
     * Таблицы (для метаописания)
     */

    public class Table : EntityData
    {
        public Table()
        {
            Props = new HashSet<Prop>();
        }

        public string Name { get; set; }
        public ICollection<Prop> Props { get; set; }
    }
}