using AutoRepair.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Models
{
    public class InspecionViewModel : Inspecion
    {
        

        //[Range(0.0001, double.MaxValue, ErrorMessage = "The quantity must be a positive number.")]
        //public double Quantity { get; set; }

       

       
        [Display(Name = "Brand")]
        public string Brand { get; set; }

        [Display(Name = "Model")]
        public string Model { get; set; }
        
        [Display(Name = "Register Car")]
        public string RegisterCar { get; set; }


    }
}
