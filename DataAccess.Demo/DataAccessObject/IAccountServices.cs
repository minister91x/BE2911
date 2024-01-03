using DataAccess.Demo.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Demo.DataAccessObject
{
    public interface IAccountServices
    {
        List<User> GetUsers();
    }
}
