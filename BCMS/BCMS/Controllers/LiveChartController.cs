using BCMS.Models;
using BCMS.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace BCMS.Controllers
{
    public class LiveChartController : Controller
    {
        MqttConnection mqtt = new MqttConnection();
        BCMSDatabaseEntities db = new BCMSDatabaseEntities();
        // GET: LiveChart
        public ActionResult Index()
        {
            return View();
        }

        public ContentResult JSON(int xStart = 0, double yStart = 0, int length = 1)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            Random random = new Random();
            double y = yStart;

            for (int i = 0; i < length; i++)
            {
                var temp = db.SensorDatas.ToList().Last();
                y = (double)temp.Temperature;//y + random.Next(-1, 2);
                dataPoints.Add(new DataPoint(xStart + i, y));
            }

            return Content(JsonConvert.SerializeObject(dataPoints, _jsonSetting), "application/json");
        }

        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

        public ContentResult JSON2(int xStart = 0, double yStart = 0, int length = 1)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            Random random = new Random();
            double y = yStart;

            for (int i = 0; i < length; i++)
            {
                var temp = db.SensorDatas.ToList().Last();
                y = (double)temp.Humidity;//y + random.Next(-1, 2);
                dataPoints.Add(new DataPoint(xStart + i, y));
            }

            return Content(JsonConvert.SerializeObject(dataPoints, _jsonSetting), "application/json2");
        }

    }
}