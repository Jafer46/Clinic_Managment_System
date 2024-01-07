using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALibrary.Admin
{
    public class ModuleTable
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? TableName { get; set; }
        [Required]
        public int ModuleId { get; set; }

        [ForeignKey("ModuleId")]
        public virtual Module? Module { get; set; }

    }
}
