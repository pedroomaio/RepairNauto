using AutoRepair.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data
{
    public interface IUserRepository
    {
        List<User> GetAllUser();
        Task<User> GetByIdAsync(string email);

        IQueryable<User> GetAll();
        Task CreateAsync(User entity);

        string GetUserId(string username);
        Task UpdateAsync(User entity);


        Task DeleteAsync(User entity);


        Task<bool> ExistAsync(string id);
    }
}
