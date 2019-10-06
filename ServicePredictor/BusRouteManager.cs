using ServicePredictor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePredictor
{
    public class BusCarPointBuffer
    {
        public int NumberCar { get; set; }
        public string BusRouteName { get; set; }
        public List<FtpFileDataElement> FtpFileDataElements { get; set; }

        public BusCarPointBuffer(string busRouteName, int numberCar, List<FtpFileDataElement> ftpFileDataElements)
        {
            NumberCar = numberCar;
            BusRouteName = busRouteName;
            ftpFileDataElements.CopyTo(FtpFileDataElements.ToArray());
        }

        public int CalculatePowerEquals(BusCarPointBuffer busCarPointBuffer)
        {
            int result = 0;
            foreach (var item in FtpFileDataElements)
            {
                if (busCarPointBuffer.FtpFileDataElements.Any(f => MatPart.GaversinusMethod(f.Latitude, item.Latitude, f.Longitude, item.Longitude) <= 1))
                    result++;
            }
            return result;
        }
    }

    public class BusRouteNameBuffer
    {
        public string BusRouteName { get; set; }
        public List<BusCarPointBuffer> BusCars { get; set; }

        public BusRouteNameBuffer(string busRouteName, List<BusCarPointBuffer> busCarPoints)
        {
            BusRouteName = busRouteName;
            busCarPoints.CopyTo(BusCars.ToArray());
        }
    }

    public class BusRouteManager
    {
        private List<FtpFileDataElement> FtpFileDataElements { get; set; }

        public BusRouteManager(List<FtpFileDataElement> ftpFileDataElements)
        {
            FtpFileDataElements = new List<FtpFileDataElement>();
            ftpFileDataElements = SeparateUniqueElements(3.5, ftpFileDataElements);
            ftpFileDataElements.CopyTo(FtpFileDataElements.ToArray());
        }

        private List<FtpFileDataElement> SeparateUniqueElements(double dimensionSeparate, List<FtpFileDataElement> ftpFileDataElements)
        {
            var uniqueFtpFileDataElement = new List<FtpFileDataElement>();
            foreach (var item in ftpFileDataElements)
            {
                if (!uniqueFtpFileDataElement.Any(f => f.BusRouteName.Equals(item.BusRouteName)
                                                  && f.CarNumber.Equals(item.CarNumber)
                                                  && MatPart.GaversinusMethod(f.Latitude, item.Latitude, f.Longitude, item.Longitude) <= dimensionSeparate))
                {
                    uniqueFtpFileDataElement.Add(item);
                }
            }
            return uniqueFtpFileDataElement;
        }

        private int GetEtalonRoute(BusRouteNameBuffer busRouteName)
        {
            int finder = busRouteName?.BusCars
                                     ?.FirstOrDefault()
                                     ?.NumberCar ?? 0, 
                point = busRouteName?.BusCars
                                    ?.Select(s=>s.CalculatePowerEquals(busRouteName.BusCars
                                     .FirstOrDefault()))
                                    ?.Sum() ?? 0;
            foreach (var item in busRouteName.BusCars)
            {
                if(busRouteName?.BusCars
                               ?.Select(s => s.CalculatePowerEquals(item))
                               ?.Sum() > point)
                {
                    finder = item.NumberCar;
                }
            }
            return finder;
        }

        public void CreateBusRoute()
        {
            var bufferCarNumberList = new List<BusCarPointBuffer>();
            var bufferBusRouteNameList = new List<BusRouteNameBuffer>();
            var buffer = new List<FtpFileDataElement>();
            FtpFileDataElements.CopyTo(buffer.ToArray());
            while(buffer.Count > 0)
            {
                var currentCar = buffer.FirstOrDefault();
                bufferCarNumberList.Add(new BusCarPointBuffer(currentCar.BusRouteName, 
                                                              currentCar.CarNumber, 
                                                              buffer.FindAll(b => b.CarNumber.Equals(currentCar.CarNumber))));
                buffer.RemoveAll(r => r.CarNumber.Equals(currentCar.CarNumber));                
            }
            while(bufferCarNumberList.Count > 0)
            {
                var currentItem = bufferCarNumberList.FirstOrDefault();
                bufferBusRouteNameList.Add(new BusRouteNameBuffer(currentItem.BusRouteName, bufferCarNumberList.FindAll(b => b.BusRouteName.Equals(currentItem.BusRouteName))));
                bufferCarNumberList.RemoveAll(b => b.BusRouteName.Equals(currentItem.BusRouteName));
            }
            foreach (var item in bufferBusRouteNameList)
            {
                var numerCarEtaloneRoute = GetEtalonRoute(item);
            }
        }
    }
}