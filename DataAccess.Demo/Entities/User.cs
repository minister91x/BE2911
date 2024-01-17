using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Demo.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }

        public string? RefeshToken { get; set; }

        public DateTime? RefeshTokenExpired { get; set; }
    }
}
