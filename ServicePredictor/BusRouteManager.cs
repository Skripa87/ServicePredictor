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
    }
}