using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALibrary.Admin
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Role Name")]
        public string? Name { get; set; }
        [Required]
        [Display(Name = "Role description")]
        public string? Description { get; set; }
        [ScaffoldColumn(false)]
        public int status { get; set; }
    }
}
