using BE_2911.Model.Product;
using DataAccess.Demo.DataAccessObject;
using DataAccess.Demo.Dbcontext;
using DataAccess.Demo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Demo.DataAcessObjecImpl
{
    public class ProductRepository : IProductRepository
    {
        private MyShopDbContext _dbcontext;
        public ProductRepository(MyShopDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<Product>> Products_GetList()
        {
            return _dbcontext.product.ToList();
        }

        public async Task<int> Product_Delete(ProductDelete_RequestData requestData)
        {
            try
            {
                if (requestData == null || requestData.ProductId <= 0)
                {
                    return -11;
                }

                var product_delete = _dbcontext.product.ToList()
                    .Where(s => s.ProductId == requestData.ProductId).FirstOrDefault();

                if (product_delete == null || product_delete.ProductId < 0)
                {
                    return -1;
                }

                _dbcontext.product.Remove(product_delete);
                var result = _dbcontext.SaveChanges();
                return result;

            }
            catch (Exception ex)
            {

                return -2;
            }
        }

        public async Task<int> Product_InsertUpdate(Product product)
        {
            try
            {
                // check dữ lieuek client gửi lên có hợp lệ không
                if (product == null ||
                    string.IsNullOrEmpty(product.ProductName)
                    || string.IsNullOrEmpty(product.ProductImage))
                {
                    return -1;
                }


                // CHeck xem có lỗi gì về bảo mật không 
                if (!CommonLibs.Commom.CheckXSSInput(product.ProductName)
                    || !CommonLibs.Commom.CheckXSSInput(product.ProductImage))
                {
                    return -2;
                }

                if (product.ProductId <= 0)
                {
                    // INSERT

                    _dbcontext.product.Add(product);
                   /// var result = _dbcontext.SaveChanges();
                    return 1;
                }
                else
                {
                    //UPDATE 
                    var product_update = _dbcontext.product.ToList().Where(s => s.ProductId == product.ProductId).FirstOrDefault();

                    if (product_update == null || product_update.ProductId < 0)
                    {
                        return -1;
                    }

                    _dbcontext.product.Update(product_update);
                   // var result = _dbcontext.SaveChanges();
                    return 1;

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
