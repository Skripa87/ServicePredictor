using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicePredictor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var ftpManager = new FtpDataManager("ftp://92.50.187.210:21//bus1", "ftpuser", "Ln8#{T7nRsmd");
            var busRouteManager = new BusRouteManager(ftpManager.GetData());
            busRouteManager.CreateBusRoute();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}