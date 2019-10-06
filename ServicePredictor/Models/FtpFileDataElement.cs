using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePredictor.Models
{
    public class FtpFileDataElement
    {
        public int CarNumber { get; set; }
        public string BusRouteName { get; set; }
        public int WorkGraph { get; set; }
        public int WorkSmena { get; set; }
        public DateTime TimePoint { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Speed { get; set; }
        public int Azimuth { get; set; }

        public FtpFileDataElement(string garagNum, string marsh, string graph, string smena, string timenav, string latitude, string longitude, string speed, string azimuth)
        {
            CarNumber = int.TryParse(garagNum, out var garagNumNum)
                      ? garagNumNum
                      : 0;
            BusRouteName = marsh;
            WorkGraph = int.TryParse(graph, out var graphNum)
                      ? graphNum
                      : 0;
            WorkSmena = int.TryParse(smena, out var numSmena) 
                      ? numSmena
                      : 0;
            TimePoint = DateTime.TryParse(timenav,out var dateTime)
                      ? dateTime
                      : DateTime.MinValue;
            Latitude = double.TryParse(latitude, out var latitudeNum)
                     ? latitudeNum
                     : (double.TryParse(latitude.Replace(',', '.'), out var result)
                       ? result
                       : 0);
            Longitude = double.TryParse(longitude, out var longitudeNum)
                     ? longitudeNum
                     : (double.TryParse(latitude.Replace(',', '.'), out var longitudeN)
                       ? longitudeN
                       : 0);
            Speed = double.TryParse(speed, out var speedNum)
                  ? speedNum
                  : (double.TryParse(speed.Replace(',','.'), out var speedN)
                    ? speedN
                    : 0);
            Azimuth = int.TryParse(azimuth,out var azimuthNum)
                    ? azimuthNum
                    : 0;            
        }
    }
}