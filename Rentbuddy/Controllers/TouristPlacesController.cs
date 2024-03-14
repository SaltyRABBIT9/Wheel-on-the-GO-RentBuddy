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
    public class TouristPlacesController : Controller
    {
        private RentBuddyEntities db = new RentBuddyEntities();

        // GET: TouristPlaces
        public ActionResult Index()
        {
            return View(db.TouristPlaces.ToList());
        }

        // GET: TouristPlaces/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TouristPlace touristPlace = db.TouristPlaces.Find(id);
            if (touristPlace == null)
            {
                return HttpNotFound();
            }
            float x, y;

            x = float.Parse(touristPlace.TouuristPlace_Lat);
            ViewBag.lat = x;
            y = float.Parse(touristPlace.TouristPlace_long);
            ViewBag.lng = y;
            ViewBag.PlaceImage = touristPlace.TouristPlace_Image;
            return View(touristPlace);
        }

        // GET: TouristPlaces/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TouristPlaces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TouristPlace_ID,TouristPlace_Name,TouristPlace_Image,TouristPlace_Description,TouristPlace_Location,TouuristPlace_Lat,TouristPlace_long")] TouristPlace touristPlace)
        {
            if (ModelState.IsValid)
            {
                db.TouristPlaces.Add(touristPlace);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(touristPlace);
        }

        // GET: TouristPlaces/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TouristPlace touristPlace = db.TouristPlaces.Find(id);
            if (touristPlace == null)
            {
                return HttpNotFound();
            }
            return View(touristPlace);
        }

        // POST: TouristPlaces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TouristPlace tp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PlaceList");
            }
            return View(tp);
        }

        // GET: TouristPlaces/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TouristPlace touristPlace = db.TouristPlaces.Find(id);
            if (touristPlace == null)
            {
                return HttpNotFound();
            }
            return View(touristPlace);
        }

        // POST: TouristPlaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TouristPlace touristPlace = db.TouristPlaces.Find(id);
            db.TouristPlaces.Remove(touristPlace);
            db.SaveChanges();
            return RedirectToAction("PlaceList");
        }








        [HttpGet]
        public ActionResult PlaceList()
        {
            return View(db.TouristPlaces.ToList());
        }




        [HttpGet]
        public ActionResult AddPlace()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddPlace(TouristPlace tplace)
        {
            string fileName = Path.GetFileNameWithoutExtension(tplace.TouristImageFile.FileName);
            string extension = Path.GetExtension(tplace.TouristImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            tplace.TouristPlace_Image = "~/PlaceImage/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/PlaceImage/"), fileName);
            tplace.TouristImageFile.SaveAs(fileName);

            if (ModelState.IsValid)
            {
                db.TouristPlaces.Add(tplace);
                db.SaveChanges();
                ViewBag.Success = "successfully added";
            }
            else
            {
                ViewBag.Failed = "Added failed! Try Again";
            }
            ModelState.Clear();
            return View();
        }




        public ActionResult TouristPlace()
        {
            return View(db.TouristPlaces.ToList());
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
