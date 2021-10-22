using AutoRepair.Data.Entities;
using AutoRepair.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Car ToCar(CarsViewModel model, bool isNew)
        {
            return new Car
            {
                Id = isNew ? 0 : model.Id,
                Brand = model.Brand,
                RegisterCar = model.RegisterCar,
                Year = model.Year,
                Model = model.Model,
                Colour = model.Colour,
                User = model.User,
                BrandId = model.BrandId,
                ModelId = model.ModelId,
                IsUsed = false
            };
        }      public Car ToCarCheck(Car model, bool isNew)
        {
            return new Car
            {
                Id = isNew ? 0 : model.Id,
                Brand = model.Brand,
                RegisterCar = model.RegisterCar,
                Year = model.Year,
                Model = model.Model,
                Colour = model.Colour,
                User = model.User,
                BrandId = model.BrandId,
                ModelId = model.ModelId,
                IsUsed = false
            };
        }     
        
        public Car ToCarUpdate(Car model)
        {
            return new Car
            {
                Id = model.Id,
                Brand = model.Brand,
                RegisterCar = model.RegisterCar,
                Year = model.Year,
                Model = model.Model,
                Colour = model.Colour,
                User = model.User,
                BrandId = model.BrandId,
                ModelId = model.ModelId,
                IsUsed = false
            };
        }


        public CarsViewModel ToCarsViewModel(Car Cars)
        {
            return new CarsViewModel
            {
                Id = Cars.Id,
                Brand = Cars.Brand,
                RegisterCar = Cars.RegisterCar,
                Year = Cars.Year,
                Model = Cars.Model,
                Colour = Cars.Colour,
                User = Cars.User,
                ModelId = Cars.ModelId,
                BrandId = Cars.BrandId,
                IsUsed = true
            };
        }


    }
}
