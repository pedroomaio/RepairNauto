using AutoRepair.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Models
{
    public class CarsViewModel : Car
    {
        public IEnumerable<SelectListItem> Cars { get; set; }


        [Display(Name = "Brand")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a brand.")]
        public int BrandId { get; set; }
        public string BrandIdUser { get; set; }
        public double Quantity { get; set; }
        public int CarId { get; set; }
        public IEnumerable<SelectListItem> Brands { get; set; }


        [Display(Name = "Model")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a model.")]
        public int ModelId { get; set; }
        public IOrderedQueryable<Car> Car { get; set; }

        public IEnumerable<SelectListItem> Models { get; set; }
    }
}
