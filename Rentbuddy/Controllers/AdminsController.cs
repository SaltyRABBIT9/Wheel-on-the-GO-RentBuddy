using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Rentbuddy.Models;

namespace Rentbuddy.Controllers
{
    public class AdminsController : Controller
    {
        private RentBuddyEntities db = new RentBuddyEntities();

        // GET: Admins
        public ActionResult Index()
        {
            return View(db.Admins.ToList());
        }

        // GET: Admins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Admin_ID,Admin_Name,Admin_Email,Admin_PhoneNO,Admin_Address,Admin_Password,Admin_Image")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Admin adm)
        {

            string fileName = Path.GetFileNameWithoutExtension(adm.AdminImageFile.FileName);
            string extension = Path.GetExtension(adm.AdminImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            adm.Admin_Image = "~/AdminImage/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/AdminImage/"), fileName);
            adm.AdminImageFile.SaveAs(fileName);

            if (ModelState.IsValid)
            {
                db.Entry(adm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdminProfile");
            }
            return View(adm);
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }


        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
            db.SaveChanges();
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index","Vehicles");

            
        }


















        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }



        [HttpPost]
        public ActionResult SignUp(Admin aust)
        {
            string fileName = Path.GetFileNameWithoutExtension(aust.AdminImageFile.FileName);
            string extension = Path.GetExtension(aust.AdminImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            aust.Admin_Image = "~/AdminImage/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/AdminImage/"), fileName);
            aust.AdminImageFile.SaveAs(fileName);

            if (ModelState.IsValid)
            {
                db.Admins.Add(aust);
                db.SaveChanges();
                ViewBag.Success = "successfully registered";
            }
            else
            {
                ViewBag.Failed = "Registration failed! Something went wrong! Please try again";
            }
            ModelState.Clear();
            return View();
        }





        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Login(TempAdmin tempAdmin)
        {
            if (ModelState.IsValid)
            {
                var admin = db.Admins.Where(c => c.Admin_Name.Equals(tempAdmin.Admin_Name)
                        && c.Admin_Email.Equals(tempAdmin.Admin_Email)
                        && c.Admin_Password.Equals(tempAdmin.Admin_Password)).FirstOrDefault();

                if (admin != null)
                {
                    FormsAuthentication.SetAuthCookie(tempAdmin.Admin_Name, false);
                    Session["AdminEmail"] = admin.Admin_Email;
                    Session["type"] = "Admin";
                    return RedirectToAction("AdminView");
                }
                else
                {
                    ViewBag.Failed = "Login Faild! Please try again";
                    return View();
                }
            }
            return View();
        }







        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Vehicles");
        }






        [HttpGet]
        public ActionResult AdminProfile()
        {
            String email = Convert.ToString(Session["AdminEmail"]);
            var admin = db.Admins.Where(u => u.Admin_Email.Equals(email)).FirstOrDefault();
            return View(admin);
        }



        public ActionResult AdminView()
        {
            return View();
        }


        public ActionResult AdminList()
        {
            return View(db.Admins.ToList());
        }







        public ActionResult AdminDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult AdminDeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("AdminList");
        }


        public ActionResult AllAdmin()
        {
            return View(db.Admins.ToList());
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
