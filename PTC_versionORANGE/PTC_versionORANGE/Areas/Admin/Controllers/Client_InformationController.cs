using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PTC_versionORANGE.Models;

namespace PTC_versionORANGE.Areas.Admin.Controllers
{
    public class Client_InformationController : Controller
    {
        private PTCEntities db = new PTCEntities();

        // GET: Admin/Client_Information
        public ActionResult Index()
        {
            return View(db.Client_Information.ToList());
        }

        // GET: Admin/Client_Information/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client_Information client_Information = db.Client_Information.Find(id);
            if (client_Information == null)
            {
                return HttpNotFound();
            }
            return View(client_Information);
        }

        // GET: Admin/Client_Information/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Client_Information/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Client_ID,Client_Code,Client_Short_Name,Client_Full_Name,Approach_Year,EM_Code,EM_Full_Name,CL_Field_Code,CL_Field_Full_Name,Client_Phone,Client_website,Client_mail,Client_Owner,Client_Owner_Phone,Client_Owner_Email,Client_Principal,Client_Principal_Phone,Client_Principal_Email,Address_Detail,Address_District,Status,Note,Type,CreateDate,CreateBy")] Client_Information client_Information)
        {
            if (ModelState.IsValid)
            {
                db.Client_Information.Add(client_Information);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(client_Information);
        }

        // GET: Admin/Client_Information/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client_Information client_Information = db.Client_Information.Find(id);
            if (client_Information == null)
            {
                return HttpNotFound();
            }
            return View(client_Information);
        }

        // POST: Admin/Client_Information/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Client_ID,Client_Code,Client_Short_Name,Client_Full_Name,Approach_Year,EM_Code,EM_Full_Name,CL_Field_Code,CL_Field_Full_Name,Client_Phone,Client_website,Client_mail,Client_Owner,Client_Owner_Phone,Client_Owner_Email,Client_Principal,Client_Principal_Phone,Client_Principal_Email,Address_Detail,Address_District,Status,Note,Type,CreateDate,CreateBy")] Client_Information client_Information)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client_Information).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client_Information);
        }

        // GET: Admin/Client_Information/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client_Information client_Information = db.Client_Information.Find(id);
            if (client_Information == null)
            {
                return HttpNotFound();
            }
            return View(client_Information);
        }

        // POST: Admin/Client_Information/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client_Information client_Information = db.Client_Information.Find(id);
            db.Client_Information.Remove(client_Information);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
