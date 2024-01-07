using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALibrary.Patient
{
    public class PatientList
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter first name")]
        [Display(Name = "Frirst Name")]
        public string? FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [Required]
        public string? Gender { get; set; }
        [Display(Name = "Contact or Phone No")]
        public string? Contact { get; set; }
        [Required]
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}
