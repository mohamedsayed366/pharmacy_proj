using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Pharma.DAL;
using Pharma.Models;
using PagedList;

namespace Pharma.Controllers
{
    public class CustomerController : Controller
    {
        private PharmaContext db = new PharmaContext();

        // GET: Customer
        public ActionResult Index(string sortOrder, string searchString , string currentFilter, int? page)
        {
            if (Session["userName"] != null)
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";


                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                ViewBag.CurrentFilter = searchString;
                var customers = from s in db.Customers select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    customers = customers.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                    || s.FirstName.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case
                    "name_desc":
                        customers = customers.OrderByDescending(s => s.LastName);
                        break;
                    case
                    "Date":
                        customers = customers.OrderBy(s => s.FirstName);
                        break;
                    case
                    "date_desc":
                        customers = customers.OrderByDescending(s => s.FirstName); break;
                    default:
                        customers = customers.OrderBy(s => s.LastName);
                        break;
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(customers.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Login");
            }
        }















        // GET: Customer/Details/5
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

        // GET: Customer/Create
        public ActionResult Create()
        {
            if (Session["userName"] != null)
                return View();
            else
                return RedirectToAction("Login");
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,Password,Title,VisaNumber")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customer/Edit/5
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

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,Password,Title,VisaNumber")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customer/Delete/5
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

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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


        public ActionResult Login()
        {
            Session["userName"] = null; 
            return View();
        }



        [HttpPost]
        public ActionResult Login([Bind(Include = "Email,Password")] Customer customer)
        {

            var rec = db.Customers.Where(x => x.Email == customer.Email && x.Password == customer.Password).ToList().FirstOrDefault();

            if (rec.Email== "Admin@gmail.com" && rec.Password == "Admin")
            {
                return RedirectToAction("Index");
            }


          else if (rec != null)
            {
                Session["userName"] = rec.FirstName;
                return RedirectToAction("login");
            }
            else
            {
                ViewBag.error = "Invalid User";
                return View(customer);
            }


        }
    }
  
}
