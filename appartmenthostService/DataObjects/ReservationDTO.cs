using System;
using System.Collections.Generic;
using apartmenthostService.Attributes;
using apartmenthostService.Helpers;

namespace apartmenthostService.DataObjects
{
    public class ReservationDTO
    {
        [Metadata(DataType = ConstDataType.Id)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        public string Id { get; set; }

        [Metadata(DataType = ConstDataType.Id)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string CardId { get; set; }

        [Metadata(DataType = ConstDataType.Id)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string UserId { get; set; }

        [Metadata(DataType = ConstDataType.List)]
        [GetRule(Order = 1, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 1, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        [PutRule(Order = 1, RequiredForm = false, RequiredTransfer = true, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Status { get; set; }

        [Metadata(DataType = ConstDataType.Text)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public string Type { get; set; }

        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 2, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 2, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 2, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTime DateFrom { get; set; }

        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 3, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 3, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [PutRule(Order = 3, RequiredForm = true, RequiredTransfer = true, Visible = true)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTime DateTo { get; set; }

        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 4, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTimeOffset? CreatedAt { get; set; }

        [Metadata(DataType = ConstDataType.Date)]
        [GetRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public DateTimeOffset? UpdatedAt { get; set; }

        [Metadata(DataType = ConstDataType.User)]
        [GetRule(Order = 5, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public virtual BaseUserDTO User { get; set; }

        [Metadata(DataType = ConstDataType.Card)]
        [GetRule(Order = 6, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public virtual CardDTO Card { get; set; }

        // Отзывы
        [Metadata(DataType = ConstDataType.Reviews)]
        [GetRule(Order = 7, RequiredForm = false, RequiredTransfer = false, Visible = true)]
        [PostRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [PutRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        [DeleteRule(Order = 0, RequiredForm = false, RequiredTransfer = false, Visible = false)]
        public ICollection<ReviewDTO> Reviews { get; set; }
    }
}