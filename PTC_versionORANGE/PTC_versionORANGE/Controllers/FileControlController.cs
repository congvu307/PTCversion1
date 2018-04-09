using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
namespace PTC_versionORANGE.Areas.Admin.Controllers
{
    public class FileControlController : Controller
    {
        // GET: Admin/FileControl
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/App_Data/uploads"), _FileName);
                    string _TemplatePath = Path.Combine(Server.MapPath("~/App_Data/uploads"), "Test.xlsx");
                    string _FileExtension = Path.GetExtension(_FileName);
                    if (_FileExtension ==".xlsx")
                    {
                        //read file exel
                        FileStream fs = new FileStream(_TemplatePath, FileMode.Open);
                        XSSFWorkbook wb = new XSSFWorkbook(fs);
                        ISheet sheet = wb.GetSheetAt(0);
                        Console.OutputEncoding = Encoding.UTF8;
                        string temp = sheet.GetRow(0).GetCell(0).StringCellValue;
                        Console.WriteLine(temp);
                        file.SaveAs(_path);
                    }
                    else
                    {
                        ViewBag.Message = "Sai định dạng file (.xls .xlsx)";
                    }
                }
                ViewBag.Message = "File Uploaded Successfully!!";
               
                return View();
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }
    }
}