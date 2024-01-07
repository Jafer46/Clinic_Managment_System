using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALibrary.Payment
{
    public class LabPayment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int LabRequestId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public bool Checked { get; set; } = false;

        [ForeignKey("LabRequestId")]
        public virtual Medical.LabRequest? LabRequest { get; set; }

    }
}
