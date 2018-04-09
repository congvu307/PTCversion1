using PTC_versionORANGE.Areas.Admin.Models;
using PTC_versionORANGE.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PTC_versionORANGE.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private List<string> ListRole;
        private List<string> ListEMPosition;
        private PTCEntities db = new PTCEntities();
        // GET: Admin/Login
        [HttpGet]
        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            //kiểm tra tồn tại khóa key trong cookies hay không
            //if (Request.Cookies.AllKeys.Any(s=>s.Contains("key")))
            //{
            //    return RedirectToAction("Index", "Home");

            //}
            //else
            //{
            return View();
            //}
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel model)
        {
            if (Membership.ValidateUser(model.Username, model.Password) && ModelState.IsValid)
            {
                SqlParameter[] sqlParams_Role = new SqlParameter[]
                {
                     new SqlParameter("@usercode",(object)model.Username ?? DBNull.Value),
                };
                //Save username in Browser COokie
                FormsAuthentication.SetAuthCookie(model.Username, true);
                Response.Cookies["UserName"].Value = model.Username;
                SqlParameter[] sqlParams_ID = new SqlParameter[]
               {
                     new SqlParameter("@User_Code",(object)model.Username ?? DBNull.Value),
               };
                var key = db.Database.SqlQuery<int>("FindIDbyUserCode @User_Code", sqlParams_ID).SingleOrDefault();
                int temp = key;
                //var key2 = db.Database.SqlQuery<int>("Select ");
                Response.Cookies["key"].Value = Convert.ToString(temp);
                //Response.Cookies["key"].Expires = DateTime.Now.AddYears(1);
                ListRole = db.Database.SqlQuery<string>("Check_Role @usercode", sqlParams_Role).ToList();
                var NumOfRole = 0;
                var NumOfEMPosition = 10;
                foreach (string role in ListRole)
                {
                    //Check if you are employee
                    //lưu role từ 1 tới 10
                    Response.Cookies[NumOfRole.ToString()].Value = role;
                    //Response.Cookies[NumOfRole.ToString()].Expires = DateTime.Now.AddHours(12);
                    NumOfRole++;
                    if (role == "EM")
                    {
                        Response.Cookies["currentRole"].Value = "EM";
                        SqlParameter[] sqlParams_EMPosition = new SqlParameter[]
                        {
                             new SqlParameter("@User_Code",(object)model.Username ?? DBNull.Value),
                        };
                        //Check List of Employ
                        //lưu danh sách EM position từ 10 về sau 
                        ListEMPosition = db.Database.SqlQuery<string>("Check_EMPosition @User_Code", sqlParams_EMPosition).ToList();
                        foreach (string EMposition in ListEMPosition)
                        {
                            Response.Cookies[NumOfEMPosition.ToString()].Value = EMposition;
                            //Response.Cookies[NumOfEMPosition.ToString()].Expires = DateTime.Now.AddHours(12);
                            NumOfEMPosition++;
                        }
                    }  
                    else
                    {
                        //xử lý sau
                    }
                }
                Response.Cookies["NumberRole"].Value = NumOfRole.ToString();
                //Response.Cookies["NumberRole"].Expires = DateTime.Now.AddHours(12);
                Response.Cookies["NumberEMPosition"].Value = NumOfEMPosition.ToString();
               // Response.Cookies["NumberEMPosition"].Expires = DateTime.Now.AddHours(12);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}