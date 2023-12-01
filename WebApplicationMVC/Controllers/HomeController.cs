using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationMVC.Models;

namespace WebApplicationMVC.Controllers
{
    public class HomeController : Controller
    {
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Index1(string id)
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


        public ActionResult About(string id)
        {
            ViewBag.Message = "Your application description page.";
            if (id == "abc")
            {


                return RedirectToAction("Contact", "Home");
            }
            else
            {
                return View();

            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}