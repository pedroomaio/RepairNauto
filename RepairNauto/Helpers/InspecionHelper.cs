using AutoRepair.Data;
using AutoRepair.Data.Entities;
using AutoRepair.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Helpers
{
    public class InspecionHelper : IInspecionHelper
    {
        public Inspecion ToInspecion(AddItemViewModel model)
        {
            return new Inspecion
            {
                //Price = model.Price,
                InspecionDateStart = DateTime.UtcNow
            };
        }


        public InspecionViewModel ToInspecionsViewModel(Inspecion inspecion)
        {
            return new InspecionViewModel
            {
                //Price = inspecion.Price,
                //date = inspecion.PreferDate,
                //PreferHours = inspecion.PreferHours
            };
        }
    }
}
