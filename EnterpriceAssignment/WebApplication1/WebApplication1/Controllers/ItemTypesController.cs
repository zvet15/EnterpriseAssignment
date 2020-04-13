using Google.Apis.Drive.v3;
using PagedList;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ItemTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ItemTypes
        public ActionResult Index(int? page)
        {
            var itemTypes = db.ItemTypes.Include(i => i.Categories);
            return View(itemTypes.ToList().ToPagedList(page?? 1,5));
        }

        // GET: ItemTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTypes itemTypes = db.ItemTypes.Find(id);
            if (itemTypes == null)
            {
                return HttpNotFound();
            }
            return View(itemTypes);
        }

        // GET: ItemTypes/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: ItemTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemTypeId,Name,Image,CategoryId")] ItemTypes itemTypes, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                //jpg and png only accepted

                byte[] bytesRead = new byte[8];
                file.InputStream.Read(bytesRead, 0, 8);

                if (file != null && file.ContentLength > 0 && (bytesRead[0] == 255 && bytesRead[1] == 216 )|| (bytesRead[0] == 137 && bytesRead[1] == 80 && bytesRead[2] == 78 && bytesRead[3] == 71 && bytesRead[4] == 13 && bytesRead[5] == 10 && bytesRead[6] == 26 && bytesRead[7] == 10 ))
                {
                    //create service
                   var service= GoogleDriveAPIHelper.GetService();
                    var FileMetaData = new Google.Apis.Drive.v3.Data.File();
                    FileMetaData.Name = Path.GetFileName(file.FileName);
                    FileMetaData.MimeType = file.ContentType;
                    FilesResource.CreateMediaUpload request;
                    using (var stream = file.InputStream)
                    {
                        request = service.Files.Create(FileMetaData, stream, FileMetaData.MimeType);
                        request.Fields = "id";
                        // var fileC = request.ResponseBody;   
                        request.Upload();
                    }                 
                    var filei = request.ResponseBody;
                    itemTypes.Image= "https://drive.google.com/uc?id="+filei.Id;
                    db.ItemTypes.Add(itemTypes);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
                ModelState.AddModelError("", "Invalid image");

            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", itemTypes.CategoryId);
            return View(itemTypes);
        }

        // GET: ItemTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTypes itemTypes = db.ItemTypes.Find(id);
            if (itemTypes == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", itemTypes.CategoryId);
            return View(itemTypes);
        }

        // POST: ItemTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemTypeId,Name,Image,CategoryId")] ItemTypes itemTypes, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                //jpg and png only accepted

                byte[] bytesRead = new byte[8];
                file.InputStream.Read(bytesRead, 0, 8);

                if (file != null && file.ContentLength > 0 && (bytesRead[0] == 255 && bytesRead[1] == 216) || (bytesRead[0] == 137 && bytesRead[1] == 80 && bytesRead[2] == 78 && bytesRead[3] == 71 && bytesRead[4] == 13 && bytesRead[5] == 10 && bytesRead[6] == 26 && bytesRead[7] == 10))
                {
                    //create service
                    var service = GoogleDriveAPIHelper.GetService();
                    var FileMetaData = new Google.Apis.Drive.v3.Data.File();
                    FileMetaData.Name = Path.GetFileName(file.FileName);
                    FileMetaData.MimeType = file.ContentType;
                    FilesResource.CreateMediaUpload request;
                    using (var stream = file.InputStream)
                    {
                        request = service.Files.Create(FileMetaData, stream, FileMetaData.MimeType);
                        request.Fields = "id";
                        // var fileC = request.ResponseBody;   
                        request.Upload();
                    }
                    var filei = request.ResponseBody;
                    itemTypes.Image = "https://drive.google.com/uc?id=" + filei.Id;
                    db.Entry(itemTypes).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Invalid image");

            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", itemTypes.CategoryId);
            return View(itemTypes);
        }

        // GET: ItemTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTypes itemTypes = db.ItemTypes.Find(id);
            if (itemTypes == null)
            {
                return HttpNotFound();
            }
            return View(itemTypes);
        }

        // POST: ItemTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemTypes itemTypes = db.ItemTypes.Find(id);
            db.ItemTypes.Remove(itemTypes);
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
