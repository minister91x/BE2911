using BE_ASPNET_2911.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_ASPNET_2911.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        [HttpPost("GetData")]
        public async Task<ActionResult> GetData(string name)
        {
            var list = new List<DemoResponseData>();
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    list.Add(new DemoResponseData { Name = "Khoa hoc BACKEND " + i + " " + name });
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return Ok(list);
        }
    }
}
