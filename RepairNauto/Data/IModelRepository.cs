using AutoRepair.Data.Entities;
using AutoRepair.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data
{
    public interface IModelRepository : IGenericRepository<Model>
    {
        Task AddBrandAsync(BrandViewModel model);
        Task<int> DeleteBrandAsync(Brand brand);
        IQueryable GetModelsWithBrands();

        Task<Model> GetModelsWithBrandsAsync(int id);

        Task<Brand> GetBrandAsync(int id);
        Task<Brand> GetBrandWithUserAsync(string id);
        Task<Brand> GetBrandAsync();

        Task<int> UpdateBrandAsync(Brand brand);
        IEnumerable<SelectListItem> GetComboModels();

        IEnumerable<SelectListItem> GetComboBrands(int modelId);

        Task<Model> GetModelAsync(Brand brand);
    }
}
