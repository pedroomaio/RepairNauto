using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Entities
{
    public class Service : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Service")]
        public string ServiceName { get; set; }
        public double Price { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
