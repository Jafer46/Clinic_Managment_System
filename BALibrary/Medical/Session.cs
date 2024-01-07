using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALibrary.Medical
{
    public class Session
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PatientId { get; set; }
        [Required]
        [Display(Name = "Session Start Time")]
        [DataType(DataType.Date)]
        public DateTime SartDateTime { get; set; }
        [Required]
        [Display(Name = "Docter's Name")]
        public int DocterId { get; set; }
        [Required]
        public int Status { get; set; }        

        [ForeignKey("PatientId")]
        public virtual Patient.PatientList ?Patient { get; set; }
        [ForeignKey("DocterId")]
        public virtual User.User ?Docter { get; set; }
        [ForeignKey("Status")]
        public virtual Other.SessionStatus ?SessionStatus { get; set; }


    }
}
