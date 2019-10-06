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

        private List<FtpFileDataElement> GetUniqueElements(List<FtpFileDataElement> ftpFileDataElements)
        {
            if (ftpFileDataElements == null) return null;
            var uniqueElements = new List<FtpFileDataElement>();
            foreach (var item in ftpFileDataElements)
            {
                if (!uniqueElements.Any(f => f.BusRouteName.Equals(item.BusRouteName)
                                        && f.CarNumber.Equals(item.CarNumber)
                                        && f.Azimuth.Equals(item.Azimuth)
                                        && f.Latitude.Equals(item.Latitude)
                                        && f.Longitude.Equals(item.Longitude)))
                {
                    uniqueElements.Add(item);
                }
            }
            return uniqueElements;
        }

        private List<FtpFileDataElement> GetPreData(string fileName)
        {
            StringReader reader = null;
            var ftpFileDataElements = new List<FtpFileDataElement>();
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
                        ftpFileDataElements.Add(new FtpFileDataElement(garageNum, marsh, graph, smena, timenav, latitude, longitude, speed, azimuth));
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return ftpFileDataElements;
        }

        public List<FtpFileDataElement> GetData()
        {
            var result = new List<FtpFileDataElement>();
            var targetDate = DateTime.Now
                                     .AddDays(-1)
                                     .AddHours(-1 * DateTime.Now.Hour)
                                     .AddMinutes(-1 * DateTime.Now.Minute)
                                     .AddSeconds(-1 * DateTime.Now.Second);
            int coreCount = Environment.ProcessorCount;
            int coreWeight = 1440 / coreCount;
            List<FtpFileDataElement>[] fileDataElementsArrResult = new List<FtpFileDataElement>[coreCount]; 
            for (int i = 0; i < coreCount; i++)
            {
                String k = i.ToString();
                lock (k)
                {
                    
                    Thread thread = new Thread(() => 
                    {
                        fileDataElementsArrResult[(int.TryParse(k,out var knum) ? knum : 0)] = 
                        ThreadGetData(coreWeight, targetDate.AddMinutes(coreWeight * (int.TryParse(k,out var knum2) 
                                                                                     ? knum2 
                                                                                     : 0))); });
                    thread.Start();
                }
            }
            while (fileDataElementsArrResult.ToList().Any(f => f == null)) { }
            for(int i = 0; i < coreCount; i++)
            {
                result.AddRange(fileDataElementsArrResult[i]);
            }
            return result;
        }

        public List<FtpFileDataElement>ThreadGetData(int coreWeight, DateTime targetDate)
        {
            var result = new List<FtpFileDataElement>();
            var endDate = targetDate.AddMinutes(coreWeight);
            lock (result)
            {
                while (!targetDate.Hour.Equals(endDate.Hour) && targetDate.Minute.Equals(endDate.Minute))
                {
                    var fileName = "//" + targetDate.ToString("yyyy") + "_" +
                                   targetDate.ToString("MM") +
                                   "//" + targetDate.ToString("yyyy") + "_"
                                        + targetDate.ToString("MM") + "_"
                                        + targetDate.ToString("dd") + "//"
                                        + "Otmetki_" + targetDate.ToString("yyyy") + "_"
                                        + targetDate.ToString("MM") + "_"
                                        + targetDate.ToString("dd") + "_"
                                        + targetDate.ToString("HH") + "_"
                                        + targetDate.ToString("mm") + ".xml";
                    result.AddRange(GetUniqueElements(GetPreData(fileName)) ?? new List<FtpFileDataElement>());
                    targetDate = targetDate.AddMinutes(1);
                }
                return result;
            }
        }

        public FtpDataManager(string ftpPath, string user, string password)
        {
            FtpPath = ftpPath;
            UserName = user;
            Password = password;
        }
    }
}