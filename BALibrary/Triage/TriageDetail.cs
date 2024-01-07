using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALibrary.Triage
{
    public class TriageDetail
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public int PatientTriageId { get; set; }
        [Required]
        public string? Case { get; set;}
        [Required]
        [DataType(DataType.MultilineText)]
        public string? CaseDetail { get; set;}

        [ForeignKey("PatientTriageId")]
        public virtual PatientTriage? PatientTriage { get; set; }

    }
}
