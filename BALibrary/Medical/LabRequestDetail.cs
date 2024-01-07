using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALibrary.Medical
{
    public class LabRequestDetail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int LabRequestId { get; set; }
        [Required]
        public string? Case { get; set; }
        [Required]
        [Display(Name = "Case Detail")]
        [DataType(DataType.MultilineText)]
        public string? CaseDetail{ get; set; }

        [ForeignKey("LabRequestId")]
        public virtual LabRequest? LabRequest { get; set; }  
    }
}
