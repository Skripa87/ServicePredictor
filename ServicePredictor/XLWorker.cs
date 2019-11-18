﻿using System;
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
                var row = 3;
                foreach (var item in busRoute.MapPoints)
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
                    row = 3;
                }
                ws.Columns().AdjustToContents();
            }
            workbook.SaveAs(FileName);
        }
    }
}