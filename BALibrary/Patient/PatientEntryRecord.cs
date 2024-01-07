using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALibrary.Patient
{
    public class PatientEntryRecord
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Patient ID is required")]
        [Display(Name = "Patient Name")]
        public int PatientId { get; set; }
        [Required]
        [Display(Name = "Entry Date")]
        [DataType(DataType.Date)]
        public DateTime EntryDate { get; set; }
        [Display(Name = "Age at Entry")]
        public int Age { get; set; }
        [Required]
        public int SessionId { get; set; }


        [ForeignKey("SessionId")]
        public virtual Medical.Session ?Session { get; set; }
    }
}
