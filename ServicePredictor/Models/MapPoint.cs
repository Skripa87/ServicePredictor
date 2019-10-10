using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePredictor.Models
{
    public class MapPoint:IEquatable<MapPoint>
    {
        public string Id { get; set; }
        public DateTime TimePoint { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Azimut { get; set; }
        public int Speed { get; set; }

        public MapPoint()
        {

        }
        public bool Equals(MapPoint other)
        {
            return (other.Latitude.Equals(Latitude) && other.Longitude.Equals(Longitude))
                || MatPart.GaversinusMethod(other.Latitude, Latitude, other.Longitude, Longitude) < 3.5;
        }

        public override int GetHashCode()
        {
            var hashCode = 129010546;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + TimePoint.GetHashCode();
            hashCode = hashCode * -1521134295 + Latitude.GetHashCode();
            hashCode = hashCode * -1521134295 + Longitude.GetHashCode();
            hashCode = hashCode * -1521134295 + Azimut.GetHashCode();
            hashCode = hashCode * -1521134295 + Speed.GetHashCode();
            return hashCode;
        }
    }
}