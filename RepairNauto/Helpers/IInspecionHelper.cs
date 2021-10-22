using AutoRepair.Data;
using AutoRepair.Data.Entities;
using AutoRepair.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Helpers
{
    public interface IInspecionHelper 
    {
        Inspecion ToInspecion(AddItemViewModel model);
        InspecionViewModel ToInspecionsViewModel(Inspecion inspecion);
    }
}
