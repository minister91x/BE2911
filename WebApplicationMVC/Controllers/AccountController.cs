using BeAspNet.DataaAccress.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationMVC.Models;

namespace WebApplicationMVC.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListUserPartialView()
        {
            var model = new List<User>();
            try
            {
                model = new BeAspNet.DataaAccress.DataAcessObjecImpl.UserDAOImpl().GetUsers();
            }
            catch (Exception ex)
            {

                throw;
            }
            return PartialView(model);
        }

        public ActionResult Edit(int? Id)
        {
            var model = new User();
            try
            {
                if (Id != null)
                {
                    // gọi vào db để lấy dữ liệu
                    model = new BeAspNet.DataaAccress.DataAcessObjecImpl.UserDAOImpl().GetById(Convert.ToInt32(Id));
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return View(model);
        }

      
        public JsonResult AccountUpdate(User user)
        {
            var returnData = new ResponseData();
            try
            {
                var rs = new BeAspNet.DataaAccress.DataAcessObjecImpl.UserDAOImpl().UserUpdate(user);
                if (rs <= 0)
                {
                    returnData.code = -1;
                    returnData.mes = "Cập nhật thất bại!";
                    return Json(returnData, JsonRequestBehavior.AllowGet);
                }

                returnData.code = 1;
                returnData.mes = "Cập nhật thành công!";
                return Json(returnData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                returnData.code = -969;
                returnData.mes = ex.Message;
                return Json(returnData, JsonRequestBehavior.AllowGet);
            }


        }
    }
}