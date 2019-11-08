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

        public void CreateXlDocument(List<BusRouteBuffer> busRoutes)
        {
            var workbook = new XLWorkbook();
            var number = 0;
            foreach (var busRoute in busRoutes)
            {
                foreach (var crew in busRoute.BusesBuffer)
                {
                    number++;
                    //string num = "1";
                    var ws = workbook.Worksheets.Add("ws" + number);
                    var range = ws.Range(1, 1, 1, 15);
                    range.Merge();
                    range.SetValue(
                        $"Маршрут номер = {busRoute.BusRouteName}, автобус номер = {crew.CarNumber}, график = {crew.Sheduler}, смена = {crew.Turn}");
                    ws.Cell(2, 1).SetValue($"Азимут");
                    ws.Cell(2, 2).SetValue($"Широта");
                    ws.Cell(2, 3).SetValue($"Долгота");
                    var row = 3;
                    foreach (var item in crew.MapPoints)
                    {
                        ws.Cell(row, 1).SetValue(item.Azimut);
                        ws.Cell(row, 2).SetValue(item.Latitude);
                        ws.Cell(row, 3).SetValue(item.Longitude);
                        row++;
                        if (row > 64000)
                        {
                            number++;
                            ws = workbook.Worksheets.Add("ws" + number);
                            range = ws.Range(1, 1, 1, 15);
                            range.Merge();
                            range.SetValue(String.Format("Маршрут номер = {0}, автобус номер = {1}, график = {2}, смена = {3}", busRoute.BusRouteName, crew.CarNumber, crew.Sheduler, crew.Turn));
                            ws.Cell(2, 1).SetValue("Азимут");
                            ws.Cell(2, 2).SetValue("Широта");
                            ws.Cell(2, 3).SetValue("Долгота");
                            row = 3;
                        }
                    }
                    ws.Columns().AdjustToContents();
                }
            }
            workbook.SaveAs(FileName);
        }

        public void CreateXlDocument(List<BusRoute> busRoutes)
        {
            var workbook = new XLWorkbook();
            var numbers = 0;
            foreach (var busRoute in busRoutes)
            {
                numbers++;
                //string num = "1";
                var ws = workbook.Worksheets.Add("ws" + numbers);
                var range = ws.Range(1, 1, 1, 15);
                range.Merge();
                range.SetValue($"Маршрут номер = {busRoute.Name}");
                ws.Cell(2, 1).SetValue("Азимут");
                ws.Cell(2, 3).SetValue("Долгота");
                ws.Cell(2, 2).SetValue("Широта");
                ws.Cell(2, 4).SetValue("Остановка");
                ws.Cell(2, 5).SetValue("Время посещения");
                var row = 3;
                foreach (var item in busRoute.MapPoints)
                {
                    ws.Cell(row, 1).SetValue(item.Azimut);
                    ws.Cell(row, 2).SetValue(item.Longitude);
                    ws.Cell(row, 3).SetValue(item.Latitude);
                    ws.Cell(row, 4).SetValue(busRoute.Stations
                                                           .FirstOrDefault(s => s.Lat.Equals(item.Latitude) 
                                                                             && s.Lng.Equals(item.Longitude))
                                                     ?.Name ?? "");
                    ws.Cell(row, 5).SetValue(item.TimePoint);
                    row++;
                    if (row <= 64000) continue;
                    numbers++;
                    ws = workbook.Worksheets.Add("ws" + numbers);
                    range = ws.Range(1, 1, 1, 15);
                    range.Merge();
                    range.SetValue($"Маршрут номер = {busRoute.Name}");
                    ws.Cell(2, 1).SetValue($"Азимут");
                    ws.Cell(2, 2).SetValue($"Долгота");
                    ws.Cell(2, 3).SetValue($"Широта");
                    ws.Cell(2, 4).SetValue($"Остановка");
                    ws.Cell(2, 5).SetValue($"Время посещения");
                    row = 3;
                }
                ws.Columns().AdjustToContents();
            }
            workbook.SaveAs(FileName);
        }
    }
}