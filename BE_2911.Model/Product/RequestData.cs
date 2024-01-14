using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_2911.Model.Product
{
    public class RequestData
    {
        public string token { get; set; }
    }

    public class ProductDelete_RequestData : RequestData
    {
        public int ProductId { get; set; }
    }
}
