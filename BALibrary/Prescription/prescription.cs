using BALibrary.Medical;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALibrary.Prescription
{
    public class Prescription
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Prescription Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public int SessionId { get; set; }

        [ForeignKey("SessionId")]
        public Medical.Session? Session { get; set; }
    }
}
