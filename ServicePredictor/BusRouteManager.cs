using ServicePredictor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace ServicePredictor
{
    public static class BusRouteManager
    {
        public static List<BusRouteBuffer> AttachBusRoutes(List<BusRouteBuffer> busRoutsFirst, List<BusRouteBuffer> busRoutsSecond)
        {
            if (busRoutsFirst == null && busRoutsSecond != null) return busRoutsSecond;
            if (busRoutsFirst != null && busRoutsSecond == null) return busRoutsFirst;
            if (busRoutsFirst == null) return new List<BusRouteBuffer>();
            foreach (var item in busRoutsSecond)
            {
                if (!busRoutsFirst.Contains(item))
                {
                    busRoutsFirst.Add(item);
                }
                else
                {
                    var busRouteFinder = busRoutsFirst.Find(f => string.Equals(f.BusRouteName, item.BusRouteName));
                    var busRouteFinderIndex = busRoutsFirst.IndexOf(busRouteFinder);
                    foreach (var crew in item.BusesBuffer)
                    {
                        if (!busRoutsFirst.ElementAt(busRouteFinderIndex)
                                                .BusesBuffer
                                                .Contains(crew))
                        {
                            busRoutsFirst.ElementAt(busRouteFinderIndex)
                                                .BusesBuffer
                                                .Add(crew);
                        }
                        else
                        {
                            var crewFinderIndex = busRoutsFirst.ElementAt(busRouteFinderIndex)
                                                                      .BusesBuffer
                                                                      .IndexOf(crew);
                            busRoutsFirst.ElementAt(busRouteFinderIndex)
                                                .BusesBuffer
                                                .ElementAt(crewFinderIndex)
                                                .InsertPoints(crew.MapPoints);
                        }
                    }
                }
            }
            return busRoutsFirst;
        }

        public static List<BusRoute> CreateValidBusRoutes(List<BusRouteBuffer> busRoutsBuffer)
        {
            var result = new List<BusRoute>();
            var dataBaseWorker = new DataBaseWorker();
            var stations = dataBaseWorker.GetStations();
            foreach (var item in busRoutsBuffer)
            {
                var sum = 0;
                BusCrew selected = null;
                foreach (var bus in item.BusesBuffer)
                {
                    var correctSum = item.BusesBuffer
                                         .Select(s => bus.SimilarityCount(s))
                                         .Sum();
                    if (correctSum <= sum) continue;
                    selected = bus;
                    sum = correctSum;
                }
                var busRoute = new BusRoute()
                {
                    Id = Guid.NewGuid()
                             .ToString(),
                    Name = item.BusRouteName,
                    Active = true
                };
                busRoute.MapPoints
                        .ToList()
                        .AddRange(selected?.MapPoints ?? new List<MapPoint>());
                result.Add(busRoute);
            }
            dataBaseWorker.SaveBusRoute(result);
            return result;
        }

        private static int GetIndexPointInBusRoute(BusRoute busRoute, MapPoint point)
        {
            int result = -1;
            var points = busRoute.MapPoints
                                 .ToList();
            var length = MatPart.GaversinusMethod(points.First()
                                                                .Latitude,
                                                  point.Latitude,
                                                 points.First()
                                                                .Longitude,
                                                 point.Longitude);
            foreach (var item in points)
            {
                var currentLength = MatPart.GaversinusMethod(point.Latitude, point.Latitude, point.Longitude, point.Longitude);
                if (!(currentLength < length)) continue;
                length = currentLength;
                result = points.IndexOf(point);
            }
            return length > 50 ? -1 : result;
        }

        private static double GetTimeOfPredict(int indexFirst, int indexLast, List<MapPoint> mapPoints)
        {
            double resultLength = 0;
            double speedSummary = 0;
            var arrPoints = mapPoints.ToArray();
            for (int i = indexFirst; i < indexLast; i++)
            {
                resultLength += MatPart.GaversinusMethod(arrPoints[i].Latitude, arrPoints[i + 1].Latitude,
                                                        arrPoints[i].Longitude, arrPoints[i + 1].Longitude);
                speedSummary += arrPoints[i].Speed;
            }
            var speed = speedSummary / Math.Abs(indexLast - indexFirst);
            speed = speed * 1000 / 3600;
            return resultLength / speed;
        }

        public static Dictionary<string, double> GetAllPredict(FtpDataManager manager, string stationId)
        {
            var result = new Dictionary<string,double>();
            var dataBaseWorker = new DataBaseWorker();
            var station = dataBaseWorker.GetStation(stationId);
            var stationPoint = new MapPoint()
            {
                Latitude = station.Lat,
                Longitude = station.Lng,
                Speed = 0,
                TimePoint = DateTime.Now,
                Id = Guid.NewGuid()
                         .ToString(),
                Azimut = 0
            };
            var busRoutes = dataBaseWorker.GetBusRouts();
            var current = DateTime.Now.AddMinutes(-1);
            var fileName = "//" + "Otmetki_" + current.ToString("yyyy") + "_"
                                + current.ToString("MM") + "_"
                                + current.ToString("dd") + "_"
                                + current.ToString("HH") + "_"
                                + current.ToString("mm") + ".xml";
            var baseRouteData = manager.GetPartData(fileName);
            foreach (var item in baseRouteData)
            {
                var currentBusRoute = busRoutes.Find(b => string.Equals(b.Name, item.BusRouteName));
                int pointStationInBusRoute = GetIndexPointInBusRoute(currentBusRoute,stationPoint);
                if (pointStationInBusRoute == -1) continue;
                
                foreach (var busCrew in item.BusesBuffer)
                {
                    busCrew.MapPoints
                           .Sort();
                    var index = GetIndexPointInBusRoute(currentBusRoute, busCrew.MapPoints
                                                                                     .Last());
                    result.Add(item.BusRouteName + busCrew.CarNumber,GetTimeOfPredict(index,pointStationInBusRoute,currentBusRoute.MapPoints
                                                                                                                                                  .ToList()));
                }
            }
            return result;
        }
    }
}