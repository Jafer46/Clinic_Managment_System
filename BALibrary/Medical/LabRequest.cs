using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALibrary.Medical
{
    public class LabRequest
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SessionId { get; set; }

        [ForeignKey("SessionId")]
        public virtual Session? Session { get; set; }
    }
}
