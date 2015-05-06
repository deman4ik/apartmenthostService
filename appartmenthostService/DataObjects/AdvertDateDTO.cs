using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apartmenthostService.Attributes;
using apartmenthostService.Models;

namespace apartmenthostService.DataObjects
{
    public class AdvertDateDTO
    {
        // [GET][PUT][DELETE] - {Uniq}{NN} - Уникальный идентификатор(Advert)
        [Metadata(Visible = false, Required = false)]
        public string Id { get; set; }

        public string AdvertId { get; set; }

        public string ReservationId { get; set; }

        public DateTimeOffset Date { get; set; }

        // [GET] - Дата и Время создания объекта
        [Metadata(Visible = false, Required = false)]
        public DateTimeOffset? CreatedAt { get; set; }

        // [GET] - Дата и Время изменения объекта
        [Metadata(Visible = false, Required = false)]
        public DateTimeOffset? UpdatedAt { get; set; }

        public virtual Advert Advert { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}
