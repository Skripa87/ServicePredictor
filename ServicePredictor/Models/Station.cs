using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePredictor.Models
{
    public class Station:IEquatable<Station>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public bool Type { get; set; }
        public bool Active { get; set; }
        public string InformationTable_Id { get; set; }
        public string AccessCode { get; set; }
        public string UserCity { get; set; }

        public virtual ICollection<BusRoute> BusRoutes { get; set; }

        public Station()
        {
            BusRoutes = new List<BusRoute>();
        }

        public bool Equals(Station other)
        {
            return Lat.Equals(other.Lat) && Lng.Equals(other.Lng) && Name.Equals(other.Name);
        }
    }
}