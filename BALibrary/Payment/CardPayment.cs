using BALibrary.Medical;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALibrary.Payment
{
    public class CardPayment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public bool Checked { get; set; } = false;
        [Required]
        public int SessionId { get; set; }

        [ForeignKey("SessionId")]
        public virtual Session? Session { get; set; }

    }
}
