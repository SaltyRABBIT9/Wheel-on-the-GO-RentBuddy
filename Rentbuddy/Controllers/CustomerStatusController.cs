using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Rentbuddy.Models;

namespace Rentbuddy.Controllers
{
    public class CustomerStatusController : Controller
    {
        private RentBuddyEntities db = new RentBuddyEntities();

        // GET: CustomerStatus
        public ActionResult Index()
        {
            var customerStatus = db.CustomerStatus.Include(c => c.Customer);
            return View(customerStatus.ToList());
        }

        // GET: CustomerStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerStatu customerStatu = db.CustomerStatus.Find(id);
            if (customerStatu == null)
            {
                return HttpNotFound();
            }
            return View(customerStatu);
        }

        // GET: CustomerStatus/Create
        public ActionResult Create()
        {
            ViewBag.Customer_ID = new SelectList(db.Customers, "Customer_ID", "Customer_Name");
            return View();
        }

        // POST: CustomerStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Status_ID,Customer_ID,Status_Time,Satus_Date,Status_Image,Status")] CustomerStatu customerStatu)
        {
            if (ModelState.IsValid)
            {
                db.CustomerStatus.Add(customerStatu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Customer_ID = new SelectList(db.Customers, "Customer_ID", "Customer_Name", customerStatu.Customer_ID);
            return View(customerStatu);
        }

        // GET: CustomerStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerStatu customerStatu = db.CustomerStatus.Find(id);
            if (customerStatu == null)
            {
                return HttpNotFound();
            }
            ViewBag.Customer_ID = new SelectList(db.Customers, "Customer_ID", "Customer_Name", customerStatu.Customer_ID);
            return View(customerStatu);
        }

        // POST: CustomerStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Status_ID,Customer_ID,Status_Time,Satus_Date,Status_Image,Status")] CustomerStatu customerStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerStatu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Customer_ID = new SelectList(db.Customers, "Customer_ID", "Customer_Name", customerStatu.Customer_ID);
            return View(customerStatu);
        }

        // GET: CustomerStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerStatu customerStatu = db.CustomerStatus.Find(id);
            if (customerStatu == null)
            {
                return HttpNotFound();
            }
            return View(customerStatu);
        }

        // POST: CustomerStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerStatu customerStatu = db.CustomerStatus.Find(id);
            db.CustomerStatus.Remove(customerStatu);
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
