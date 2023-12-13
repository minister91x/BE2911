using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationMVC.Models
{
    public class RequestData
    {
        public string Name { get; set; }
    }

    public class UserLoginRequestData
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}