using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Entities
{
    public class Inspecion : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Inspecion Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime InspecionDate { get; set; }
        [Display(Name = "Inspecion Date Start")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime InspecionDateStart { get; set; }

        public string InspecionHours { get; set; }

        public double Price { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        public string Username { get; set; }



        public IEnumerable<InspecionDetails> Items { get; set; }
        public User User { get; set; }
    }
}
