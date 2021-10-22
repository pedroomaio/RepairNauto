using AutoRepair.Data.Entities;
using AutoRepair.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Helpers
{
    public class ConverterUserHelper : IConverterUserHelper
    {
        public User ToUser(UsersViewModel models)
        {
            return new User
            {
                Id = models.Id,
                FirstName = models.FirstName,
                LastName = models.LastName,
                Email = models.Email,
                EmailConfirmed = models.EmailConfirmed
            };
        }

        public UsersViewModel ToUserViewModel(User users)
        {
            return new UsersViewModel
            {
                Id = users.Id,
                FirstName = users.FirstName,
                LastName = users.LastName,
                Email = users.Email,
                EmailConfirmed = users.EmailConfirmed
            };
        }
    }
}
