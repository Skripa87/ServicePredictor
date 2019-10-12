using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using ServicePredictor.Models;

namespace ServicePredictor
{
    public class XLWorker
    {
        public string FileName { get; set; }

        public XLWorker(string fileName)
        {
            FileName = fileName;
        }

        public void CreateXLDocument(List<BusRouteBuffer> busRoutes)
        {
            XLWorkbook workbook = new XLWorkbook();
            int wsnumber = 0;
            foreach (var busRoute in busRoutes)
            {
                foreach (var crew in busRoute.BusesBuffer)
                {
                    wsnumber++;
                    //string num = "1";
                    var ws = workbook.Worksheets.Add("ws" + wsnumber);
                    var range = ws.Range(1, 1, 1, 15);
                    range.Merge();
                    range.SetValue(String.Format("Маршрут номер = {0}, автобус номер = {1}, график = {2}, смена = {3}", busRoute.BusRouteName, crew.CarNumber, crew.Sheduler, crew.Turn));
                    ws.Cell(2, 1).SetValue("Азимут");
                    ws.Cell(2, 2).SetValue("Широта");
                    ws.Cell(2, 3).SetValue("Долгота");
                    var row = 3;
                    var k = 0;
                    foreach (var item in crew.MapPoints)
                    {
                        ws.Cell(row, 1).SetValue(item.Azimut);
                        ws.Cell(row, 2).SetValue(item.Latitude);
                        ws.Cell(row, 3).SetValue(item.Longitude);
                        row++;
                        if (row > 64000)
                        {
                            k++;
                            wsnumber++;
                            ws = workbook.Worksheets.Add("ws" + wsnumber);
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

        public void CreateXLDocument(List<BusRoute> busRoutes)
        {
            XLWorkbook workbook = new XLWorkbook();
            int wsnumber = 0;
            foreach (var busRoute in busRoutes)
            {
                wsnumber++;
                //string num = "1";
                var ws = workbook.Worksheets.Add("ws" + wsnumber);
                var range = ws.Range(1, 1, 1, 15);
                range.Merge();
                range.SetValue(String.Format("Маршрут номер = {0}", busRoute.Name));
                ws.Cell(2, 1).SetValue("Азимут");
                ws.Cell(2, 2).SetValue("Широта");
                ws.Cell(2, 3).SetValue("Долгота");
                var row = 3;
                var k = 0;
                foreach (var item in busRoute.MapPoints)
                {
                    ws.Cell(row, 1).SetValue(item.Azimut);
                    ws.Cell(row, 2).SetValue(item.Latitude);
                    ws.Cell(row, 3).SetValue(item.Longitude);
                    row++;
                    if (row > 64000)
                    {
                        k++;
                        wsnumber++;
                        ws = workbook.Worksheets.Add("ws" + wsnumber);
                        range = ws.Range(1, 1, 1, 15);
                        range.Merge();
                        range.SetValue(String.Format("Маршрут номер = {0}", busRoute.Name));
                        ws.Cell(2, 1).SetValue("Азимут");
                        ws.Cell(2, 2).SetValue("Широта");
                        ws.Cell(2, 3).SetValue("Долгота");
                        row = 3;
                    }
                }
                ws.Columns().AdjustToContents();
            }
            workbook.SaveAs(FileName);
        }
    }
}