using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ServicePredictor
{
    public class FtpDataManager
    {
        public string FtpPath { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }

        private string ReadFileToString(string fileName)
        {
            string result = "";
            var request = new WebClient();
            string url = FtpPath + fileName;
            request.Credentials = new NetworkCredential(UserName, Password);

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

        //private List<FtpFileDataElement> GetBusRoutes(string fileName)
        //{
        //    StringReader reader = null;
        //    var currentBusPoints = new List<CurrentBusPoint>();
        //    try
        //    {
        //        reader = new StringReader(ReadFileToString(fileName));
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    XmlDocument Document = new XmlDocument();
        //    try
        //    {
        //        Document.Load(reader);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    try
        //    {
        //        var items = Document.GetElementsByTagName("item");
        //        if (items != null)
        //        {
        //            foreach (var item in items)
        //            {
        //                var attributes = ((XmlNode)item).Attributes;
        //                string garageNum = "", marsh = "", graph = "", smena = "", timenav = "", latitude = "", longitude = "", speed = "", azimuth = "";
        //                foreach (XmlAttribute attr in attributes)
        //                {
        //                    switch (attr.Name)
        //                    {
        //                        case "GaragNumb": garageNum = attr.Value; break;
        //                        case "Marsh": marsh = attr.Value; break;
        //                        case "Graph": graph = attr.Value; break;
        //                        case "Smena": smena = attr.Value; break;
        //                        case "TimeNav": timenav = attr.Value; break;
        //                        case "Latitude": latitude = attr.Value; break;
        //                        case "Longitude": longitude = attr.Value; break;
        //                        case "Speed": speed = attr.Value; break;
        //                        case "Azimuth": azimuth = attr.Value; break;
        //                    }
        //                }
        //                currentBusPoints.Add(new CurrentBusPoint(garageNum, marsh, graph, smena, timenav, latitude, longitude, speed, azimuth));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    return currentBusPoints;
        //}

        //private Dictionary<string, Dictionary<string, List<MapPoint>>> CreatePreBusRoutes()
        //{
        //    var result = new Dictionary<string, Dictionary<string, List<MapPoint>>>();
        //    var targetDate = DateTime.Now
        //                             .AddDays(-1)
        //                             .AddHours(-1 * DateTime.Now.Hour)
        //                             .AddMinutes(-1 * DateTime.Now.Minute)
        //                             .AddSeconds(-1 * DateTime.Now.Second);
        //    while (!DateTime.Now.Day.Equals(targetDate.Day))
        //    {
        //        var fileName = "//" + targetDate.ToString("yyyy") + "_"
        //                            + targetDate.ToString("MM") + "_"
        //                            + targetDate.ToString("dd") + "//"
        //                            + "Otmetki_" + targetDate.ToString("yyyy") + "_"
        //                            + targetDate.ToString("MM") + "_"
        //                            + targetDate.ToString("dd") + "_"
        //                            + targetDate.ToString("HH") + "_"
        //                            + targetDate.ToString("mm") + ".xml";
        //        var
        //        targetDate.AddMinutes(1);
        //    }
        //    return result;
        //}

        public FtpDataManager(string ftpPath, string user, string password)
        {
            FtpPath = ftpPath;
            UserName = user;
            Password = password;
        }
    }
}