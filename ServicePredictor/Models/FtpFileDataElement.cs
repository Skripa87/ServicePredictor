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
        public TimeSpan TimePoint { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Speed { get; set; }
        public int Azimuth { get; set; }

        public FtpFileDataElement(string garagNum, string marsh, string graph, string smena, string timenav, string latitude, string longitude, string speed, string azimuth)
        {
            if()

        }
    }
}