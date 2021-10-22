using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Entities
{
    public class InspecionDetails : IEntity
    {
        public int Id { get; set; }


        [Required]
        public Car Car { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ExitDate { get; set; }
        public bool InspesioStatus { get; set; }

        [Display(Name = "Total Price")]
        public double TotalPrice { get; set; }


        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity { get; set; }
    }
}
