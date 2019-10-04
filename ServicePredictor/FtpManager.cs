using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using ServicePredictor.Models;
using System.IO;
using System.Xml;

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
            var uniqueElements = new List<FtpFileDataElement>();
            foreach (var item in ftpFileDataElements)
            {
                if(!uniqueElements.Any(f=>f.BusRouteName.Equals(item.BusRouteName) 
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

        public List<FtpFileDataElement> GetData(string fileName)
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

        public FtpDataManager(string ftpPath, string user, string password)
        {
            FtpPath = ftpPath;
            UserName = user;
            Password = password;
        }
    }
}