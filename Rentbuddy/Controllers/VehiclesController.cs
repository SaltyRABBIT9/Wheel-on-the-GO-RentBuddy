using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Rentbuddy.Models;

namespace Rentbuddy.Controllers
{
    
    public class VehiclesController : Controller
    {
        private RentBuddyEntities db = new RentBuddyEntities();

        // GET: Vehicles
        public ActionResult Index()
        {
            var vehicles = db.Vehicles.Include(v => v.Category);
            return View(vehicles.ToList());
        }

        // GET: Vehicles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // GET: Vehicles/Create
        public ActionResult Create()
        {
            ViewBag.Category_ID = new SelectList(db.Categories, "Category_ID", "Category_Name");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Vehicle_ID,Category_ID,Vehicle_Name,Vehicle_LicenseNO,Vehicle_Status,Vehicle_Image,Vehicle_Engine,Vehicle_Price")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Vehicles.Add(vehicle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_ID = new SelectList(db.Categories, "Category_ID", "Category_Name", vehicle.Category_ID);
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_ID = new SelectList(db.Categories, "Category_ID", "Category_Name", vehicle.Category_ID);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vehicle vcl)
        {
            string fileName = Path.GetFileNameWithoutExtension(vcl.VehicleImageFile.FileName);
            string extension = Path.GetExtension(vcl.VehicleImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            vcl.Vehicle_Image = "~/VehicleImage/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/VehicleImage/"), fileName);
            vcl.VehicleImageFile.SaveAs(fileName);

            if (ModelState.IsValid)
            {
                db.Entry(vcl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AllVehicle");
            }
            ViewBag.Category_ID = new SelectList(db.Categories, "Category_ID", "Category_Name", vcl.Category_ID);
            return View(vcl);
        }

        // GET: Vehicles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            db.Vehicles.Remove(vehicle);
            db.SaveChanges();
            return RedirectToAction("AllVehicle");
        }







        public ActionResult AllVehicle()
        {
            var vehicles = db.Vehicles.Include(v => v.Category);
            return View(vehicles.ToList());
        }





        [HttpGet]
        public ActionResult AddVehicle()
        {
            ViewBag.Category_ID = new SelectList(db.Categories, "Category_ID", "Category_Name");
            return View();
        }

        [HttpPost]
        public ActionResult AddVehicle(Vehicle vc)
        {
            string fileName = Path.GetFileNameWithoutExtension(vc.VehicleImageFile.FileName);
            string extension = Path.GetExtension(vc.VehicleImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            vc.Vehicle_Image = "~/VehicleImage/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/VehicleImage/"), fileName);
            vc.VehicleImageFile.SaveAs(fileName);

            if (ModelState.IsValid)
            {
                db.Vehicles.Add(vc);
                db.SaveChanges();
                ViewBag.Success = "Successfully added";
            }
            else
            {
                ViewBag.Failed = "Something went wrong! PLease try again";
            }
            ViewBag.Category_ID = new SelectList(db.Categories, "Category_ID", "Category_Name", vc.Category_ID);
            ModelState.Clear();
            return View();
        }





        [HttpGet]
        public ActionResult AvailableVehicle()
        {
            var available = db.Vehicles.Where(u => u.Vehicle_Status == "Available").ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (available != null)
            {
                return View(available);
            }
            else
            {
                ViewBag.Available = "No Vehicle Available";
                return View();
            }
        }

        [HttpGet]
        public ActionResult RentVehicle()
        {
            var rent = db.Vehicles.Where(u => u.Vehicle_Status == "Rent").ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (rent != null)
            {
                return View(rent);
            }
            else
            {
                ViewBag.Rent = "No Vehicle On Rent";
                return View();
            }
        }






        public ViewResult Bike()
        {
            var bike = db.Vehicles.Where(u => u.Category_ID == 1).ToList();
            if (bike != null)
            {
                return View(bike);
            }
            else
            {
                ViewBag.Bike = "There is no vehicle in this category";
                return View();
            }
        }

        public ViewResult Car()
        {
            var car = db.Vehicles.Where(u => u.Category_ID == 2).ToList();
            if (car != null)
            {
                return View(car);
            }
            else
            {
                ViewBag.Car = "There is no vehicle in this category";
                return View();
            }
        }


        public ViewResult Jeep()
        {
            var jeep = db.Vehicles.Where(u => u.Category_ID == 3).ToList();
            if (jeep != null)
            {
                return View(jeep);
            }
            else
            {
                ViewBag.Jeep = "There is no vehicle in this category";
                return View();
            }
        }

        public ViewResult MicroBus()
        {
            var micro = db.Vehicles.Where(u => u.Category_ID == 4).ToList();
            if (micro != null)
            {
                return View(micro);
            }
            else
            {
                ViewBag.Micro = "There is no vehicle in this category";
                return View();
            }
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
