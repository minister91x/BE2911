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
    }
}
