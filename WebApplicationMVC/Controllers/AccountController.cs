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
            var text = "Thêm mới";
            try
            {

                if (Id != null)
                {
                    text = "Cập nhật";
                    // gọi vào db để lấy dữ liệu
                    model = new BeAspNet.DataaAccress.DataAcessObjecImpl.UserDAOImpl().GetById(Convert.ToInt32(Id));
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            ViewBag.Tittle = text;

            return View(model);
        }

        // [ValidateAntiForgeryToken]
        public JsonResult AccountUpdate(User user)
        {
            var returnData = new ResponseData();
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    returnData.code = -1;
                //    returnData.mes = "Cập nhật thất bại!";
                //    return Json(returnData, JsonRequestBehavior.AllowGet);
                //}

                if (user == null)
                {
                    returnData.code = -1;
                    returnData.mes = "Cập nhật thất bại!";
                    return Json(returnData, JsonRequestBehavior.AllowGet);
                }


                if (string.IsNullOrEmpty(user.UserName)
                    || string.IsNullOrEmpty(user.FUllName)
                      || string.IsNullOrEmpty(user.UserAddress))
                {
                    returnData.code = -2;
                    returnData.mes = "Dữ liệu đầu vào không hợp lệ!";
                    return Json(returnData, JsonRequestBehavior.AllowGet);
                }


                //check dữ liệu liên quand dến bảo mật
                if (!CommonLibs.Commom.CheckXSSInput(user.UserName))
                {
                    returnData.code = -3;
                    returnData.mes = "UserName không hợp lệ!";
                    return Json(returnData, JsonRequestBehavior.AllowGet);
                }


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


        public JsonResult AccountDelete(int ID)
        {
            var returnData = new ResponseData();
            try
            {
                if (ID <= 0)
                {
                    returnData.code = -1;
                    returnData.mes = "Dữ liệu không hợp lệ!";
                    return Json(returnData, JsonRequestBehavior.AllowGet);
                }

                var rs = new BeAspNet.DataaAccress.DataAcessObjecImpl.UserDAOImpl().UserDelete(ID);

                if (rs <= 0)
                {
                    switch (rs)
                    {
                        case -1:
                            returnData.code = -1;
                            returnData.mes = "id không tồn tại trên hệ thống";
                            return Json(returnData, JsonRequestBehavior.AllowGet);
                        default:
                            returnData.code = -99;
                            returnData.mes = "Hệ thống đang bận";
                            return Json(returnData, JsonRequestBehavior.AllowGet);
                    }

                }

                returnData.code = 1;
                returnData.mes = "Xóa thành công!";
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