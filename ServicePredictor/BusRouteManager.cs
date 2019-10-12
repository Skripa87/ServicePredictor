using ServicePredictor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePredictor
{
    public static class BusRouteManager
    {
        public static List<BusRouteBuffer> AttachBusRoutes(List<BusRouteBuffer> busRoutesBufferFirst, List<BusRouteBuffer> busRoutesbufferSecond)
        {
            if (busRoutesBufferFirst == null && busRoutesbufferSecond != null) return busRoutesbufferSecond;
            if (busRoutesBufferFirst != null && busRoutesbufferSecond == null) return busRoutesBufferFirst;
            if (busRoutesBufferFirst == null && busRoutesbufferSecond == null) return new List<BusRouteBuffer>();
            foreach (var item in busRoutesbufferSecond)
            {
                if (!busRoutesBufferFirst.Contains(item))
                {
                    busRoutesBufferFirst.Add(item);
                }
                else
                {
                    var busRouteFinder = busRoutesBufferFirst.Find(f => f.BusRouteName.Equals(item.BusRouteName));
                    var busRouteFinderIndex = busRoutesBufferFirst.IndexOf(busRouteFinder);
                    foreach (var crew in item.BusesBuffer)
                    {
                        if (!busRoutesBufferFirst.ElementAt(busRouteFinderIndex)
                                                .BusesBuffer
                                                .Contains(crew))
                        {
                            busRoutesBufferFirst.ElementAt(busRouteFinderIndex)
                                                .BusesBuffer
                                                .Add(crew);
                        }
                        else
                        {
                            var crewFinder = busRoutesBufferFirst.ElementAt(busRouteFinderIndex)
                                                                 .BusesBuffer
                                                                 .Find(f => f.CarNumber.Equals(crew.CarNumber));
                            var crewFinderIndex = busRoutesBufferFirst.ElementAt(busRouteFinderIndex)
                                                                      .BusesBuffer
                                                                      .IndexOf(crew);
                            busRoutesBufferFirst.ElementAt(busRouteFinderIndex)
                                                .BusesBuffer
                                                .ElementAt(crewFinderIndex)
                                                .InsertPoints(crew.MapPoints);
                        }
                    }
                }
            }
            return busRoutesBufferFirst;
        }

        public static List<BusRoute> CreateValidBusRoutes(List<BusRouteBuffer> busRoutesBuffer)
        {
            var result = new List<BusRoute>();
            var dataBaseWorker = new DataBaseWorker();
            var stations = dataBaseWorker.GetStations();
            foreach (var item in busRoutesBuffer)
            {
                var summ = 0; int selected = 0;
                foreach (var bus in item.BusesBuffer)
                {
                    var correctSumm = item.BusesBuffer
                                         .Select(s => bus.SimilarityCount(s))
                                         .Sum();
                    if(correctSumm > summ)
                    {
                        selected = bus.CarNumber;
                        summ = correctSumm;
                    }
                }
                var selectedBusCrew = item.BusesBuffer
                                          .Find(f => f.CarNumber.Equals(selected));
                var busRoute = new BusRoute()
                {
                    Id = Guid.NewGuid()
                             .ToString(),
                    Name = item.BusRouteName,
                    Active = true
                };
                var busRouteStations = new List<Station>();
                foreach (var point in selectedBusCrew.MapPoints)
                 {
                    var finderStation = stations.Find(s => MatPart.GaversinusMethod(s.Lat, point.Latitude, s.Lng, point.Longitude) <= 150);
                    if(finderStation != null && !busRouteStations.Contains(finderStation)) 
                    {
                        busRouteStations.Add(finderStation);
                        busRoute.MapPoints.Add(new MapPoint()
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
                    busRoute.MapPoints.Add(point);
                }
                result.Add(busRoute);
            }
            dataBaseWorker.SaveBusRoute(result);
            return result;
        }
    }
}