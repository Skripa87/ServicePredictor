using ServicePredictor.Models;
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
            var ftpManager = new FtpDataManager("ftp://192.168.10.10//bus1", "ftpuser", "Ln8#{T7nRsmd");
            var x = ftpManager.GetData();
            var xlworker = new XLWorker("D:\\table.xlsx");
            xlworker.CreateXLDocument(x);
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