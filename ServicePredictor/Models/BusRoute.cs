using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePredictor.Models
{
    public class BusRoute
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BusStopStart_Id { get; set; }
        public virtual BusStop BusStopStart { get; set; }
        public string BusStopEnd_Id { get; set; }
        public virtual BusStop BusStopEnd { get; set; }
        public virtual ICollection<BusStop> BusStops { get; set; }
        public virtual ICollection<MapPoint> MapPoints { get; set; }

        public BusRoute() 
        {
            BusStops = new List<BusStop>();
            MapPoints = new List<MapPoint>();
        }
    }
}