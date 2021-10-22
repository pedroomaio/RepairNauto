using AutoRepair.Data.Entities;
using AutoRepair.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Helpers
{
    public interface IConverterUserHelper
    {
        User ToUser(UsersViewModel models);

        UsersViewModel ToUserViewModel(User users);
    }
}
