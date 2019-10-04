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
            var sqadCosSin = Math.Pow(Math.Cos(latitudeB) * Math.Sin(longitudeB - longitudeA), 2);
            var cosSinLat = Math.Cos(latitudeA) * Math.Sin(latitudeB);
            var sinCosCos = Math.Sin(latitudeA) * Math.Cos(latitudeB) * Math.Cos(longitudeB - longitudeA);
            var sqadSinCosCos = Math.Pow(cosSinLat - sinCosCos, 2);
            return result = 6372795 * Math.Atan(Math.Pow(sqadCosSin + sqadSinCosCos, 0.5) / (Math.Sin(latitudeA) * Math.Sin(latitudeB) + Math.Cos(latitudeA) * Math.Cos(latitudeB) * Math.Cos(longitudeB - longitudeA)));
        }
    }
}
