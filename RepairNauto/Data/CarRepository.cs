using AutoRepair.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data
{
    public class CarRepository : GenericRepository<Car>, ICarRepository
    {
        private readonly DataContext _context;

        public CarRepository(DataContext context) : base(context)
        {
            _context = context;
        }


        public IQueryable GetAllWithUsers()
        {
            return _context.Cars.Include(p => p.User);
        }
        public Car GetAllIsNotUserWithUsers()
        {
            var a = new Car();
           var aq = _context.Cars.Include(p => p.User);

            foreach (var item in aq)
            {
                if (item.IsUsed)
                {
                    a = item;
                }
            }
            return a;
        }

        public Car GetAllId(int id)
        {
            var a = new Car();

           return  _context.Cars
                .Include(c => c.User)
                .Where(c => c.Id == id).FirstOrDefaultAsync().Result;
            
               

        }
        public IEnumerable<SelectListItem> GetComboCars()
        {

            var list = _context.Cars.Select(p => new SelectListItem
            {
                Text = p.RegisterCar,
                Value = p.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a car...)",
                Value = "0"
            });

            return list;

        }
    }
}
