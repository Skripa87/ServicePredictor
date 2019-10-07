using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePredictor.Models
{
    public class BusRouteBuffer:IEquatable<BusRouteBuffer>
    {
        public string BusRouteName { get; set; }
        public List<BusCrew> BusesBuffer { get; set; }

        public BusRouteBuffer(string nameBusRoute)
        {
            BusRouteName = nameBusRoute;
            BusesBuffer = new List<BusCrew>();
        }
        public void InsertBuses(BusCrew busCrew)
        {
            if (!BusesBuffer.Contains(busCrew) || BusesBuffer.Count == 0) 
            {
                BusesBuffer.Add(busCrew);
            }
            else 
            {
                BusesBuffer.ElementAt(BusesBuffer.IndexOf(busCrew))
                           .InsertPoints(busCrew.MapPoints);
            }
        }
        
        public bool Equals(BusRouteBuffer other)
        {
            return other.BusRouteName.Equals(BusRouteName);
        }
    }
}