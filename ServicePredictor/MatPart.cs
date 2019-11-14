using System;

namespace ServicePredictor
{
    public static class MatPart
    {
        public static double GaversinusMethod(double latitudeA, double latitudeB, double longitudeA, double longitudeB)
        {
            return Math.Acos(Math.Sin(latitudeA * Math.PI / 180) * Math.Sin(latitudeB * Math.PI / 180) + Math.Cos(latitudeA * Math.PI / 180) * Math.Cos(latitudeB * Math.PI / 180) * Math.Cos((longitudeA - longitudeB) * Math.PI / 180)) * 6372795;
        }
    }
}
