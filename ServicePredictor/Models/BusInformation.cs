﻿using System;
using System.Collections.Generic;

namespace ServicePredictor.Models
{
    public class BusInformation:IEquatable<BusInformation>
    {
        public string Id { get; }
        public int CarNumber { get;}
        public int Turn { get;}
        public int Sheduler { get;}
        public string RouteName { get; set; }
        public List<MapPoint> MapPoints { get; }
        
        public BusInformation(string carNumber, string turn, string sheduler,  string routeName) 
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
            RouteName = routeName;
            MapPoints = new List<MapPoint>();
        }
        
        public int SimilarityCount(BusInformation other)
        {
            var result = 0;
            var countSelfPoint = MapPoints.Count;
            var countOtherPoint = other.MapPoints
                                       .Count;
            var procentRange = countOtherPoint / 100 * 5;
            var k = (int) (countOtherPoint / countSelfPoint);
            
            for (int iterator=0;iterator<countSelfPoint;iterator++)
            {
                var startK = k*iterator - procentRange < 0
                    ? 0
                    : k*iterator - procentRange;
                var endK = k*iterator + procentRange > other.MapPoints.Count - 1
                    ? other.MapPoints.Count - 1
                    : k*iterator + procentRange; 
                for (int i = startK; i < endK; i++)
                {
                    if (MapPoints[iterator].Equals(other.MapPoints[i]))
                    {
                        result++;
                        break;
                    }
                }
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

        public bool Equals(BusInformation other)
        {
            return other != null && (other.CarNumber == CarNumber);
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