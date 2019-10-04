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

        public BusCarPointBuffer(string busRouteName, int numberCar)
        {
            NumberCar = numberCar;
            BusRouteName = busRouteName;
        }
    }

    public class BusRouteNameBuffer
    {
        public string BusRouteName { get; set; }
        public List<BusCarPointBuffer> BusCars { get; set; }

        public BusRouteNameBuffer(string busRouteName)
        {
            BusRouteName = busRouteName;
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

        public void CreateBusRoute()
        {
            var bufferCarNumberList = new List<BusCarPointBuffer>();
            var busRouteNameBuffe = new List<BusRouteNameBuffer>();
            var buffer = new List<FtpFileDataElement>();
            FtpFileDataElements.CopyTo(buffer.ToArray());
            while(buffer.Count > 0)
            {
                var currentCarNumber = buffer.FirstOrDefault()
                                            ?.CarNumber;

            }
        }
    }
}