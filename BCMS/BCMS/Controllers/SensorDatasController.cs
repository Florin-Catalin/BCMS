using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BCMS.Models;
using BCMS.Services;

namespace BCMS.Controllers
{
    public class SensorDatasController : Controller
    {
        private BCMSDatabaseEntities db = new BCMSDatabaseEntities();
        private readonly MqttConnection mqttConnection = new MqttConnection();


        // GET: SensorDatas
        public ActionResult Index()
        {
            return View(db.SensorDatas.ToList());
        }

        // GET: SensorDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SensorData sensorData = db.SensorDatas.Find(id);
            if (sensorData == null)
            {
                return HttpNotFound();
            }
            return View(sensorData);
        }

        // GET: SensorDatas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SensorDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RPiID,Temperature,Humidity,Distance,Date")] SensorData sensorData)
        {
            if (ModelState.IsValid)
            {
                db.SensorDatas.Add(sensorData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sensorData);
        }

        // GET: SensorDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SensorData sensorData = db.SensorDatas.Find(id);
            if (sensorData == null)
            {
                return HttpNotFound();
            }
            return View(sensorData);
        }

        // POST: SensorDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RPiID,Temperature,Humidity,Distance,Date")] SensorData sensorData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sensorData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sensorData);
        }

        // GET: SensorDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SensorData sensorData = db.SensorDatas.Find(id);
            if (sensorData == null)
            {
                return HttpNotFound();
            }
            return View(sensorData);
        }

        // POST: SensorDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SensorData sensorData = db.SensorDatas.Find(id);
            db.SensorDatas.Remove(sensorData);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RunMqttConnection()
        {
            //this.mqttConnection;
            //mqttConnection = new MqttConnection();
            System.Diagnostics.Debug.WriteLine("@@@@@@@@@@@@@@@\n");
            mqttConnection.run();
            return RedirectToAction("Index");
            //return View();
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
