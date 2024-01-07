using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALibrary.Payment
{
    public class LabPaymentDetail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int LabPaymentId { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string? Reason { get; set; }

        [ForeignKey("LabPaymentId")]
        public virtual LabPayment?LabPayment { get; set; }
    }
}
