using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALibrary.Patient
{
     public class RecordSummary
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [Display(Name = "Summary Discription")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }
        [Required]
        public int SessionId { get; set; }

        [ForeignKey("SessionId")]
        public virtual Medical.Session? Session{ get; set; }
    }
}
