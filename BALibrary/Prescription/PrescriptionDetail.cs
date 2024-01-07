using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALibrary.Prescription
{
    public class PrescriptionDetail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PrescriptionId { get; set; }
        [Required]
        public string? MedicineName { get; set; }
        public string? Brand { get; set; }
        public string? PrescribedAmount { get; set; }
        [DataType(DataType.MultilineText)]
        public string? OtherDetail { get; set; }

        [ForeignKey("PrescriptionId")]
        public virtual Prescription? Prescription { get; set; }
    }
}
