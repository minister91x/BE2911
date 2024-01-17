﻿using BE_2911.Model.Account;
using DataAccess.Demo.DataObject;
using DataAccess.Demo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Demo.DataAccessObject
{
    public interface IAccountRepository
    {
        Task<List<User>> GetUsers();

        Task<User> User_Login(AccountLoginRequestData requestData);

        Task<int> AccountUpdateRefeshToken(AccountUpdateRefeshTokenRequestData requestData);
    }
}