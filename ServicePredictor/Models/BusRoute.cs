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
        public string StartStationId_Id { get; set; }
        public virtual Station StartStation { get; set; }
        public string LastStation_Id { get; set; }
        public virtual Station LastStation { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<Station> Stations { get; set; }
        public virtual ICollection<MapPoint> MapPoints { get; set; }

        public BusRoute() 
        {
            Stations = new List<Station>();
            MapPoints = new List<MapPoint>();
        }
    }
}