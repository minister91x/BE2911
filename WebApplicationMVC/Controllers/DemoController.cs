using BeAspNet.DataaAccress.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationMVC.Models;

namespace WebApplicationMVC.Controllers
{
    public class DemoController : Controller
    {
        // GET: Demo
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index2()
        {
           
            var list = new BeAspNet.DataaAccress.DataAcessObjecImpl.UserDAOImpl().GetUsers();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index3()
        {
            return PartialView();
        }

    }
}