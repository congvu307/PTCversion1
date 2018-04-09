using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Threading.Tasks;
using System.Text;
using PTC_versionORANGE.Models;
using System.Threading;
using System.Globalization;

namespace PTC_versionORANGE.Controllers
{
    public class HomeController : Controller
    {
        private PTCEntities db = new PTCEntities();
        public ActionResult Change(String LanguageAbbrevation)
        {
            if (LanguageAbbrevation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbrevation);
            }

            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = LanguageAbbrevation;
            Response.Cookies.Add(cookie);

            return View("Index");
        }
        public ActionResult Recruit()
        {
            return View();
        }
        public ActionResult Statistic()
        {
            return View();
        }
        public ActionResult Life_Skill()
        {
            return View();
        }
        public ActionResult Public_Aministration()
        {
            return View();
        }
        public ActionResult Index()
        {
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