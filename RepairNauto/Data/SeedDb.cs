using AutoRepair.Data.Entities;
using AutoRepair.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Customer");
            await _userHelper.CheckRoleAsync("Employee");
            await _userHelper.CheckRoleAsync("Mechanic");



            var user = await _userHelper.GetUserByEmailAsync("repairnauto@gmail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Auto",
                    LastName = "Repair",
                    Email = "repairnauto@gmail.com",
                    UserName = "repairnauto@gmail.com",
                    PhoneNumber = "123456789",
                    Address = "Rua Salmão Real 123 nº6",
                    AgreeTerm = true,
                    EmailConfirmed=true
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

                if (!_context.Models.Any())
                {
                    var brands = new List<Brand>();
                    brands.Add(new Brand { Name = "500", UserId = user.Id });
                    brands.Add(new Brand { Name = "Panda", UserId = user.Id });
                    brands.Add(new Brand { Name = "Punto", UserId = user.Id });

                    _context.Models.Add(new Model
                    {
                        brands = brands,
                        Name = "Fiat"
                    });

                    await _context.SaveChangesAsync();
                }
                if (!_context.Services.Any())
                {
                    _context.Services.Add(new Service { ServiceName = "Revision", Price=123, UserId=user.Id});
                    _context.Services.Add(new Service { ServiceName = "Oil Change", Price = 12, UserId = user.Id });
                    _context.Services.Add(new Service { ServiceName = "Battery Checkup", Price = 20, UserId = user.Id });
                    _context.Services.Add(new Service { ServiceName = "Braking", Price = 22, UserId = user.Id });
                    _context.Services.Add(new Service { ServiceName = "Visibility", Price = 40, UserId = user.Id });
                    _context.Services.Add(new Service { ServiceName = "Air Conditioning", Price = 50, UserId = user.Id });
                    _context.Services.Add(new Service { ServiceName = "Suspension", Price = 75, UserId = user.Id });


                    await _context.SaveChangesAsync();
                }
            }


            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

        }

    }
}
