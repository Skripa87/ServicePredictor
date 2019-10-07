using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using ServicePredictor.Models;
using System.IO;
using System.Xml;
using System.Threading;

namespace ServicePredictor
{
    public class FtpDataManager
    {
        public string FtpPath { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }
        private static List<BusRouteBuffer>[] BusRoutesBuffer { get; set; }
        private string ReadFileToString(string fileName)
        {
            var request = new WebClient();
            string url = FtpPath + fileName;
            request.Credentials = new NetworkCredential(UserName, Password);

            string result;
            try
            {
                byte[] newFileData = request.DownloadData(url);
                result = System.Text.Encoding.UTF8.GetString(newFileData);
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        private List<BusRouteBuffer> GetPartData(string fileName)
        {
            var buses = new List<BusCrew>();
            var busRoutes = new List<BusRouteBuffer>();
            StringReader reader;
            try
            {
                reader = new StringReader(ReadFileToString(fileName));
            }
            catch (Exception ex)
            {
                return null;
            }
            XmlDocument Document = new XmlDocument();
            try
            {
                Document.Load(reader);
            }
            catch (Exception ex)
            {
                return null;
            }
            try
            {
                var items = Document.GetElementsByTagName("item");
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        var attributes = ((XmlNode)item).Attributes;
                        string garageNum = "", marsh = "", graph = "", smena = "", timenav = "", latitude = "", longitude = "", speed = "", azimuth = "";
                        foreach (XmlAttribute attr in attributes)
                        {
                            switch (attr.Name)
                            {
                                case "GaragNumb": garageNum = attr.Value; break;
                                case "Marsh": marsh = attr.Value; break;
                                case "Graph": graph = attr.Value; break;
                                case "Smena": smena = attr.Value; break;
                                case "TimeNav": timenav = attr.Value; break;
                                case "Latitude": latitude = attr.Value; break;
                                case "Longitude": longitude = attr.Value; break;
                                case "Speed": speed = attr.Value; break;
                                case "Azimuth": azimuth = attr.Value; break;
                            }
                        }
                        var busRoute = new BusRouteBuffer(marsh);
                        var busCrew = new BusCrew(garageNum, smena, graph);
                        if (busRoutes.Contains(busRoute))
                        {
                            var busRouteIndex = busRoutes.IndexOf(busRoutes.Find(b => b.BusRouteName.Equals(marsh)));
                            if (busRoutes.ElementAt(busRouteIndex)
                                         .BusesBuffer
                                         .Contains(busCrew))
                            {
                                var busCrewIndex = busRoutes.ElementAt(busRouteIndex)
                                                            .BusesBuffer.IndexOf(busRoutes.ElementAt(busRouteIndex)
                                                            .BusesBuffer
                                                            .Find(f => f.CarNumber.Equals(garageNum) &&
                                                                       f.Sheduler.Equals(graph) &&
                                                                       f.Turn.Equals(smena)));
                                busRoutes.ElementAt(busRouteIndex)
                                         .BusesBuffer
                                         .ElementAt(busCrewIndex)
                                         .InsertPoint(latitude, longitude, timenav, azimuth, speed);
                            }
                            else
                            {
                                busCrew.InsertPoint(latitude, longitude, timenav, azimuth, speed);
                                busRoutes.ElementAt(busRouteIndex)
                                         .InsertBuses(busCrew);
                            }
                        }
                        else
                        {
                            busCrew.InsertPoint(latitude, longitude, timenav, azimuth, speed);
                            busRoute.InsertBuses(busCrew);
                            busRoutes.Add(busRoute);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return busRoutes;
        }

        public List<BusRouteBuffer> GetData()
        {
            var result = new List<BusRouteBuffer>();
            var targetDate = DateTime.Now
                                     .AddDays(-1)
                                     .AddHours(-1 * DateTime.Now.Hour)
                                     .AddMinutes(-1 * DateTime.Now.Minute)
                                     .AddSeconds(-1 * DateTime.Now.Second);
            int coreCount = Environment.ProcessorCount;
            int coreWeight = 1440 / coreCount;
            var threads = new Thread[coreCount];
            BusRoutesBuffer = new List<BusRouteBuffer>[coreCount];
            for (int i = 0; i < coreCount; i++)
            {
                threads[i] = new Thread(() => {BusRoutesBuffer[i] = ThreadGetData() });
                threads[i].Start();
                threads[i].Join();
            }
            return result;
        }

        public List<BusRouteBuffer> ThreadGetData(DateTime currentDate, DateTime endDate)
        {
            var result = new List<BusRouteBuffer>();
            while (!currentDate.Hour.Equals(endDate.Hour) && currentDate.Minute.Equals(endDate.Minute))
            {
                var fileName = "//" + currentDate.ToString("yyyy") + "_" +
                               currentDate.ToString("MM") +
                               "//" + currentDate.ToString("yyyy") + "_"
                                    + currentDate.ToString("MM") + "_"
                                    + currentDate.ToString("dd") + "//"
                                    + "Otmetki_" + currentDate.ToString("yyyy") + "_"
                                    + currentDate.ToString("MM") + "_"
                                    + currentDate.ToString("dd") + "_"
                                    + currentDate.ToString("HH") + "_"
                                    + currentDate.ToString("mm") + ".xml";
                targetDate = targetDate.AddMinutes(1);
            }
            return result;
        }

        public FtpDataManager(string ftpPath, string user, string password)
        {
            FtpPath = ftpPath;
            UserName = user;
            Password = password;
        }
    }
}