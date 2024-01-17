using BE_2911.Model.Account;
using DataAccess.Demo.DataAccessObject;
using DataAccess.Demo.DataObject;
using DataAccess.Demo.Dbcontext;
using DataAccess.Demo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Demo.DataAcessObjecImpl
{
    public class AccountRepository : IAccountRepository
    {
        private MyShopDbContext _dbcontext;
        public AccountRepository(MyShopDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<int> AccountUpdateRefeshToken(AccountUpdateRefeshTokenRequestData requestData)
        {
            try
            {
                var user = _dbcontext.users.Where(s => s.UserID == requestData.UserID).FirstOrDefault();
                if (user != null)
                {
                    user.RefeshToken = requestData.RefeshToken;
                    user.RefeshTokenExpired = requestData.RefeshTokenExpired;
                    _dbcontext.users.Update(user);
                   _dbcontext.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return 0;
        }

        public async Task<List<User>> GetUsers()
        {
            var list = new List<User>();
            try
            {
                list = _dbcontext.users.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }

            return list;
        }

        public async Task<User> User_Login(AccountLoginRequestData requestData)
        {
            var user = new User();
            try
            {
                var user_db = _dbcontext.users.Where(s => s.UserName == requestData.UserName && s.Password == requestData.Password).FirstOrDefault();
                if (user_db == null)
                {
                    return user;
                }

                user.UserID = user_db.UserID;
                user.UserName = user_db.UserName;
                user.FullName = user_db.FullName;

                return user;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
