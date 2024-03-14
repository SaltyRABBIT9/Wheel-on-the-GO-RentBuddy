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
    public class CustomersController : Controller
    {
        private RentBuddyEntities db = new RentBuddyEntities();

        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Customer_ID,Customer_Name,Customer_Email,Customer_PhoneNO,Customer_Address,Customer_History,Customer_Password,Customer_Image")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer custm)
        {
            string fileName = Path.GetFileNameWithoutExtension(custm.CustomerImageFile.FileName);
            string extension = Path.GetExtension(custm.CustomerImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            custm.Customer_Image = "~/CustomerImage/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/CustomerImage/"), fileName);
            custm.CustomerImageFile.SaveAs(fileName);

            if (ModelState.IsValid)
            {
                db.Entry(custm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CustomerProfile");
            }
            return View(custm);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Vehicles");
        }










        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SignUp(Customer cust)
        {
            string fileName = Path.GetFileNameWithoutExtension(cust.CustomerImageFile.FileName);
            string extension = Path.GetExtension(cust.CustomerImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            cust.Customer_Image = "~/CustomerImage/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/CustomerImage/"), fileName);
            cust.CustomerImageFile.SaveAs(fileName);

            if (ModelState.IsValid)
            {
                db.Customers.Add(cust);
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
        public ActionResult Login(TempCustomer tempCustomer)
        {
            if (ModelState.IsValid)
            {
                var customer = db.Customers.Where(c => c.Customer_Name.Equals(tempCustomer.Customer_Name)
                                && c.Customer_Email.Equals(tempCustomer.Customer_Email)
                                && c.Customer_Password.Equals(tempCustomer.Customer_Password)).FirstOrDefault();

                if (customer != null)
                {
                    FormsAuthentication.SetAuthCookie(tempCustomer.Customer_Name, false);
                    Session["CustomerName"] = customer.Customer_Name;
                    Session["CustomerEmail"] = customer.Customer_Email;
                    Session["type"] = "Customer";
                    return RedirectToAction("CustomerProfile");
                    //return Content("Login Successful!");
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
        public ActionResult CustomerProfile()
        {
            String email = Convert.ToString(Session["CustomerEmail"]);
            var customer = db.Customers.Where(u => u.Customer_Email.Equals(email)).FirstOrDefault();
            return View(customer);
        }



        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index","Vehicles");

        }


        public ActionResult CustomerList()
        {
            return View(db.Customers.ToList());
        }


        public ActionResult ADelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ADeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("CustomerList", "Customers");
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
