using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_2911.Model.Account
{
    public class RequestData
    {

    }

    public class AccountLoginRequestData
    {
        public string UserName { get; set; }
        public string Password { get; set; }    
    }

    public class AccountUpdateRefeshTokenRequestData
    {
        public int UserID { get; set; }
        public string RefeshToken { get; set; }

        public DateTime RefeshTokenExpired { get; set; }
    }
}
