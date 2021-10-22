using AutoRepair.Data.Entities;
using AutoRepair.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data
{
    public interface ICarRepository : IGenericRepository<Car>
    {
        IQueryable GetAllWithUsers();
        Car GetAllIsNotUserWithUsers();
        Car GetAllId(int id);
        IEnumerable<SelectListItem> GetComboCars();
    }
}
