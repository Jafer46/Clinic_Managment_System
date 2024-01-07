using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALibrary.Medical
{
    public class LabReportDetail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int LabReportId { get; set; }
        [Required]
        public string? Case { get; set; }
        [Display(Name = "Case Detail")]
        [DataType(DataType.MultilineText)]
        public string? CaseDetail { get; set; }

        [ForeignKey("LabReportId")]
        public virtual LabReport? LabReport { get; set; }
    }
}
