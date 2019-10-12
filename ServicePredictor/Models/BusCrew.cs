using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePredictor.Models
{
    public class BusCrew:IEquatable<BusCrew>
    {
        public string Id { get; }
        public int CarNumber { get;}
        public int Turn { get;}
        public int Sheduler { get;}
        public List<MapPoint> MapPoints { get; set; }
        
        public BusCrew(string carNumber, string turn, string sheduler) 
        {
            Id = Guid.NewGuid()
                     .ToString();
            CarNumber = int.TryParse(carNumber, out var carNumberInt)
                      ? carNumberInt 
                      :-1;
            Turn = int.TryParse(turn, out var turnNumber)
                 ? turnNumber
                 :-1;
            Sheduler = int.TryParse(sheduler, out var shedulerNum)
                     ? shedulerNum
                     : -1;
            MapPoints = new List<MapPoint>();
        }
        
        public int SimilarityCount(BusCrew other)
        {
            var result = 0;
            MapPoints.Sort();
            var arrMapPoint = MapPoints.ToArray();
            other.MapPoints
                 .Sort();
            var otherArrMapPoints = other.MapPoints
                                         .ToArray();
            var count = MapPoints.Count;
            for(int i = 0; i < count; i++)
            {
                result += (MatPart.GaversinusMethod(otherArrMapPoints[i].Latitude, arrMapPoint[i].Latitude, otherArrMapPoints[i].Longitude, arrMapPoint[i].Longitude) <= 3.5
                          ? 1
                          : 0); 
            }
            return result;
        }

        public void InsertPoint(string latitude, string longitude, string date, string azimuth, string speed) 
        {
            var newMapPoint = new MapPoint()
            {
                Azimut = (int.TryParse(azimuth, out var azimuthInt)
                       ? azimuthInt
                       : 0),
                Id = (Guid.NewGuid()
                          .ToString()),
                Latitude = (double.TryParse(latitude, out var latitudeDouble)
                         ? latitudeDouble
                         : (double.TryParse(latitude.Replace(',', '.'), out var latitudeDoubleAfterReplace)
                           ? latitudeDoubleAfterReplace
                           : -1)),
                Longitude = (double.TryParse(longitude, out var longitudeDouble)
                          ? longitudeDouble
                          : (double.TryParse(longitude.Replace(',', '.'), out var longitudeDoubleAfterReplace)
                           ? longitudeDoubleAfterReplace
                           : -1)),
                Speed = (int.TryParse(speed, out var speedInt)
                      ? speedInt
                      : -1),
                TimePoint = (DateTime.TryParse(date, out var dateFormat)
                          ? dateFormat
                          : DateTime.Now)
            };
            if (MapPoints.Contains(newMapPoint)) return;
            MapPoints.Add(newMapPoint);
        }

        public void InsertPoint(MapPoint mapPoint) 
        {
            if (MapPoints.Contains(mapPoint)) return;
            MapPoints.Add(mapPoint);
        }

        public void InsertPoints(List<MapPoint> mapPoints)
        {
            foreach (var item in mapPoints)
            {
                if (!MapPoints.Contains(item)) MapPoints.Add(item);
            }
        }

        public bool Equals(BusCrew other)
        {
            return other.CarNumber.Equals(CarNumber) && other.Sheduler.Equals(Sheduler) && other.Turn.Equals(Turn);
        }

        public override int GetHashCode()
        {
            var hashCode = -1232783287;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + CarNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + Turn.GetHashCode();
            hashCode = hashCode * -1521134295 + Sheduler.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<MapPoint>>.Default.GetHashCode(MapPoints);
            return hashCode;
        }
    }
}