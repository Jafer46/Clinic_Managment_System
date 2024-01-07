using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALibrary.Patient
{
    public class PatientRecord
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "patient Name")]
        public int PatientId { get; set; }
        [Required]
        [Display(Name = "Leave Date")]
        [DataType(DataType.Date)]
        public DateTime LeaveDate{ get; set; }
        [Required]
        public int SessionId { get; set; }

        [ForeignKey("SessionId")]
        public virtual Medical.Session ?Session { get; set; }       


    }
}
