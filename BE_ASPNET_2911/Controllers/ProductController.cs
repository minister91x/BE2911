using BE_2911.Model.Product;
using BE_ASPNET_2911.Filter;
using BE_ASPNET_2911.Models;
using DataAccess.Demo.DataAccessObject;
using DataAccess.Demo.Entities;
using DataAccess.Demo.UnitOfWork;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace BE_ASPNET_2911.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IMyShopUnitOfWork _myShopUnitOfWork;
        private IConfiguration _configuration;
        private IProductServicesDapper _productServicesDapper;

        private readonly IDistributedCache _cache;

        public ProductController(IMyShopUnitOfWork myShopUnitOfWork, IConfiguration configuration, 
            IDistributedCache cache, IProductServicesDapper productServicesDapper)
        {
            _myShopUnitOfWork = myShopUnitOfWork;
            _configuration = configuration;
            _cache = cache;
            _productServicesDapper = productServicesDapper;
        }

        [HttpPost("ProductGetList")]
        // [Authorize("MYSHOP_PRODUCT_GETLIST", "ISVIEW")]
        public async Task<ActionResult> Products_GetList()
        {
            var list = new List<Product>();
            try
            {
                //Bước 1: Kiểm tra dữ liệu trong redis có chưa 
                string cacheKey = "PRODUCT_GET_LIST_CACHING";
                byte[] cachedData = await _cache.GetAsync(cacheKey);


                if (cachedData != null)
                {
                    //Nếu đã có dữ liệu trong Redis

                    var cachedDataFrom_Redis = Encoding.UTF8.GetString(cachedData);
                    //lấy dữ iệu từ redis ra vả Deserialize ra object và trả về client
                    list = JsonSerializer.Deserialize<List<Product>>(cachedDataFrom_Redis);

                    return Ok(list);
                }

                // Chưa có dữ liệu trong Redis
                // Vào database để lấy
                // list = await _myShopUnitOfWork._productGenericRepository.GetAll();
                list = await _productServicesDapper.Products_GetList();
                 //Set lại cache vào Redis 
                 DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                   .SetAbsoluteExpiration(DateTime.Now.AddMinutes(1))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                string cachedDataString = JsonSerializer.Serialize(list);

                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);

                await _cache.SetAsync(cacheKey, dataToCache, options);

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


        [HttpPost("ProductImportByExcel")]
        public async Task<ActionResult> ProductImportByExcel([FromForm] UploadFileInputDto formFile)
        {
            try
            {
                if (formFile == null || formFile.File.Length <= 0)
                {
                    throw new Exception("file dữ liệu không được trống");
                }

                if (!Path.GetExtension(formFile.File.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Hệ thống chỉ hỗ trợ file .xlsx");
                }

                using (var stream = new MemoryStream())
                { 
                    await formFile.File.CopyToAsync(stream);

                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelPackage.LicenseContext = LicenseContext.Commercial;
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var name = worksheet.Cells[row, 1]?.Value?.ToString()?.Trim();

                            await _myShopUnitOfWork._productGenericRepository.Add(new Product
                            {
                                ProductName = name
                            });
                        }


                    }

                    var result = _myShopUnitOfWork.SaveChange();
                }

                return Ok();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
