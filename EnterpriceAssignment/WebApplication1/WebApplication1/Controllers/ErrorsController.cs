using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ErrorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Errors
        public ActionResult Index()
        {
            return View(db.Errors.ToList());
        }

        // GET: Errors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Error error = db.Errors.Find(id);
            if (error == null)
            {
                return HttpNotFound();
            }
            return View(error);
        }

        // GET: Errors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Errors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ErrorId,errorValue")] Error error)
        {
            if (ModelState.IsValid)
            {
                db.Errors.Add(error);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(error);
        }

        // GET: Errors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Error error = db.Errors.Find(id);
            if (error == null)
            {
                return HttpNotFound();
            }
            return View(error);
        }

        // POST: Errors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ErrorId,errorValue")] Error error)
        {
            if (ModelState.IsValid)
            {
                db.Entry(error).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(error);
        }

        // GET: Errors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Error error = db.Errors.Find(id);
            if (error == null)
            {
                return HttpNotFound();
            }
            return View(error);
        }

        // POST: Errors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Error error = db.Errors.Find(id);
            db.Errors.Remove(error);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public string LogError(string username, Exception ex, string page, string method)
        {
            string errorValue = "";
            if (ex.InnerException != null)
            {
                Trace.WriteLine(
                    errorValue = String.Format("Username: {0}, Exception: {1}, Message: {2}, Page: {3}, Method: {4}, Inner Exception: {5}," +
                    "Date: {6}",
                    username, ex.GetType().ToString(), ex.Message, page, method, ex.InnerException.Message, DateTime.Now
                    )
                    );
            }
            else
            {
                Trace.WriteLine(
                   errorValue = String.Format("Username: {0}, Exception: {1}, Message: {2}, Page: {3}, Method: {4}, Inner Exception: {5}, Date: {6}",
                   username, ex.GetType().ToString(), ex.Message, page, method, "null", DateTime.Now
                   )
                   );
            }
            return errorValue;
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
