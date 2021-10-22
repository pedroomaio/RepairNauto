using AutoRepair.Data.Entities;
using AutoRepair.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data
{
    public interface IInspecionRepository : IGenericRepository<Inspecion>
    {
        Task<IQueryable<Inspecion>> GetOrderAsync(string userName);

        IQueryable<Inspecion> GetOrderInProcessing();
        IQueryable<Inspecion> GetOrderCompleted();
        Task<IQueryable<InspecionDetailTemp>> GetDetailTempsAsync(string userName);

        IQueryable GetAllWithCars(int id);
        Task AddItemToOrderAsync(AddItemViewModel model, string userName);


        Task ModifyOrderDetailTempQuantityAsync(int id, double quantity);


        Task DeleteDetailTempAsync(int id);


        Task<bool> ConfirmOrderAsync(string userName);


        Task DeliverOrder(MarkViewModel model);


        Task<Inspecion> GetOrderAsync(int id);
    }
}
