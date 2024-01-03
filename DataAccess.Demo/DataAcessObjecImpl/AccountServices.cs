using DataAccess.Demo.DataAccessObject;
using DataAccess.Demo.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Demo.DataAcessObjecImpl
{
    public class AccountServices : IAccountServices
    {
        public List<User> GetUsers()
        {
            var list = new List<User>();
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    list.Add(new User { ID = 1, UserName = "UserName_" + i, FUllName = "BE_2911", UserAddress = "TEAMS" });
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return list;




        }
    }
}
