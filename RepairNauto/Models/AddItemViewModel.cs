using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Models
{
    public class AddItemViewModel
    {
        [Display(Name = "Car")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a product.")]
        public int CarId { get; set; }
        //public DateTime? PStartDate { get; set; }

        public double Price { get; set; }

        public IEnumerable<SelectListItem> Cars { get; set; }
    }
}
