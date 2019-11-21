using ServicePredictor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace ServicePredictor
{
    public static class BusRouteManager
    {
        public static List<BusInformation> AttachBusRoutes(List<BusInformation> busInformationFirst, List<BusInformation> busInformationSecond)
        {
            if (busInformationFirst == null && busInformationSecond != null) return busInformationSecond;
            if (busInformationFirst != null && busInformationSecond == null) return busInformationFirst;
            if (busInformationFirst == null) return new List<BusInformation>();
            foreach (var item in busInformationSecond)
            {
                if (!busInformationFirst.Contains(item))
                {
                    busInformationFirst.Add(item);
                }
                else
                {
                    busInformationFirst.Find(b=>b.CarNumber == item.CarNumber)
                                       .InsertPoints(item.MapPoints);

                    
                    
                    //var busRouteFinder = busRoutsFirst.Find(f => string.Equals(f.BusRouteName, item.BusRouteName));
                    //var busRouteFinderIndex = busRoutsFirst.IndexOf(busRouteFinder);
                    //foreach (var crew in item.BusesBuffer)
                    //{
                    //    if (!busRoutsFirst.ElementAt(busRouteFinderIndex)
                    //                            .BusesBuffer
                    //                            .Contains(crew))
                    //    {
                    //        busRoutsFirst.ElementAt(busRouteFinderIndex)
                    //                            .BusesBuffer
                    //                            .Add(crew);
                    //    }
                    //    else
                    //    {
                    //        var crewFinderIndex = busRoutsFirst.ElementAt(busRouteFinderIndex)
                    //                                                  .BusesBuffer
                    //                                                  .IndexOf(crew);
                    //        busRoutsFirst.ElementAt(busRouteFinderIndex)
                    //                            .BusesBuffer
                    //                            .ElementAt(crewFinderIndex)
                    //                            .InsertPoints(crew.MapPoints);
                    //    }
                    //}
                }
            }
            return busInformationFirst;
        }

        private static BusRoute[] SplitForwardBackwardBusRoutes(BusRoute busRoute)
        {
            var result = new BusRoute[]{new BusRoute()
            {
                Name = busRoute.Name,
                Active = true, 
                Id = Guid.NewGuid()
                         .ToString(),
                Direction = true
            },new BusRoute()
            {
                Name = busRoute.Name,
                Active = true,
                Id = Guid.NewGuid()
                    .ToString(),
                Direction = false
            }};
            if (!busRoute.MapPoints.Any()) return result;
            var arrMapPoints = busRoute.MapPoints
                                       .ToArray();
            var i = 0;
            var currentPath = 0;
            var count = arrMapPoints.Count()-1;
            do
            {
                result[currentPath].MapPoints
                                   .Add(arrMapPoints[i]);
                if (i >= count - 2)
                {
                    break;
                }
                if (MatPart.GaversinusMethod(arrMapPoints[i].Latitude, arrMapPoints[i + 1].Latitude,
                        arrMapPoints[i].Longitude, arrMapPoints[i + 1].Longitude) > 30
                    && MatPart.GaversinusMethod(arrMapPoints[i].Latitude, arrMapPoints[i + 2].Latitude,
                        arrMapPoints[i].Longitude, arrMapPoints[i + 2].Longitude) < 30)
                {
                    result[currentPath].MapPoints
                                       .Add(arrMapPoints[i + 1]);
                    currentPath = currentPath == 0
                                ? 1
                                : 0;
                    result[currentPath].MapPoints.Add(arrMapPoints[i+2]);
                }
                i++;
            } while (true);
            return result;
        }

        private static int CheckPowerPointInThisRoute(List<BusInformation> busesInformation, MapPoint mapPoint)
        {
            return busesInformation.Select(b => b.MapPoints.Contains(mapPoint))
                                   .Count();
        }

        public static List<BusRoute> CreateValidBusRoutes(List<BusInformation> busesInformation)
        {
            var result = new List<BusRoute>();
            var buses = new List<BusInformation>();
            var avOfBusPoints = (busesInformation.Select(b => b.MapPoints.Count)
                                                 .Sum())/(busesInformation.Count);
            var countInfo = ((busesInformation.Count) / 100) * 55;
            var availebleBusInformationList = busesInformation.FindAll(f => f.MapPoints.Count > avOfBusPoints);
            foreach (var bus in busesInformation)
            {
                buses.Add(bus);
            }
            var dataBaseWorker = new DataBaseWorker();
            var routeName = buses.First()
                                 .RouteName;
            do
            {
                var buffer = buses.FindAll(b => string.Equals(b.RouteName, routeName));
                var sum = 0;
                BusInformation selected = null;
                foreach (var bus in buffer)
                {
                    var correctSum = buffer
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
                    Name = routeName,
                    Active = true
                };
                foreach (var adedetItem in selected?.MapPoints ?? new List<MapPoint>())
                {
                    if(CheckPowerPointInThisRoute(availebleBusInformationList,adedetItem) > countInfo)
                    busRoute.MapPoints
                            .Add(adedetItem);
                }
                buses.RemoveAll(r => string.Equals(r.RouteName, routeName));
                routeName = buses.FirstOrDefault()
                                ?.RouteName;
                var busRoutseAfterSplitOnDirection = SplitForwardBackwardBusRoutes(busRoute);
                result.AddRange(busRoutseAfterSplitOnDirection);
            } while (buses.Count > 0);
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
            var arrPoints = mapPoints.ToArray();
            for (int i = indexFirst; i < indexLast; i++)
            {
                resultLength += MatPart.GaversinusMethod(arrPoints[i].Latitude, arrPoints[i + 1].Latitude,
                                                        arrPoints[i].Longitude, arrPoints[i + 1].Longitude);
            }
            return resultLength;
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
            //foreach (var item in baseRouteData)
            //{
            //    var currentBusRoute = busRoutes.Find(b => string.Equals(b.Name, item.BusRouteName));
            //    int pointStationInBusRoute = GetIndexPointInBusRoute(currentBusRoute,stationPoint);
            //    if (pointStationInBusRoute == -1) continue;
            //    foreach (var busCrew in item.BusesBuffer)
            //    {
            //        busCrew.MapPoints
            //               .Sort();
            //        var index = GetIndexPointInBusRoute(currentBusRoute, busCrew.MapPoints
            //                                                                         .Last());
            //        result.Add(item.BusRouteName + busCrew.CarNumber,GetTimeOfPredict(index,pointStationInBusRoute,currentBusRoute.MapPoints
            //                                                                                                                                      .ToList()));
            //    }
            //}
            return result;
        }
    }
}