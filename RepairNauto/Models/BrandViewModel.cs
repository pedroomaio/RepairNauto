using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Models
{
    public class BrandViewModel
    {
        public int ModelId { get; set; }


        public int BrandId { get; set; }


        [Required]
        [Display(Name = "Brand")]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }
    }
}
