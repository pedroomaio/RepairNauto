using AutoRepair.Data.Entities;
using AutoRepair.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data
{
    public class ModelRepository : GenericRepository<Model>, IModelRepository
    {
        private readonly DataContext _context;

        public ModelRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddBrandAsync(BrandViewModel model)
        {
            var country = await this.GetModelsWithBrandsAsync(model.ModelId);
            if (country == null)
            {
                return;
            }

            country.brands.Add(new Brand { Name = model.Name });
            _context.Models.Update(country);
            await _context.SaveChangesAsync();
        }


        public async Task<int> DeleteBrandAsync(Brand brand)
        {
            var country = await _context.Models
                .Where(c => c.brands.Any(ci => ci.Id == brand.Id))
                .FirstOrDefaultAsync();
            if (country == null)
            {
                return 0;
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return country.Id;
        }

        public IQueryable GetModelsWithBrands()
        {
            return _context.Models
                .Include(c => c.brands)
                .OrderBy(c => c.Name);
        }

        public async Task<Model> GetModelsWithBrandsAsync(int id)
        {
            return await _context.Models
                .Include(c => c.brands)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }


        public async Task<int> UpdateBrandAsync(Brand brand)
        {
            var country = await _context.Models
                .Where(c => c.brands.Any(ci => ci.Id == brand.Id)).FirstOrDefaultAsync();
            if (country == null)
            {
                return 0;
            }

            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
            return country.Id;
        }


        public async Task<Brand> GetBrandWithUserAsync(string id)
        {
            //return await _context.Brands.FindAsync(id);

            var a = _context.Brands;
            return await _context.Brands
                .Where(c => c.UserId == id)
                .FirstOrDefaultAsync();
        }   
        public async Task<Brand> GetBrandAsync()
        {
            //return await _context.Brands.FindAsync(id);

            var a = _context.Brands;
            return await _context.Brands
                .FirstOrDefaultAsync();
        }

        public async Task<Brand> GetBrandAsync(int id)
        {
            //return await _context.Brands.FindAsync(id);

            var a =  _context.Brands;
            return await _context.Brands
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }


        public async Task<Model> GetModelAsync(Brand brand)
        {
            return await _context.Models
                .Where(c => c.brands.Any(ci => ci.Id == brand.Id))
                .FirstOrDefaultAsync();
        }


        public IEnumerable<SelectListItem> GetComboModels()
        {
            var list = _context.Models.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()

            }).OrderBy(l => l.Text).ToList();


            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Model...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboBrands(int modelId)
        {
            var model = _context.Models.Find(modelId);
            var list = new List<SelectListItem>();
            if (model != null)
            {
                list = _context.Brands.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()

                }).OrderBy(l => l.Text).ToList();


                list.Insert(0, new SelectListItem
                {
                    Text = "(Select a brand...)",
                    Value = "0"
                });

            }

            return list;
        }
    }
}
