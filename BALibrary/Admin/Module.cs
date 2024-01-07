using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALibrary.Admin
{
    public class Module
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Name")]
        [Display(Name = "Module Name")]
        public string? Name { get; set; }
        [ScaffoldColumn(false)]
        public int Status { get; set; }
    }
}
