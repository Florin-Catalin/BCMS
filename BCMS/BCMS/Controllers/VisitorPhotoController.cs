using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BCMS.Models;

namespace BCMS.Controllers
{
    public class VisitorPhotoController : Controller
    {
        private BCMSDatabaseEntities1 db = new BCMSDatabaseEntities1();
        private BCMSDatabaseEntities2 db2 = new BCMSDatabaseEntities2();
        // GET: VisitorPhoto
        public ActionResult Index()
        {
            var imageData = db.VisitorPhotoes.ToList();
            int i = 1;
            //ViewBag.Base64String = imageData;
            //var[] Base64String;
            foreach (var item in imageData)
            {
                ViewData[i.ToString()] = "data:image/jpeg;base64," + Encoding.UTF8.GetString(item.Photo);
                i++;
                try
                {
                    ViewData[item.Mac] = db2.VisitorDatas.ToList().First(item1 => item1.Mac == item.Mac).Name;
                }
                catch
                {
                    ViewData[item.Mac] = "Unknown";
                }
            }
                ViewData["lastPerson"] = "data:image/jpeg;base64," + Encoding.UTF8.GetString(imageData.LastOrDefault().Photo);
                if (imageData.LastOrDefault().Mac.Equals("Unknown")) // TODO: unlocked+green in if
                {
                    ViewData["necunoscut"] = "Locked";
                    ViewData["culoare"] = "red";
                }
                else
                {
                    ViewData["necunoscut"] = "Unlocked";
                    ViewData["culoare"] = "green";
                }
            
            //System.Diagnostics.Debug.WriteLine(Encoding.UTF8.GetString(imageData.Photo));
            ViewBag.visitorsName = db2.VisitorDatas.ToList();
            return View(db.VisitorPhotoes.ToList());
        }

        // GET: VisitorPhoto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisitorPhoto visitorPhoto = db.VisitorPhotoes.Find(id);
            ViewBag.Base64String = "data:image/jpeg;base64," + Encoding.UTF8.GetString(visitorPhoto.Photo);
            if (visitorPhoto == null)
            {
                return HttpNotFound();
            }
            return View(visitorPhoto);
        }

        // GET: VisitorPhoto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VisitorPhoto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Mac,Photo,Date")] VisitorPhoto visitorPhoto)
        {
            if (ModelState.IsValid)
            {
                db.VisitorPhotoes.Add(visitorPhoto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(visitorPhoto);
        }

        // GET: VisitorPhoto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisitorPhoto visitorPhoto = db.VisitorPhotoes.Find(id);
            if (visitorPhoto == null)
            {
                return HttpNotFound();
            }
            return View(visitorPhoto);
        }

        // POST: VisitorPhoto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Mac,Photo,Date")] VisitorPhoto visitorPhoto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(visitorPhoto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(visitorPhoto);
        }

        // GET: VisitorPhoto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisitorPhoto visitorPhoto = db.VisitorPhotoes.Find(id);
            if (visitorPhoto == null)
            {
                return HttpNotFound();
            }
            return View(visitorPhoto);
        }

        // POST: VisitorPhoto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VisitorPhoto visitorPhoto = db.VisitorPhotoes.Find(id);
            db.VisitorPhotoes.Remove(visitorPhoto);
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

        public ActionResult Show(int id)
        {
            var imageData = db.VisitorPhotoes.Find(id);
            //ViewBag.Base64String = "data:image/jpg;base64," + Convert.ToBase64String(imageData.Photo, 0, imageData.Photo.Length);

            return File(imageData.Photo, "image/jpg");
        }
    }
}
