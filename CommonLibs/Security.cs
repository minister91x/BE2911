using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibs
{
    public static class Security
    {
        public static string MD5(this string s)
        {
            var provider = System.Security.Cryptography.MD5.Create();
            StringBuilder builder = new StringBuilder();

            foreach (byte b in provider.ComputeHash(Encoding.UTF8.GetBytes(s)))
                builder.Append(b.ToString("x2").ToLower());

            return builder.ToString();
        }
    }
}
