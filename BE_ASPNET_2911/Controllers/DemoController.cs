using BE_ASPNET_2911.Models;
using DataAccess.Demo.DataAccessObject;
using DataAccess.Demo.DataObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_ASPNET_2911.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private IAccountServices _accountServices;
        private IConfiguration _configuration;
        public DemoController(IAccountServices accountServices, IConfiguration configuration)
        {
            _accountServices = accountServices;
            _configuration = configuration;
        }

        [HttpPost("GetData")]
        public async Task<ActionResult> GetData([FromBody] DemoResponseData requestData)
        {
            var list = new List<User>();
            try
            {
                var url = _configuration["URL:ROOT"] ?? "";

                list = _accountServices.GetUsers();
            }
            catch (Exception ex)
            {

                throw;
            }

            return Ok(list);
        }
    }
}
