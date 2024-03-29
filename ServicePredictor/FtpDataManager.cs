﻿using System;
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
    public class TimerFtpDataManager
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int NumberCore { get; set; }

        public TimerFtpDataManager(DateTime start, DateTime end, int numberCore)
        {
            Start = start;
            End = end;
            NumberCore = numberCore;
        }
    }

    public class FtpDataManager
    {
        public string FtpPath { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }

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

        public List<BusInformation> GetPartData(string fileName)
        {
            StringReader reader = null;
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
            var buses = new List<BusInformation>();
            try
            {
                var items = Document.GetElementsByTagName("item");
                if (items.Count > 1)
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
                        var busInformation = new BusInformation(garageNum,smena,graph,marsh);
                        if (!buses.Contains(busInformation))
                        {
                            busInformation.InsertPoint(latitude,longitude,timenav,azimuth,speed);
                            buses.Add(busInformation);
                        }
                        else
                        {
                            buses.Find(b=>string.Equals(b.CarNumber
                                                                .ToString(),garageNum))
                                 .InsertPoint(latitude,longitude,timenav,azimuth,speed);
                        }
                        //var busRoute = new BusRouteBuffer(marsh);
                        //var busCrew = new BusCrew(garageNum, smena, graph);
                        //if (busRoutes.Contains(busRoute))
                        //{
                        //    var busRouteIndex = busRoutes.IndexOf(busRoutes.Find(b => string.Equals(b.BusRouteName,marsh)));
                        //    if (busRoutes.ElementAt(busRouteIndex)
                        //                 .BusesBuffer
                        //                 .Contains(busCrew))
                        //    {
                        //        var busCrewFinder = busRoutes.ElementAt(busRouteIndex)
                        //                                     .BusesBuffer
                        //                                     .Find(b => b.Equals(busCrew));
                        //        var busCrewIndex = busRoutes.ElementAt(busRouteIndex)
                        //                                    .BusesBuffer
                        //                                    .IndexOf(busCrewFinder);
                        //        busRoutes.ElementAt(busRouteIndex)
                        //                 .BusesBuffer
                        //                 .ElementAt(busCrewIndex)
                        //                 .InsertPoint(latitude, longitude, timenav, azimuth, speed);
                        //    }
                        //    else
                        //    {
                        //        busCrew.InsertPoint(latitude, longitude, timenav, azimuth, speed);
                        //        busRoutes.ElementAt(busRouteIndex)
                        //                 .InsertBuses(busCrew);
                        //    }
                        //}
                        //else
                        //{
                        //    busCrew.InsertPoint(latitude, longitude, timenav, azimuth, speed);
                        //    busRoute.InsertBuses(busCrew);
                        //    busRoutes.Add(busRoute);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return buses;
        }

        public List<BusRoute> GetData()
        {
            var busesInformation = new List<BusInformation>();
            var targetDate = DateTime.Now
                                     .AddDays(-1)
                                     .AddHours(-1 * DateTime.Now.Hour)
                                     .AddMinutes(-1 * DateTime.Now.Minute)
                                     .AddSeconds(-1 * DateTime.Now.Second);
            var current = targetDate.AddHours(2);
            var end = targetDate.AddDays(1)
                                .AddHours(0);
            while (!current.Equals(end))
            {
                var fileName = "//" + current.ToString("yyyy") + "_" +
                               current.ToString("MM") +
                               "//" + current.ToString("yyyy") + "_"
                                    + current.ToString("MM") + "_"
                                    + current.ToString("dd") + "//"
                                    + "Otmetki_" + current.ToString("yyyy") + "_"
                                    + current.ToString("MM") + "_"
                                    + current.ToString("dd") + "_"
                                    + current.ToString("HH") + "_"
                                    + current.ToString("mm") + ".xml";
                var preResult = GetPartData(fileName);
                busesInformation = BusRouteManager.AttachBusRoutes(busesInformation, preResult);
                current = current.AddMinutes(1);
             }
            return BusRouteManager.CreateValidBusRoutes(busesInformation);
        }

        public FtpDataManager(string ftpPath, string user, string password)
        {
            FtpPath = ftpPath;
            UserName = user;
            Password = password;
        }
    }
}