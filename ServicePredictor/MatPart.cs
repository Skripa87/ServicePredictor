using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePredictor
{
    class point
    {
        public double pointX { get; }
        public double pointY { get; }

        public point(double x, double y)
        {
            pointX = x;
            pointY = y;
        }
    }
    public static class MatPart
    {
        private static double _eccentricitySquad = 0.00669342162296594;
        private static double _bigcircleradius = 6378245;
        private static double _altitude = 67;

        private static point getPoint(double latitude, double longitude)
        {
            var NH = ((6378245 / Math.Pow(1 - _eccentricitySquad * Math.Pow(Math.Sin(latitude * Math.PI / 180), 2.0),
                           0.5)) + _altitude);
            var cosA = Math.Cos(latitude * Math.PI / 180);
            var xPoint = NH * cosA * Math.Cos(longitude * Math.PI / 180);
            var yPoint = NH * cosA * Math.Sin(longitude * Math.PI / 180);
            return new point(xPoint, yPoint);
        }
        public static double GaversinusMethod(double latitudeA, double latitudeB, double longitudeA, double longitudeB)
        {
            var firstPoint = getPoint(latitudeA, longitudeA);
            var secondPoint = getPoint(latitudeB, longitudeB);
            return Math.Pow(
                Math.Pow(secondPoint.pointX - firstPoint.pointX, 2.0) +
                Math.Pow(secondPoint.pointY - firstPoint.pointY, 2.0), 0.5);
            //double result;
            //var sqadCosSin = Math.Pow(Math.Cos(latitudeB * Math.PI / 180) * Math.Sin((longitudeB - longitudeA) * Math.PI / 180), 2);
            //var cosSinLat = Math.Cos(latitudeA * Math.PI / 180) * Math.Sin(latitudeB * Math.PI / 180);
            //var sinCosCos = Math.Sin(latitudeA * Math.PI / 180) * Math.Cos(latitudeB * Math.PI / 180) * Math.Cos((longitudeB - longitudeA) * Math.PI / 180);
            //var sqadSinCosCos = Math.Pow(cosSinLat - sinCosCos, 2);
            //return result = 6372795 * Math.Atan(Math.Pow(sqadCosSin + sqadSinCosCos, 0.5) / (Math.Sin(latitudeA * Math.PI / 180) * Math.Sin(latitudeB * Math.PI / 180) + Math.Cos(latitudeA * Math.PI / 180) * Math.Cos(latitudeB * Math.PI / 180) * Math.Cos((longitudeB - longitudeA) * Math.PI / 180)));
            //return Math.Acos(Math.Sin(latitudeA * Math.PI / 180) * Math.Sin(latitudeB * Math.PI / 180) + Math.Cos(latitudeA * Math.PI / 180) * Math.Cos(latitudeB * Math.PI / 180) * Math.Cos((longitudeA - longitudeB) * Math.PI / 180)) * 6372795;
        }
    }
}
