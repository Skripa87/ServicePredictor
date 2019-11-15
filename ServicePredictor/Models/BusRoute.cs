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
        public bool Direction { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<MapPoint> MapPoints { get; set; }

        public BusRoute() 
        {
            MapPoints = new List<MapPoint>();
        }
    }
}