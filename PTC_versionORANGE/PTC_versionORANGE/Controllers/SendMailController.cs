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

namespace PTC_versionORANGE.Controllers
{
    public class SendMailController : Controller
    {
        public JsonResult SendMyMail(string FullName, string PhoneNumber, string EmailAddress, string Message)
        {
            MailMessage toUser = new MailMessage();
            toUser.From = new MailAddress("duongcongvu.com@gmail.com", "Đây là Tittle", Encoding.UTF8);
            toUser.To.Add(EmailAddress);
            toUser.SubjectEncoding = Encoding.UTF8;
            toUser.Subject = "cảm ơn bạn đã quan tâm đến PTC";
            toUser.BodyEncoding = Encoding.UTF8;
            toUser.IsBodyHtml = true;
            toUser.Body = "<p> mail.Body = Đây là nội dung mail<p> </br> <h1>tiêu đề" + FullName + "</h1>";


            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Host = "smtp.gmail.com";
            client.UseDefaultCredentials = false;
            NetworkCredential netwo = new NetworkCredential();
            netwo.UserName = "duongcongvu.com@gmail.com";
            netwo.Password = "attackontitan";
            client.Credentials = netwo;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Send(toUser);
            return Json(FullName);
            
        }
        // GET: SendMail
        public ActionResult Index()
        {
            return View();
        }
    }
}