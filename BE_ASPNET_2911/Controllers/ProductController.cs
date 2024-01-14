using BE_2911.Model.Product;
using DataAccess.Demo.DataAccessObject;
using DataAccess.Demo.Entities;
using DataAccess.Demo.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_ASPNET_2911.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IMyShopUnitOfWork _myShopUnitOfWork;
        public ProductController(IMyShopUnitOfWork myShopUnitOfWork)
        {
            _myShopUnitOfWork = myShopUnitOfWork;
        }

        [HttpPost("ProductGetList")]
        public async Task<ActionResult> Products_GetList()
        {
            try
            {
                var list = await _myShopUnitOfWork.productRepository.Products_GetList();

                return Ok(list);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpPost("Product_InsertUpdate")]
        public async Task<int> Product_InsertUpdate([FromBody] Product product)
        {
            try
            {
                await _myShopUnitOfWork.productRepository.Product_InsertUpdate(product);
                var result = _myShopUnitOfWork.SaveChange();
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost("Product_Delete")]
        public async Task<int> Product_Delete(ProductDelete_RequestData requestData)
        {
            try
            {
                var result = await _myShopUnitOfWork.productRepository.Product_Delete(requestData);

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
