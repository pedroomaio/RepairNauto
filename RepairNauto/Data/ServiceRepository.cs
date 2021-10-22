using AutoRepair.Data.Entities;
using AutoRepair.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AutoRepair.Data
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        private readonly DataContext _context;

        public ServiceRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable GetAllWithUsers()
        {
            return _context.Services.Include(p => p.User);
        }

        public Service ToService(Service models, bool isNew, string userid)
        {
            return new Service
            {
                Id = isNew ? 0 : models.Id,
                ServiceName = models.ServiceName,
                Price=models.Price,
                UserId= userid
            };
        }

        public ServicesViewModel ToServiceViewModel(Service service, string userid)
        {
            return new ServicesViewModel
            {
                Id = service.Id,
                ServiceName = service.ServiceName,
                Price=service.Price,
                UserId= userid
            };
        }
    }
}
