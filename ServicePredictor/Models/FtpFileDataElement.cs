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
            if (int.TryParse(garagNum,out var garagNumNum))
            {
                CarNumber = garagNumNum;
            }
            else
            {
                CarNumber = 0;
            }
            BusRouteName = marsh;
            if (int.TryParse(graph, out var graphNum))
            {
                WorkGraph = graphNum;
            }
            else
            {
                WorkGraph = 0;
            }
            if(int.TryParse(smena, out var numSmena))
            {
                WorkSmena = numSmena;
            }
            else
            {
                WorkSmena = 0;
            }
            if(DateTime.TryParse(timenav,out var dateTime))
            {
                TimePoint = dateTime;
            }
            else
            {
                TimePoint = DateTime.MinValue;
            }
            if(double.TryParse(latitude,out var latitudeNum))
            {
                Latitude = latitudeNum;
            }
            else
            {
                Latitude = 0;
            }
            if(double.TryParse(longitude, out var longitudeNum))
            {
                Longitude = longitudeNum;
            }
            else
            {
                Longitude = 0;
            }
            if (double.TryParse(speed, out var speedNum))
            {
                Speed = speedNum;
            }
            else
            {
                Speed = 0;
            }
            if(int.TryParse(azimuth,out var azimuthNum))
            {
                Azimuth = azimuthNum;
            }
            else
            {
                Azimuth = 0;
            }
        }
    }
}