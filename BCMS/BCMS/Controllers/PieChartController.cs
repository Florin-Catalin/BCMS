using BCMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCMS.Controllers
{
    public class PieChartController : Controller
    {
        public ActionResult Index()
        {
            List<PieChartPoint> dataPoints = new List<PieChartPoint>();
            List<PieChartPoint> dataPoints2 = new List<PieChartPoint>();
            List<PieChartPoint> dataPoints3 = new List<PieChartPoint>();
            BCMSDatabaseEntities db = new BCMSDatabaseEntities();
            BCMSDatabaseEntities1 db1 = new BCMSDatabaseEntities1();
            
            int under20 = 0;
            int between20and23 = 0;
            int between23and26 = 0;
            int between26and29 = 0;
            int above29 = 0;
            foreach(var tempHumidity in db.SensorDatas.ToList())
            {
                if (tempHumidity.Temperature < 20)
                {
                    under20++;
                }else if(tempHumidity.Temperature >= 20 && tempHumidity.Temperature < 23)
                {
                    between20and23++;
                }else if(tempHumidity.Temperature >= 23 && tempHumidity.Temperature < 26)
                {
                    between23and26++;
                }else if(tempHumidity.Temperature >= 26 && tempHumidity.Temperature < 29){
                    between26and29++;
                }else if(tempHumidity.Temperature >= 29)
                {
                    above29++;
                }
            }
            int total = under20 + between20and23 + between23and26 + between26and29 + above29;
            dataPoints.Add(new PieChartPoint("<20°C", under20*100/total));
            dataPoints.Add(new PieChartPoint("20-23°C", between20and23*100/total));
            dataPoints.Add(new PieChartPoint("23-26°C", between23and26 * 100 / total));
            dataPoints.Add(new PieChartPoint("26-29°C", between26and29 * 100 / total));
            dataPoints.Add(new PieChartPoint(">29°C", above29 * 100 / total));

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            int under30 = 0;
            int between30and40 = 0;
            int between40and50 = 0;
            int between50and60 = 0;
            int above60 = 0;
            foreach (var tempHumidity in db.SensorDatas.ToList())
            {
                if (tempHumidity.Humidity < 30)
                {
                    under30++;
                }
                else if (tempHumidity.Humidity >= 30 && tempHumidity.Humidity < 40)
                {
                    between30and40++;
                }
                else if (tempHumidity.Humidity >= 40 && tempHumidity.Humidity < 50)
                {
                    between40and50++;
                }
                else if (tempHumidity.Humidity >= 50 && tempHumidity.Humidity < 60)
                {
                    between50and60++;
                }
                else if (tempHumidity.Humidity >= 60)
                {
                    above60++;
                }
            }

            int total2 = under30 + between30and40 + between40and50 + between50and60 + above60;

            dataPoints2.Add(new PieChartPoint("<30", under30 * 100 / total));
            dataPoints2.Add(new PieChartPoint("30-40", between30and40 * 100 / total));
            dataPoints2.Add(new PieChartPoint("40-50", between40and50 * 100 / total));
            dataPoints2.Add(new PieChartPoint("50-60", between50and60 * 100 / total));
            dataPoints2.Add(new PieChartPoint(">60", above60 * 100 / total));

            int hour_9 = 0;
            int hour_9_10 = 0;
            int hour_10_11 = 0;
            int hour_11_12 = 0;
            int hour_12_14 = 0;
            int hour_14 = 0;
            foreach (var visitor in db1.VisitorPhotoes.ToList())
            {
                if (visitor.Date.Value.Hour < 9)
                {
                    hour_9++;
                }
                else if (visitor.Date.Value.Hour >= 9 && visitor.Date.Value.Hour < 10)
                {
                    hour_9_10++;
                }
                else if (visitor.Date.Value.Hour >= 10 && visitor.Date.Value.Hour < 11)
                {
                    hour_10_11++;
                }
                else if (visitor.Date.Value.Hour >= 11 && visitor.Date.Value.Hour < 12)
                {
                    hour_11_12++;
                }
                else if (visitor.Date.Value.Hour >= 12 && visitor.Date.Value.Hour < 14)
                {
                    hour_12_14++;
                }else if (visitor.Date.Value.Hour >= 14)
                {
                    hour_14++;
                }
            }

            dataPoints3.Add(new PieChartPoint("< 9 AM", hour_9));
            dataPoints3.Add(new PieChartPoint("9 - 10 AM", hour_9_10));
            dataPoints3.Add(new PieChartPoint("10 - 11 AM", hour_10_11));
            dataPoints3.Add(new PieChartPoint("11 - 12 AM", hour_11_12));
            dataPoints3.Add(new PieChartPoint("12 - 2 PM", hour_12_14));
            dataPoints3.Add(new PieChartPoint("> 2 PM", hour_14));

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);
            ViewBag.DataPoints3 = JsonConvert.SerializeObject(dataPoints3);
            return View();
        }
    }
}