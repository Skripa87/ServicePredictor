using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using ServicePredictor.Models;

namespace ServicePredictor
{
    public class XlWorker
    {
        public string FileName { get; set; }

        public XlWorker(string fileName)
        {
            FileName = fileName;
        }

        public void CreateXlDocument(List<BusRoute> busRoutes)
        {
            var workbook = new XLWorkbook();
            var numbers = 0;
            var dbWorker = new DataBaseWorker();
            var stations = dbWorker.GetStations();
            foreach (var busRoute in busRoutes)
            {
                numbers++;
                //string num = "1";
                var ws = workbook.Worksheets.Add("ws" + numbers);
                var range = ws.Range(1, 1, 1, 15);
                range.Merge();
                range.SetValue($"Маршрут номер = {busRoute.Name}");
                ws.Cell(2, 1).SetValue("Маршрут");//$"Направление {(busRoute.Direction ? "Прямое" : "Обратное")}");
                ws.Cell(2, 2).SetValue("Долгота");
                ws.Cell(2, 3).SetValue("Широта");
                ws.Cell(2, 4).SetValue("Часы");
                ws.Cell(2, 5).SetValue("Минуты");
                ws.Cell(2, 6).SetValue("Секунды");
                ws.Cell(2, 7).SetValue("Азимут");//("Остановка");
                ws.Cell(2, 8).SetValue("Скорость");//("Остановка");
                ws.Cell(2, 9).SetValue("Позиция");
                var row = 3;
                var list = busRoute.MapPoints.ToList();
                foreach (var item in list)
                {
                    ws.Cell(row, 1)
                      .SetValue(busRoute.Name);
                    ws.Cell(row, 2).SetValue(item.Longitude);
                    ws.Cell(row, 3).SetValue(item.Latitude);
                    //var name = stations.Find(s => MatPart.GaversinusMethod(s.Lat,item.Latitude, s.Lng, item.Longitude) < 35)?.Name ?? "";
                    //ws.Cell(row, 5).SetValue(name);
                    ws.Cell(row, 4).SetValue(item.TimePoint.ToString("hh"));
                    ws.Cell(row, 5).SetValue(item.TimePoint.ToString("mm"));
                    ws.Cell(row, 6).SetValue(item.TimePoint.ToString("ss"));
                    ws.Cell(row, 7).SetValue(item.Azimut);
                    ws.Cell(row, 8).SetValue(item.Speed);
                    ws.Cell(row, 9).SetValue(list.IndexOf(item));
                    row++;
                    if (row <= 64000) continue;
                    numbers++;
                    ws = workbook.Worksheets.Add("ws" + numbers);
                    range = ws.Range(1, 1, 1, 15);
                    range.Merge();
                    range.SetValue($"Маршрут номер = {busRoute.Name}");
                    ws.Cell(2, 1).SetValue("Маршрут");//$"Направление {(busRoute.Direction ? "Прямое" : "Обратное")}");
                    ws.Cell(2, 2).SetValue("Долгота");
                    ws.Cell(2, 3).SetValue("Широта");
                    ws.Cell(2, 4).SetValue("Часы");
                    ws.Cell(2, 5).SetValue("Минуты");
                    ws.Cell(2, 6).SetValue("Секунды");
                    ws.Cell(2, 7).SetValue("Азимут");//("Остановка");
                    ws.Cell(2, 8).SetValue("Скорость");//("Остановка");
                    ws.Cell(2, 9).SetValue("Позиция");
                    row = 3;
                }
                ws.Columns().AdjustToContents();
            }
            workbook.SaveAs(FileName);
        }
    }
}