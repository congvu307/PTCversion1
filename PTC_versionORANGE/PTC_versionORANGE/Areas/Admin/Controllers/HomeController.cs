using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PTC_versionORANGE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace PTC_versionORANGE.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public PTCEntities db = new PTCEntities();
        private const int CHECK_IS_NULL = 1;
        private const int CHECK_DATE_TYPE = 2;
        private const int CHECK_NUMBER_TYPE = 3;
        private const int CHECK_ISNULL_AND_NUMBER = 4;
        private const int CHECK_ISNULL_AND_IN_ROLELIST = 5;
        public bool CheckIDInDistrictList(int? id)
        {
            var xxxa = db.Ref_District.Any(x => x.District_ID == id);
            return true;
        }
        public string Check_Format_Excel(ISheet isheet, int type, int row, int column)
        {
            switch (type)
            {
                //kiếm tra khác null
                case CHECK_IS_NULL:
                    if (isheet.GetRow(row).GetCell(column) == null)
                    {
                        return "missing " + isheet.GetRow(0).GetCell(column).StringCellValue + " |";
                    }
                    break;
                //kiểm tra dạng số
                case CHECK_NUMBER_TYPE:
                    if (isheet.GetRow(row).GetCell(column) != null)
                    {
                        if (!isheet.GetRow(row).GetCell(column).ToString().All(char.IsDigit))
                        {
                            return "format " + isheet.GetRow(0).GetCell(column).StringCellValue + " |";
                        }
                    }
                    break;
                //kiểm trả khác null và dạng số
                case CHECK_ISNULL_AND_NUMBER:
                    if (isheet.GetRow(row).GetCell(column) == null)
                    {
                        return "missing " + isheet.GetRow(0).GetCell(column).StringCellValue + " |";
                    }
                    else
                    {
                        if (!isheet.GetRow(row).GetCell(column).ToString().All(char.IsDigit))
                        {
                            return "format " + isheet.GetRow(0).GetCell(column).StringCellValue + " |";
                        }
                    }
                    break;
                case CHECK_ISNULL_AND_IN_ROLELIST:
                    if (isheet.GetRow(row).GetCell(column) == null)
                    {
                        return "missing " + isheet.GetRow(0).GetCell(column).StringCellValue + " |";
                    }
                    else
                    {
                        if (!isheet.GetRow(row).GetCell(column).ToString().All(char.IsDigit))
                        {
                            return "format " + isheet.GetRow(0).GetCell(column).StringCellValue + " |";
                        }
                    }
                    break;
            }
            return "";
        }

        // GET: Admin/Home
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Information()
        {
            var mykey = Int32.Parse(Request.Cookies["key"].Value);
            User_Basic userbasic = db.User_Basic.Find(mykey);
            if (userbasic == null)
            {
                return HttpNotFound();
            }
            return PartialView(userbasic);
        }
        public void AddExel()
        {
            FileStream fs = new FileStream("D:/input.xlsx", FileMode.Open);
            XSSFWorkbook wb = new XSSFWorkbook(fs);
        }

        public ActionResult User_Basic_Create()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult User_Basic_Create([Bind(Include = "User_ID,User_Code,Password,User_First_Name,User_Last_Name,User_Alternative_Name,User_DoB_Day,User_DoB_Month,User_DoB_Year,Gender,Address_Detail,Address_District,Email,Phone,Role,Client_Code,Education_Type_Code,Job_Type_Code,Job_Detail,Status,Note")] User_Basic user_Basic)
        {
            if (ModelState.IsValid)
            {
                db.User_Basic.Add(user_Basic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user_Basic);
        }


        public ActionResult User_Basic_Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Basic user_Basic = db.User_Basic.Find(id);
            if (user_Basic == null)
            {
                return HttpNotFound();
            }
            return PartialView(user_Basic);
        }

        [HttpPost, ActionName("User_Basic_Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult User_Basic_EditConfirm([Bind(Include = "User_ID,User_Code,Password,User_First_Name,User_Last_Name,User_Alternative_Name,User_DoB_Day,User_DoB_Month,User_DoB_Year,Gender,Address_Detail,Address_District,Email,Phone,Role,Client_Code,Education_Type_Code,Job_Type_Code,Job_Detail,Status,Note")] User_Basic user_Basic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user_Basic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user_Basic);
        }

        // chọn role chính
        public ActionResult SelectRole(string role)
        {
            Response.Cookies["currentRole"].Value = role;
            return RedirectToAction("Index", "Home");
        }
        //chọn position chính
        public ActionResult SelectPosition(string position)
        {
            Response.Cookies["currentPosition"].Value = position;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult User_Basic_Index()
        {
            return PartialView(db.User_Basic.ToList());
        }
        [HttpGet]
        public ActionResult User_Basic_MultiUpload()
        {
            return PartialView();
        }
        //download file template
        public FileResult UserBasic_Download(string filename)
        {
            string fileDir = Path.Combine(Server.MapPath("~/App_Data/uploads"), filename);
            return File(fileDir, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        [HttpGet]
        public ActionResult EMList_MultiUpload()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult EMList_MultiUpload(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    ViewBag.ErrorEMListFileName = "";
                    ViewBag.CurrentFunctionExcelUpload = "EMList";
                    string ErrorFileName = "";
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/App_Data/uploads"), _FileName);
                    string _TemplatePath = Path.Combine(Server.MapPath("~/App_Data/uploads"), "EMList_Template.xlsx");
                    string _FileExtension = Path.GetExtension(_FileName);
                    if (_FileExtension == ".xlsx")
                    {
                        // lưu file excel lại với tên Usercode_filename.xlsxS
                        string FileNameToSave = Path.Combine(Server.MapPath("~/App_Data/uploads"), Request.Cookies["UserName"].Value.ToString() + "_" + _FileName);
                        file.SaveAs(FileNameToSave);

                        //read file index excel
                        FileStream fsIndex = new FileStream(FileNameToSave, FileMode.Open);
                        XSSFWorkbook wbIndex = new XSSFWorkbook(fsIndex);
                        ISheet sheetIndex = wbIndex.GetSheetAt(0);
                        var rowIndex = sheetIndex.GetRow(0);
                        //read file template excel
                        FileStream fsTemplate = new FileStream(_TemplatePath, FileMode.Open);
                        XSSFWorkbook wbTemplate = new XSSFWorkbook(fsTemplate);
                        ISheet sheetTemplate = wbTemplate.GetSheetAt(0);
                        var rowTemplate = sheetTemplate.GetRow(0);

                        //vòng lặp kiểm tra file upload != file template
                        for (int i = 0; i <= 17; i++)
                        {
                            if (rowTemplate.GetCell(0).StringCellValue != rowIndex.GetCell(0).StringCellValue)
                            {
                                ViewBag.Message = "Sai định dạng template";
                                return PartialView();
                            }
                        }
                        if (sheetIndex.LastRowNum == 0)
                        {
                            ViewBag.Message = "File rỗng!";
                            return PartialView();
                        }
                        bool checkError = true;

                        for (int i = 1; i <= sheetIndex.LastRowNum; i++)
                        {

                            string error = "";
                            var rowNow = sheetIndex.GetRow(i);
                            // cộng lỗi vào chuổi error 
                            //CHECK_IS_NULL kiểm tra khác rổng
                            //CHECK_NUMBER_TYPE kiểm tra dạng sổ
                            //CHECK_ISNULL_AND_NUMBER kiểm khác rống và dạng sô
                            error += Check_Format_Excel(sheetIndex, CHECK_IS_NULL, i, 0);
                            error += Check_Format_Excel(sheetIndex, CHECK_IS_NULL, i, 1);
                            error += Check_Format_Excel(sheetIndex, CHECK_IS_NULL, i, 2);
                            error += Check_Format_Excel(sheetIndex, CHECK_IS_NULL, i, 3);
                            error += Check_Format_Excel(sheetIndex, CHECK_IS_NULL, i, 4);
                            error += Check_Format_Excel(sheetIndex, CHECK_IS_NULL, i, 5);
                            error += Check_Format_Excel(sheetIndex, CHECK_IS_NULL, i, 6);
                            error += Check_Format_Excel(sheetIndex, CHECK_IS_NULL, i, 7);
                            error += Check_Format_Excel(sheetIndex, CHECK_IS_NULL, i, 8);
                            error += Check_Format_Excel(sheetIndex, CHECK_IS_NULL, i, 9);

                            if (error == "")
                            {
                                EM_List EMlist = new EM_List();
                                EMlist.EM_Position_Code = rowNow.GetCell(0).ToString();
                                EMlist.EM_Position_Full_Name = rowNow.GetCell(1).ToString();
                                EMlist.User_Code = rowNow.GetCell(2).ToString();
                                EMlist.EM_Code = rowNow.GetCell(3).ToString();
                                EMlist.EM_Full_Name = rowNow.GetCell(4).ToString();
                                EMlist.Contract_Code = rowNow.GetCell(5).ToString();
                                EMlist.Signal_Day = Int32.Parse(rowNow.GetCell(6).ToString());
                                EMlist.Signal_Month = Int32.Parse(rowNow.GetCell(7).ToString());
                                EMlist.Signal_Year = Int32.Parse(rowNow.GetCell(8).ToString());
                                EMlist.EM_Status = rowNow.GetCell(9).ToString();
                                if (rowNow.GetCell(10) != null) { EMlist.Note = rowNow.GetCell(10).ToString(); }
                                //userbasic.User_Code = userbasic.Role + 
                                //db.User_Basic.Add(userbasic);
                                //db.SaveChanges();
                            }
                            else
                            {
                                checkError = false;
                                var RowErrorIndex = sheetTemplate.CreateRow(i);
                                for (int j = 0; j <= 17; j++)
                                {
                                    if (rowNow.GetCell(j) != null)
                                        RowErrorIndex.CreateCell(j).SetCellValue(rowNow.GetCell(j).ToString());
                                }
                                RowErrorIndex.CreateCell(17).SetCellValue(error);

                            }
                        }
                        if (!checkError)
                        {
                            ErrorFileName = Request.Cookies["UserName"].Value.ToString() + "_EMList_Error.xlsx";
                            string ErrorPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), ErrorFileName);
                            FileStream fsError = new FileStream(ErrorPath, FileMode.Create);
                            wbTemplate.Write(fsError);
                            fsError.Close();
                            fsTemplate.Close();
                            fsIndex.Close();
                            ViewBag.Message = "Lỗi trong file upload.";
                            ViewBag.ErrorEMListFileName = ErrorFileName;
                            return PartialView();
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Sai định dạng file .xlsx";
                        return PartialView();
                    }
                }
                ViewBag.Message = "Cập nhật thành công!";
                return PartialView();
            }
            catch
            {
                ViewBag.Message = "Cập nhật thất bại!";
                return PartialView();
            }
        }
        //cập nhật user hàng loạt
        [HttpPost, ActionName("User_Basic_MultiUpload")]
        public ActionResult User_Basic_MultiUpload(HttpPostedFileBase file)
        {

            try
            {
                if (file.ContentLength > 0)
                {
                    ViewBag.ErrorFileName = "";
                    ViewBag.CurrentFunctionExcelUpload = "UserBasic";
                    string ErrorFileName = "";
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/App_Data/uploads"), _FileName);
                    string _TemplatePath = Path.Combine(Server.MapPath("~/App_Data/uploads"), "UserBasic_Template.xlsx");
                    string _FileExtension = Path.GetExtension(_FileName);
                    if (_FileExtension == ".xlsx")
                    {
                        // lưu file excel lại với tên Usercode_filename.xlsxS
                        string FileNameToSave = Path.Combine(Server.MapPath("~/App_Data/uploads"), Request.Cookies["UserName"].Value.ToString() + "_" + _FileName);
                        file.SaveAs(FileNameToSave);

                        //read file index excel
                        FileStream fsIndex = new FileStream(FileNameToSave, FileMode.Open);
                        XSSFWorkbook wbIndex = new XSSFWorkbook(fsIndex);
                        ISheet sheetIndex = wbIndex.GetSheetAt(0);
                        var rowIndex = sheetIndex.GetRow(0);
                        //read file template excel
                        FileStream fsTemplate = new FileStream(_TemplatePath, FileMode.Open);
                        XSSFWorkbook wbTemplate = new XSSFWorkbook(fsTemplate);
                        ISheet sheetTemplate = wbTemplate.GetSheetAt(0);
                        var rowTemplate = sheetTemplate.GetRow(0);

                        //vòng lặp kiểm tra file upload != file template
                        for (int i = 0; i <= 17; i++)
                        {
                            if (rowTemplate.GetCell(0).StringCellValue != rowIndex.GetCell(0).StringCellValue)
                            {
                                ViewBag.Message = "Sai định dạng template";
                                return PartialView();
                            }
                        }
                        if (sheetIndex.LastRowNum == 0)
                        {
                            ViewBag.Message = "File rỗng!";
                            return PartialView();
                        }
                        bool checkError = true;

                        for (int i = 1; i <= sheetIndex.LastRowNum; i++)
                        {

                            string error = "";
                            var rowNow = sheetIndex.GetRow(i);
                            // cộng lỗi vào chuổi error 
                            //CHECK_IS_NULL kiểm tra khác rổng
                            //CHECK_NUMBER_TYPE kiểm tra dạng sổ
                            //CHECK_ISNULL_AND_NUMBER kiểm khác rống và dạng sô
                            error += Check_Format_Excel(sheetIndex, CHECK_IS_NULL, i, 0);
                            error += Check_Format_Excel(sheetIndex, CHECK_IS_NULL, i, 1);
                            error += Check_Format_Excel(sheetIndex, CHECK_NUMBER_TYPE, i, 3);
                            error += Check_Format_Excel(sheetIndex, CHECK_NUMBER_TYPE, i, 4);
                            error += Check_Format_Excel(sheetIndex, CHECK_ISNULL_AND_NUMBER, i, 5);
                            error += Check_Format_Excel(sheetIndex, CHECK_IS_NULL, i, 6);
                            error += Check_Format_Excel(sheetIndex, CHECK_IS_NULL, i, 7);
                            error += Check_Format_Excel(sheetIndex, CHECK_ISNULL_AND_NUMBER, i, 8);
                            if ((rowNow.GetCell(9) == null) && (rowNow.GetCell(10) == null)) { error += "Missing Email or Phone | "; }
                            error += Check_Format_Excel(sheetIndex, CHECK_IS_NULL, i, 11);
                            error += Check_Format_Excel(sheetIndex, CHECK_IS_NULL, i, 12);
                            error += Check_Format_Excel(sheetIndex, CHECK_IS_NULL, i, 13);

                            if (error == "")
                            {
                                //lưu lại nếu không có lỗi trong rownow
                                
                                User_Basic userbasic = new User_Basic();

                                userbasic.User_First_Name = rowNow.GetCell(0).ToString();
                                userbasic.User_Last_Name = rowNow.GetCell(1).ToString();
                                if (rowNow.GetCell(2) != null) { userbasic.User_Alternative_Name = rowNow.GetCell(2).StringCellValue; }
                                userbasic.User_DoB_Day = Int32.Parse(rowNow.GetCell(3).ToString());
                                userbasic.User_DoB_Month = Int32.Parse(rowNow.GetCell(4).ToString());
                                userbasic.User_DoB_Year = Int32.Parse(rowNow.GetCell(5).ToString());
                                userbasic.Gender = rowNow.GetCell(6).ToString();
                                userbasic.Address_Detail = rowNow.GetCell(7).ToString();
                                userbasic.Address_District = Int32.Parse(rowNow.GetCell(8).ToString());
                                if (rowNow.GetCell(9) != null) { userbasic.Email = rowNow.GetCell(9).ToString(); }
                                if (rowNow.GetCell(10) != null) { userbasic.Phone = rowNow.GetCell(10).ToString(); }
                                userbasic.Role = rowNow.GetCell(11).StringCellValue;
                                userbasic.Client_Code = rowNow.GetCell(12).ToString();
                                userbasic.Education_Type_Code = rowNow.GetCell(13).ToString();
                                if (rowNow.GetCell(14) != null) { userbasic.Job_Type_Code = rowNow.GetCell(14).ToString(); }
                                if (rowNow.GetCell(15) != null) { userbasic.Job_Detail = rowNow.GetCell(15).ToString(); }
                                userbasic.CreateBy = Request.Cookies["UserName"].Value;
                                userbasic.CreateDate = DateTime.Today;
                                //userbasic.User_Code = userbasic.Role + 
                                db.User_Basic.Add(userbasic);
                                db.SaveChanges();
                            }
                            else
                            {
                                checkError = false;
                                string tempxx = rowNow.GetCell(6).StringCellValue;
                                var RowErrorIndex = sheetTemplate.CreateRow(i);
                                for (int j = 0; j <= 17; j++)
                                {
                                    if (rowNow.GetCell(j) != null)
                                        RowErrorIndex.CreateCell(j).SetCellValue(rowNow.GetCell(j).ToString());
                                }
                                RowErrorIndex.CreateCell(17).SetCellValue(error);
                                string tempxxx = sheetTemplate.GetRow(1).GetCell(6).StringCellValue;

                            }
                        }
                        if (!checkError)
                        {
                            ErrorFileName = Request.Cookies["UserName"].Value.ToString() + "_Error.xlsx";
                            string ErrorPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), ErrorFileName);
                            FileStream fsError = new FileStream(ErrorPath, FileMode.Create);
                            wbTemplate.Write(fsError);
                            fsError.Close();
                            fsTemplate.Close();
                            fsIndex.Close();
                            ViewBag.Message = "Lỗi trong file upload.";
                            ViewBag.ErrorFileName = ErrorFileName;
                            return PartialView();
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Sai định dạng file .xlsx";
                        return PartialView();
                    }
                }
                ViewBag.Message = "Cập nhật thành công!";
                return PartialView();
            }
            catch
            {
                ViewBag.Message = "Cập nhật thất bại!";
                return PartialView();
            }
        }

        //xóa userbasic
        public ActionResult User_Basic_Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Basic user_Basic = db.User_Basic.Find(id);
            if (user_Basic == null)
            {
                return HttpNotFound();
            }
            return PartialView(user_Basic);
        }
        // POST: Admin/User_Basic/Delete/5
        [HttpPost, ActionName("User_Basic_Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult UserBasic_DeleteConfirmed(int id)
        {
            User_Basic user_Basic = db.User_Basic.Find(id);
            user_Basic.Status = "Disable";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EMList_Index()
        {
            return PartialView(db.EM_List.ToList());
        }


        public ActionResult EMList_Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EM_List eM_List = db.EM_List.Find(id);
            if (eM_List == null)
            {
                return HttpNotFound();
            }
            return PartialView(eM_List);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EMList_Edit([Bind(Include = "EM_Position_Code,EM_Position_Full_Name,EM_ID,User_Code,EM_Code,EM_Full_Name,Contract_Code,Signal_Day,Signal_Month,Signal_Year,EM_Status,Note,CreateDate,CreateBy")] EM_List eM_List)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eM_List).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eM_List);
        }

        public ActionResult EMList_Create()
        {
            return PartialView();
        }


        // POST: Admin/EM_List/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EMList_Create([Bind(Include = "EM_Position_Code,EM_Position_Full_Name,EM_ID,User_Code,EM_Code,EM_Full_Name,Contract_Code,Signal_Day,Signal_Month,Signal_Year,EM_Status,Note,CreateDate,CreateBy")] EM_List eM_List)
        {
            if (ModelState.IsValid)
            {
                db.EM_List.Add(eM_List);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return PartialView(eM_List);
        }
        public ActionResult Client_Create()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Client_Create([Bind(Include = "Client_ID,Client_Code,Client_Short_Name,Client_Full_Name,Approach_Year,EM_Code,EM_Full_Name,CL_Field_Code,CL_Field_Full_Name,Client_Phone,Client_website,Client_mail,Client_Owner,Client_Owner_Phone,Client_Owner_Email,Client_Principal,Client_Principal_Phone,Client_Principal_Email,Address_Detail,Address_District,Status,Note,Type,CreateDate,CreateBy")] Client_Information client_Information)
        {
            if (ModelState.IsValid)
            {
                if (Request.Cookies["UserName"] != null)
                {
                    client_Information.CreateBy = Request.Cookies["UserName"].Value;
                }
                db.Client_Information.Add(client_Information);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(client_Information);
        }
        public ActionResult Client_Index()
        {
            return PartialView(db.Client_Information.ToList());
        }
        public ActionResult Client_Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client_Information client_information = new Client_Information();
            if (client_information == null)
            {
                return HttpNotFound();
            }
            return PartialView(client_information);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Client_Edit([Bind(Include = "Client_ID,Client_Code,Client_Short_Name,Client_Full_Name,Approach_Year,EM_Code,EM_Full_Name,CL_Field_Code,CL_Field_Full_Name,Client_Phone,Client_website,Client_mail,Client_Owner,Client_Owner_Phone,Client_Owner_Email,Client_Principal,Client_Principal_Phone,Client_Principal_Email,Address_Detail,Address_District,Status,Note,Type,CreateDate,CreateBy")] Client_Information client_Information)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client_Information).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(client_Information);
        }

        public ActionResult Ref_Region()
        {
            return PartialView(db.Ref_Region.ToList());
        }

        public ActionResult Ref_Province()
        {
            return PartialView(db.Ref_Province.ToList());
        }

        public ActionResult Ref_District()
        {
            return PartialView(db.Ref_District.ToList());
        }
    }
}