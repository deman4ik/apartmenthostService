using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace appartmenthostService.Models
{
    public class ReviewComment : EntityData
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string ReviewId { get; set; }
        [Required]
        public string Text { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("ReviewId")]
        public virtual Review Review { get; set; }
    }
}
