using BE_2911.Model.Product;
using DataAccess.Demo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Demo.DataAccessObject
{
    public interface IProductRepository
    {
        Task<List<Product>> Products_GetList();
        Task<int> Product_InsertUpdate(Product product);
        Task<int> Product_Delete(ProductDelete_RequestData requestData);
    }
}
