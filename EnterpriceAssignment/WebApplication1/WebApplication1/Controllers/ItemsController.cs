using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.Entity; 
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Items
        public ActionResult Index()
        {
            var user = User.Identity.GetUserId();
            var items = db.Items.Include(i => i.ItemTypes).Include(i => i.Quality); //.Include(y=>y.Seller).Where(y=>y.SellerId==user);
            
            return View(items.ToList());    
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Items items = db.Items.Find(id);
            if (items == null)
            {
                return HttpNotFound();
            }
            return View(items);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            ViewBag.ItemTypeId = new SelectList(db.ItemTypes, "ItemTypeId", "Name");
            ViewBag.QualityId = new SelectList(db.Qualities, "QualityId", "type");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemId,ItemTypeId,SellerId,Quantity,QualityId,Price")] Items items)
        {
            try
            {
                if (ModelState.IsValid)
                {                                    
                    var users = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                    items.SellerId = users.Id;                 
                    items.Seller = users;
                    db.Items.Add(items);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }catch(DataException)
            {
                ModelState.AddModelError("", "Unable to add this item");
                ViewBag.ItemTypeId = new SelectList(db.ItemTypes, "ItemTypeId", "Name", items.ItemTypeId);
                ViewBag.QualityId = new SelectList(db.Qualities, "QualityId", "type", items.QualityId);
                return View(items);
            }
 
            return View(items);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Items items = db.Items.Find(id);
            if (items == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemTypeId = new SelectList(db.ItemTypes, "ItemTypeId", "Name", items.ItemTypeId);
            ViewBag.QualityId = new SelectList(db.Qualities, "QualityId", "type", items.QualityId);
            return View(items);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemId,ItemTypeId,SellerId,Quantity,QualityId,Price")] Items items)
        {

            if (ModelState.IsValid)
            {
                var users = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                if (items.SellerId != users.Id)
                {               
                    return RedirectToAction("Index");                   
                }
                db.Entry(items).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemTypeId = new SelectList(db.ItemTypes, "ItemTypeId", "Name", items.ItemTypeId);
            ViewBag.QualityId = new SelectList(db.Qualities, "QualityId", "type", items.QualityId);
            return View(items);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Items items = db.Items.Find(id);
            if (items == null)
            {
                return HttpNotFound();
            }
            return View(items);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Items items = db.Items.Find(id);
            db.Items.Remove(items);
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
