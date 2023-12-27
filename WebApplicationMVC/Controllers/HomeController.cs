using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationMVC.Filter;
using WebApplicationMVC.Models;
using WebApplicationMVC.Models.Account;

namespace WebApplicationMVC.Controllers
{
    public class HomeController : Controller
    {
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Index(string inputName)
        {
            var model = new List<DataResponseModels>();
            var model2 = new List<DataResponseModels>();

            var str = "DAY LA GIA TRI CUA INPUT:";

            try
            {
                str = str + inputName;
                for (int i = 0; i < 10; i++)
                {
                    model.Add(new DataResponseModels
                    {
                        Messenger = "so :" + i
                    });
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            var object_response_from_Server = new List<GetResponseData>();
            //Khai báo URL CỦA SERVER
            var config_root_url = System.Configuration.ConfigurationManager.AppSettings["API_URL_ROOT"].ToString() ?? "";

            var data = new GetDataRequest
            {
                name = inputName
            };

            // ĐƯA DỮ LIỆU GỬI LÊN SERVER SANG DẠNG JSON
            var jsonData = JsonConvert.SerializeObject(data);

            // GỌI HÀM ĐỂ GỬI DỮ LIỆU LÊN SERVER
            var data_from_server = CommonLibs.Commom.SendPost(config_root_url, "Demo/GetData", jsonData);

            // NHẬN KẾT QUẢ TỪ SERVER TRẢ VỀ 
            if (data_from_server != null)
            {
                // ĐƯA DỮ LIỆU SERVER TRẢ VỀ Ở DẠNG JSON SANG LIST OBJECT
                object_response_from_Server = JsonConvert.DeserializeObject<List<GetResponseData>>(data_from_server);
            }


            // return View("~/Views/Home/MyView.cshtml", model);

            ViewBag.DataServer = object_response_from_Server;
            return View();
        }

        public ActionResult Pace()
        {
            return View();
        }

        [OutputCache(Duration = 100)]
        public ActionResult About(string id)
        {
            ViewBag.Message = "Your application description page.";
            if (id == "abc")
            {


                return RedirectToAction("Contact", "Home");
            }
            else
            {
                return View(); // Views -> Home -> About.cshtml

            }
        }

        [MyCustomFilter]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PartialViewDemo(RequestData data)
        {
            ViewBag.Name = data.Name;
            return PartialView();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public JsonResult Login(UserLoginRequestData requestData)
        {
            var returnData = new ResponseData();
            try
            {
                if (requestData == null)
                {
                    returnData.code = -1;
                    returnData.mes = "dữ liệu không hợp lệ";
                    return Json(returnData, JsonRequestBehavior.AllowGet);
                }

                if (!string.IsNullOrEmpty(requestData.UserName)
                    && !string.IsNullOrEmpty(requestData.Password))
                {
                    returnData.code = -2;
                    returnData.mes = "dữ liệu không hợp lệ";
                    return Json(returnData, JsonRequestBehavior.AllowGet);
                }

                if (requestData.UserName == "quannt" && requestData.Password == "123")
                {
                    returnData.code = 1;
                    returnData.mes = "Đăng nhập thành công";
                    return Json(returnData, JsonRequestBehavior.AllowGet);
                }

                returnData.code = -100;
                returnData.mes = "Đăng nhập fail";
                return Json(returnData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exx)
            {

                throw;
            }
        }
    }
}