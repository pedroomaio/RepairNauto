using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Entities
{
    public class Car : IEntity
    {
        public int Id { get; set; }

        public int BrandId { get; set; }
        public string Brand { get; set; }
        public bool IsUsed { get; set; }
        public int ModelId { get; set; }
        public string Model { get; set; }
        [Display(Name = "Register Car")]
        public string RegisterCar { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Year { get; set; }

        public string Colour { get; set; }
        public User User { get; set; }
    }
}
