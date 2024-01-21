using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_2911.Model.Account
{
    internal class ResponseData
    {
    }

    public class ReturnData
    {

        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string token { get; set; }
        public string refeshToken { get; set; }



    }

    public class Response
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
