using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePredictor
{
    public static class MatPart
    {
        public static double GaversinusMethod(double latitudeA, double latitudeB, double longitudeA, double longitudeB)
        {
            double result;
            var sqadCosSin = Math.Pow(Math.Cos(latitudeB * Math.PI / 180) * Math.Sin((longitudeB - longitudeA) * Math.PI / 180), 2);
            var cosSinLat = Math.Cos(latitudeA * Math.PI / 180) * Math.Sin(latitudeB * Math.PI / 180);
            var sinCosCos = Math.Sin(latitudeA * Math.PI / 180) * Math.Cos(latitudeB * Math.PI / 180) * Math.Cos((longitudeB - longitudeA) * Math.PI / 180);
            var sqadSinCosCos = Math.Pow(cosSinLat - sinCosCos, 2);
            return result = 6372795 * Math.Atan(Math.Pow(sqadCosSin + sqadSinCosCos, 0.5) / (Math.Sin(latitudeA * Math.PI / 180) * Math.Sin(latitudeB * Math.PI / 180) + Math.Cos(latitudeA * Math.PI / 180) * Math.Cos(latitudeB * Math.PI / 180) * Math.Cos((longitudeB - longitudeA) * Math.PI / 180)));
            //return Math.Acos(Math.Sin(latitudeA * Math.PI / 180) * Math.Sin(latitudeB * Math.PI / 180) + Math.Cos(latitudeA * Math.PI / 180) * Math.Cos(latitudeB * Math.PI / 180) * Math.Cos((longitudeA - longitudeB) * Math.PI / 180)) * 6372795;
        }
    }
}
