using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationMVC.Filter;
using WebApplicationMVC.Models;

namespace WebApplicationMVC.Controllers
{
    public class HomeController : Controller
    {
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Index1(int? id)
        {
            var model = new List<DataResponseModels>();
            var model2 = new List<DataResponseModels>();

            var str = "DAY LA GIA TRI CUA INPUT:";

            try
            {
                str = str + id;
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

            ViewBag.Data = model2;
            return View("~/Views/Home/MyView.cshtml", model);
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