using ServicePredictor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
                    var busRouteFinder = busRoutsFirst.Find(f => string.Equals(f.BusRouteName,item.BusRouteName));
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
                var sum = 0; int selected = 0;
                foreach (var bus in item.BusesBuffer)
                {
                    var correctSumm = item.BusesBuffer
                                         .Select(s => bus.SimilarityCount(s))
                                         .Sum();
                    if(correctSumm > sum)
                    {
                        selected = bus.CarNumber;
                        sum = correctSumm;
                    }
                }
                var selectedBusCrew = item.BusesBuffer
                                          .Find(f => f.CarNumber == selected);
                var busRoute = new BusRoute()
                {
                    Id = Guid.NewGuid()
                             .ToString(),
                    Name = item.BusRouteName,
                    Active = true
                };
                selectedBusCrew.MapPoints
                               .Sort();
                foreach (var point in selectedBusCrew.MapPoints)
                {
                    var finderStation = stations.Find(s => (MatPart.GaversinusMethod(s.Lat, point.Latitude, s.Lng, point.Longitude) <= 26));
                    if(finderStation != null && !busRoute.Stations
                                                         .Contains(finderStation)) 
                    {
                        busRoute.Stations
                                .Add(finderStation);
                        busRoute.MapPoints
                                .Add(new MapPoint()
                        {
                            Speed = 0,
                            Id = Guid.NewGuid()
                                     .ToString(),
                            Latitude = finderStation.Lat,
                            Longitude = finderStation.Lng,
                            TimePoint = point.TimePoint
                                             .AddSeconds(5),
                            Azimut = point.Azimut
                        });
                    }
                    busRoute.MapPoints
                            .Add(point);
                }
                result.Add(busRoute);
            }
            dataBaseWorker.SaveBusRoute(result);
            return result;
        }
    }
}