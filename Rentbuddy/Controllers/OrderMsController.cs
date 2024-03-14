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
    public class OrderMsController : Controller
    {
        private RentBuddyEntities db = new RentBuddyEntities();

        // GET: OrderMs
        public ActionResult Index()
        {
            var orderMs = db.OrderMs.Include(o => o.Customer).Include(o => o.Vehicle);
            return View(orderMs.ToList());
        }

        // GET: OrderMs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderM orderM = db.OrderMs.Find(id);
            if (orderM == null)
            {
                return HttpNotFound();
            }
            return View(orderM);
        }

        // GET: OrderMs/Create
        public ActionResult Create()
        {
            ViewBag.Customer_ID = new SelectList(db.Customers, "Customer_ID", "Customer_Name");
            ViewBag.Vehicle_ID = new SelectList(db.Vehicles, "Vehicle_ID", "Vehicle_Name");
            return View();
        }

        // POST: OrderMs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Order_ID,Customer_ID,Vehicle_ID,Order_RentHour,Order_FromLocation,Order_ToLocation,Order_TotalPrice")] OrderM orderM)
        {
            if (ModelState.IsValid)
            {
                db.OrderMs.Add(orderM);
                db.SaveChanges();
                Session["OrderID"] = orderM.Order_ID;
                return RedirectToAction("PaymentView");
            }

            ViewBag.Customer_ID = new SelectList(db.Customers, "Customer_ID", "Customer_Name", orderM.Customer_ID);
            ViewBag.Vehicle_ID = new SelectList(db.Vehicles, "Vehicle_ID", "Vehicle_Name", orderM.Vehicle_ID);
            return View(orderM);
        }

        // GET: OrderMs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderM orderM = db.OrderMs.Find(id);
            if (orderM == null)
            {
                return HttpNotFound();
            }
            ViewBag.Customer_ID = new SelectList(db.Customers, "Customer_ID", "Customer_Name", orderM.Customer_ID);
            ViewBag.Vehicle_ID = new SelectList(db.Vehicles, "Vehicle_ID", "Vehicle_Name", orderM.Vehicle_ID);
            return View(orderM);
        }

        // POST: OrderMs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Order_ID,Customer_ID,Vehicle_ID,Order_RentHour,Order_FromLocation,Order_ToLocation,Order_TotalPrice")] OrderM orderM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Customer_ID = new SelectList(db.Customers, "Customer_ID", "Customer_Name", orderM.Customer_ID);
            ViewBag.Vehicle_ID = new SelectList(db.Vehicles, "Vehicle_ID", "Vehicle_Name", orderM.Vehicle_ID);
            return View(orderM);
        }

        // GET: OrderMs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderM orderM = db.OrderMs.Find(id);
            if (orderM == null)
            {
                return HttpNotFound();
            }
            return View(orderM);
        }

        // POST: OrderMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderM orderM = db.OrderMs.Find(id);
            db.OrderMs.Remove(orderM);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult PaymentView()
        {
            int oid = Convert.ToInt32(Session["OrderID"]);
            var order = db.OrderMs.Where(u => u.Order_ID.Equals(oid)).FirstOrDefault();

            return View(order);
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
