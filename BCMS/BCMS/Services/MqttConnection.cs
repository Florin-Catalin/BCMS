using BCMS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace BCMS.Services
{
    public class MqttConnection
    {   //toDo connect this with the logics that decides if the mac of the visitor is unknown
        bool visitorStatus = true;

        private BCMSDatabaseEntities db = new BCMSDatabaseEntities();
        private BCMSDatabaseEntities1 db1 = new BCMSDatabaseEntities1();
        private BCMSDatabaseEntities2 db2 = new BCMSDatabaseEntities2();

        MqttClient client = new uPLibrary.Networking.M2Mqtt.MqttClient("192.168.43.130", 1883, false, null, null, 0, null, null);
        MqttClient client2 = new uPLibrary.Networking.M2Mqtt.MqttClient("192.168.43.130", 1883, false, null, null, 0, null, null);
        MqttClient client3 = new uPLibrary.Networking.M2Mqtt.MqttClient("192.168.43.130", 1883, false, null, null, 0, null, null);

        MqttClient rgb_client = new uPLibrary.Networking.M2Mqtt.MqttClient("192.168.43.130", 1883, false, null, null, 0, null, null);

        Converter converter = new Converter();
        public readonly SensorData sensor = new SensorData();
        public bool? getVisitorStatus()
        {
            return visitorStatus;
        }
        public double? getTemperature()
        {
            return sensor.Temperature;
        }

        int connected;
        public MqttConnection()
        {
            connected = 1;
        }

        public void disableConnection()
        {
            this.connected = 0;
        }

        public void run()
        {
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            client2.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            client3.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            rgb_client.MqttMsgPublished += client_MqttMsgPublished;

            client.Connect(Guid.NewGuid().ToString());
            client2.Connect(Guid.NewGuid().ToString());
            client3.Connect(Guid.NewGuid().ToString());

            rgb_client.Connect(Guid.NewGuid().ToString());

            string[] sensors = new string[] { "sensors5" };
            string[] sensors2 = new string[] { "sensors6" };
            string[] sensors3 = new string[] { "sensors7" };
            string[] rgb = new string[] { "rgb_led" };

            ushort msgId = client.Subscribe(sensors,
            new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.ProtocolVersion = MqttProtocolVersion.Version_3_1_1;

            ushort msgId2 = client2.Subscribe(sensors2, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client2.ProtocolVersion = MqttProtocolVersion.Version_3_1_1;

            ushort msgId3 = client3.Subscribe(sensors3,
            new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client3.ProtocolVersion = MqttProtocolVersion.Version_3_1_1;
            rgb_client.ProtocolVersion = MqttProtocolVersion.Version_3_1_1;

        }

        private void client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            Debug.WriteLine("MessageId = " + e.MessageId + " Published = " + e.IsPublished);

        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine(Encoding.UTF8.GetString(e.Message));
            //Temp,Humidity
            String s = Convert.ToString(Encoding.UTF8.GetString(e.Message));
            try
            {
                if (Encoding.UTF8.GetString(e.Message).Length > 7 && Encoding.UTF8.GetString(e.Message).Length < 100)
                {
                    string[] values = Encoding.UTF8.GetString(e.Message).Split(',');

                    sensor.RPiID = int.Parse(values[2]);
                    sensor.Date = DateTime.Now;
                    sensor.Temperature = float.Parse(values[0], CultureInfo.InvariantCulture.NumberFormat);
                    sensor.Humidity = float.Parse(values[1], CultureInfo.InvariantCulture.NumberFormat);
                    db.SensorDatas.Add(sensor);
                    db.SaveChanges();

                }
                else if (Encoding.UTF8.GetString(e.Message).Length <= 7)
                {
                    System.Diagnostics.Debug.WriteLine("Distance: " + Encoding.UTF8.GetString(e.Message));
                }
                else
                {
                    string[] values = Encoding.UTF8.GetString(e.Message).Split('@');
                    converter.convert(values[0]);
                    VisitorPhoto visitorPhoto = new VisitorPhoto();
                    string mac = values[1];
                    VisitorData visitorData = db2.VisitorDatas.SingleOrDefault(visitordata => visitordata.Mac == mac);
                    if (visitorData != null)
                    {
                        visitorPhoto.Mac = values[1];
                    }
                    else
                    {
                        visitorPhoto.Mac = "Unknown";
                    }
                    //Array.Copy(e.Message, x, values[0].Length);

                    byte[] x = { };

                    System.Diagnostics.Debug.WriteLine(e.Message[0]);
                    System.Diagnostics.Debug.WriteLine(e.Message[1]);
                    System.Diagnostics.Debug.WriteLine(e.Message[2]);
                    System.Diagnostics.Debug.WriteLine(e.Message[3]);

                    var sourceStartIndex = 0;
                    var destinationLength = values[0].Length - 10;
                    var destination = new byte[destinationLength];

                    Array.Copy(e.Message, sourceStartIndex, destination, 0, destinationLength);

                    visitorPhoto.Photo = destination;

                    visitorPhoto.Date = DateTime.Now;
                    System.Diagnostics.Debug.WriteLine(x);
                    db1.VisitorPhotoes.Add(visitorPhoto);
                    db1.SaveChanges();
                    System.Diagnostics.Debug.WriteLine("Image converted");

                    if (visitorPhoto.Mac != "Unknown")
                    {
                        ushort rgb_msgId = rgb_client.Publish("/rgb_led",
                            Encoding.UTF8.GetBytes("true"),
                            MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,//QoS level
                            true); //retained
                    }
                    else
                    {
                        ushort rgb_msgId = rgb_client.Publish("/rgb_led",
                            Encoding.UTF8.GetBytes("false"),
                            MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,//QoS level
                            true); //retained

                    }
                }


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Connection closed\n");
                System.Diagnostics.Debug.WriteLine(ex);
                client.MqttMsgPublishReceived -= client_MqttMsgPublishReceived;
                client.Unsubscribe(new string[] { "sensors5" });
                client.Disconnect();

            }

        }

    }

}