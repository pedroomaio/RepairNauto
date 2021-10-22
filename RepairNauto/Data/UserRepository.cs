using AutoRepair.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public List<User> GetAllUser()
        {
            var list = _context.Users.Select(p => new SelectListItem
            {
                Text = p.UserName,
                Value = p.Id.ToString()
            }).ToList();


            var model = new List<User>();

            foreach (var item in _context.Users)
            {
                model.Add(new User
                {
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    UserName = item.UserName,
                    EmailConfirmed = item.EmailConfirmed,
                    IsMechanic = item.IsMechanic

                });
            }

            return model;

        }
        public string GetUserId(string username)
        {

            var linqUserId = from U in _context.Users
                             where U.UserName == username
                             select U.Id;

            return linqUserId.ToString();

        }

        public IQueryable<User> GetAll()
        {
            return _context.Users.AsNoTracking();
        }
        public async Task<User> GetByIdAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Email == email);
        }


        public async Task CreateAsync(User entity)
        {
            await _context.Set<User>().AddAsync(entity);
            await SaveAllAsync();
        }


        public async Task UpdateAsync(User entity)
        {
            _context.Set<User>().Update(entity);
            await SaveAllAsync();
        }


        public async Task DeleteAsync(User entity)
        {
            _context.Set<User>().Remove(entity);
            await SaveAllAsync();
        }


        public async Task<bool> ExistAsync(string id)
        {
            return await _context.Set<User>().AnyAsync(e => e.Id == id);
        }


        private async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
