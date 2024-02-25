using MediaNetCore.Models;
using MediaNetCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MediaNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerMediaController : ControllerBase
    {

        private readonly ILoggerManager _loggerManager;

        private IConfiguration _configuration;
        public ManagerMediaController(IConfiguration configuration, ILoggerManager loggerManager)
        {
            _configuration = configuration;
            _loggerManager = loggerManager;
        }




        // lưu ảnh
        [HttpPost("SaveImage_Data")]
        public async Task<ActionResult> SaveImage_Data(SaveImage_DataRequestData requestData)
        {
            var returnData = new ReturnData();
            var ReqeuestId = DateTime.Now.Ticks;
            try
            {
                // kiểm tra đầu vào
                _loggerManager.LogInfo(ReqeuestId + "ManagerMediaController | data: |" + JsonConvert.SerializeObject(requestData));
                if (requestData == null ||
                    string.IsNullOrEmpty(requestData.Base64Image)
                    || string.IsNullOrEmpty(requestData.Sign))
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMessage = "Dữ liệu đầu vào không hợp lệ";
                    return Ok(returnData);

                }


                // kiểm tra xem chữ ký có hợp lệ không ?
                var SecretKey = _configuration["SecretKey:IMAGE_DOWN_UPLOAD"] ?? "";

                var plantext = requestData.Base64Image + SecretKey;

                var Sign = CommonLibs.Security.MD5(plantext);

                if (Sign != requestData.Sign)
                {

                    returnData.ReturnCode = -3;
                    returnData.ReturnMessage = "Chữ ký không hợp lệ không hợp lệ";

                    _loggerManager.LogInfo(ReqeuestId + " ManagerMediaController | data:" + JsonConvert.SerializeObject(returnData));
                    return Ok(returnData);
                }

                // Vẽ lại ảnh và lưu vào thư mục

                var path = "Upload"; //Path

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                string imageName = Guid.NewGuid().ToString() + ".png";
                //set the image path
                var imgPath = Path.Combine(path, imageName);

                if (requestData.Base64Image.Contains("data:image"))
                {
                    requestData.Base64Image = requestData.Base64Image.Substring(requestData.Base64Image.LastIndexOf(',') + 1);
                }

                byte[] imageBytes = Convert.FromBase64String(requestData.Base64Image);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                image.Save(imgPath, System.Drawing.Imaging.ImageFormat.Png);

                returnData.ReturnCode = 1;
                returnData.ReturnMessage = imageName;
                return Ok(returnData);
            }
            catch (Exception ex)
            {

                returnData.ReturnCode = 1;
                returnData.ReturnMessage = ex.StackTrace;
                _loggerManager.LogInfo(ReqeuestId + " | exception:" + JsonConvert.SerializeObject(returnData));
                return Ok(returnData);
            }


        }


        // lấy ảnh
    }
}
