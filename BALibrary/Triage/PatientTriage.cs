using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALibrary.Triage
{
    public class PatientTriage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SessionId { get; set; }
        public int Priority { get; set; } = 0;

        [ForeignKey("SessionId")]
        public virtual Medical.Session? Session { get; set; }

    }
}
