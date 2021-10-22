using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Models
{
    public class MechanicViewModel 
    {
        public int SpecialistTypeId { get; set; }
        public int MechanicId { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }
    }
}
