﻿using BeAspNet.DataaAccress.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeAspNet.DataaAccress.DataAccessObject
{
    public interface IUserDAO
    {
        List<User> GetUsers(int PageIndex,int PageSize, out int TotalCount);
        User GetById(int id);

        int UserUpdate(User user);

        int UserDelete(int UserId);
    }
}
