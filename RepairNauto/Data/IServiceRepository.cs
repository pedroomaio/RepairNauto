using AutoRepair.Data.Entities;
using AutoRepair.Models;
using System.Linq;

namespace AutoRepair.Data
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        public IQueryable GetAllWithUsers();
        Service ToService(Service models, bool isNew, string userid);
        ServicesViewModel ToServiceViewModel(Service service, string userid);
    }
}