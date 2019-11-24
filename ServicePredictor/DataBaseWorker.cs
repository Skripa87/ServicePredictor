using ServicePredictor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePredictor
{
    public class DataBaseWorker
    {
        private ServicePredictorDbContext Db { get; set; }
        public DataBaseWorker()
        {
            Db = new ServicePredictorDbContext();
        }

        public List<Station> GetStations() 
        {
            return Db.Stations.ToList();
        }

        public Station GetStation(string stationId)
        {
            return Db.Stations
                     .ToList()
                     .Find(s => string.Equals(s.Id, stationId));
        }

        public List<BusRoute> GetBusRouts()
        {
            return Db.BusRoutes.ToList();
        }

        public void SaveBusRoute(List<BusRoute> busRoutes) 
        {
            if (busRoutes == null || busRoutes.Count == 0) return;
            foreach (var item in busRoutes)
            {
                var current = Db.BusRoutes.Any()
                            ? Db.BusRoutes
                                .FirstOrDefault(s => s.Name.Equals(item.Name))
                            :null;
                if (current != null)
                {
                    current.Active = false;
                }
            }
            Db.BusRoutes.AddRange(busRoutes);
            Db.SaveChanges();
        }
    }
}