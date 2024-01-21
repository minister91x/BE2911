using BE_2911.Model.Product;
using BE_ASPNET_2911.Filter;
using DataAccess.Demo.DataAccessObject;
using DataAccess.Demo.Entities;
using DataAccess.Demo.UnitOfWork;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BE_ASPNET_2911.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IMyShopUnitOfWork _myShopUnitOfWork;
        private IConfiguration _configuration;
        public ProductController(IMyShopUnitOfWork myShopUnitOfWork, IConfiguration configuration)
        {
            _myShopUnitOfWork = myShopUnitOfWork;
            _configuration = configuration;
        }

        [HttpPost("ProductGetList")]
        [Authorize()]
        public async Task<ActionResult> Products_GetList()
        {
            try
            {
                var list = await _myShopUnitOfWork._productGenericRepository.GetAll();

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
             
                await _myShopUnitOfWork._productGenericRepository.Add(product);
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
                var product = new Product { ProductId = requestData.ProductId };
                _myShopUnitOfWork._productGenericRepository.Remove(product);

                return 1;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        

    }
}
