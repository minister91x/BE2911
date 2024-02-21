using Dapper;
using DataAccess.Demo.Dapper;
using DataAccess.Demo.DataAccessObject;
using DataAccess.Demo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Demo.DataAcessObjecImpl
{
    public class ProductServicesDapper : BaseApplicationService, IProductServicesDapper
    {
        public ProductServicesDapper(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<List<Product>> Products_GetList()
        {
            var parameters = new DynamicParameters();
            parameters.Add("@_ProductId", 1002);
            return await DbConnection.QueryAsync<Product>("SP_Product_GetList", parameters);
        }
    }
}
