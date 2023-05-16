using System.Diagnostics;
using System.IO;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Radiator
{
    internal class ExcelHelper
    {
        public void WriteFactGrid(Dictionary<string, DataGridView> fucktsOfPrognozes, string[,] results, string branchName, DataGridView alt)
        {
            if (fucktsOfPrognozes.Count != results.GetLength(0))
            {
                throw new Exception("Чет не так, не совпадает бля");
            }
            XLWorkbook workbook = new();
            IXLWorksheet ws = workbook.Worksheets.Add("Отчет");
            int rowIterator = 2;
            ws.Cell($"A1").Value = branchName;
            ws.Cell($"A1").Style.Font.Bold = true;
            ws.Range($"A1:D1").Merge();
            for (int i = 0; i < results.GetLength(0); i++)
            {
                ws.Cell($"A{rowIterator}").Value = $"{fucktsOfPrognozes.ElementAt(i).Key}";
                ws.Cell($"A{rowIterator}").Style.Font.Bold = true;
                ws.Range($"A{rowIterator}:D{rowIterator}").Merge();
                rowIterator++;
                ws.Cell($"A{rowIterator}").Value = $"Месяц";
                ws.Cell($"A{rowIterator}").Style.Font.Bold = true;
                ws.Cell($"B{rowIterator}").Value = $"Год";
                ws.Cell($"B{rowIterator}").Style.Font.Bold = true;
                ws.Cell($"C{rowIterator}").Value = $"Температура окр. среды";
                ws.Cell($"C{rowIterator}").Style.Font.Bold = true;
                ws.Cell($"D{rowIterator}").Value = $"Цена на рекламу";
                ws.Cell($"D{rowIterator}").Style.Font.Bold = true;
                ws.Cell($"E{rowIterator}").Value = $"Скидка";
                ws.Cell($"E{rowIterator}").Style.Font.Bold = true;
                ws.Cell($"F{rowIterator}").Value = $"Продано";
                ws.Cell($"F{rowIterator}").Style.Font.Bold = true;
                ws.Cell($"G{rowIterator}").Value = $"Цена";
                ws.Cell($"G{rowIterator}").Style.Font.Bold = true;
                ws.Cell($"H{rowIterator}").Value = $"Цена конкурентов";
                ws.Cell($"H{rowIterator}").Style.Font.Bold = true;
                rowIterator++;
                for (int j = 0; j < fucktsOfPrognozes.ElementAt(i).Value.Rows.Count - 1; j++)
                {
                    ws.Cell($"A{rowIterator}").Value = fucktsOfPrognozes.ElementAt(i).Value.Rows[j].Cells[0].Value.ToString();
                    ws.Cell($"B{rowIterator}").Value = fucktsOfPrognozes.ElementAt(i).Value.Rows[j].Cells[1].Value.ToString();
                    ws.Cell($"C{rowIterator}").Value = fucktsOfPrognozes.ElementAt(i).Value.Rows[j].Cells[2].Value.ToString();
                    ws.Cell($"D{rowIterator}").Value = fucktsOfPrognozes.ElementAt(i).Value.Rows[j].Cells[3].Value.ToString();
                    ws.Cell($"E{rowIterator}").Value = fucktsOfPrognozes.ElementAt(i).Value.Rows[j].Cells[4].Value.ToString();
                    ws.Cell($"F{rowIterator}").Value = fucktsOfPrognozes.ElementAt(i).Value.Rows[j].Cells[5].Value.ToString();
                    ws.Cell($"G{rowIterator}").Value = fucktsOfPrognozes.ElementAt(i).Value.Rows[j].Cells[6].Value.ToString();
                    ws.Cell($"H{rowIterator}").Value = fucktsOfPrognozes.ElementAt(i).Value.Rows[j].Cells[7].Value.ToString();
                    rowIterator++;
                }
                ws.Cell($"A{rowIterator}").Value = $"Температура окр. среды";
                ws.Cell($"A{rowIterator}").Style.Font.Bold = true;
                ws.Cell($"B{rowIterator}").Value = $"Цена";
                ws.Cell($"B{rowIterator}").Style.Font.Bold = true;
                ws.Cell($"C{rowIterator}").Value = $"Цена конкурентов";
                ws.Cell($"C{rowIterator}").Style.Font.Bold = true;
                ws.Cell($"D{rowIterator}").Value = $"Цена на рекламу";
                ws.Cell($"D{rowIterator}").Style.Font.Bold = true;
                ws.Cell($"E{rowIterator}").Value = $"Скидка";
                ws.Cell($"E{rowIterator}").Style.Font.Bold = true;
                rowIterator++;
                ws.Cell($"A{rowIterator}").Value = results[i, 0];
                ws.Cell($"B{rowIterator}").Value = results[i, 1];
                ws.Cell($"C{rowIterator}").Value = results[i, 2];
                ws.Cell($"D{rowIterator}").Value = results[i, 3];
                ws.Cell($"E{rowIterator}").Value = results[i, 4];
                rowIterator++;
                ws.Cell($"A{rowIterator}").Value = results[i, 5];
                ws.Cell($"A{rowIterator}").Style.Font.Bold = true;
                ws.Range($"A{rowIterator}:H{rowIterator}").Merge();
                rowIterator += 2;
            }
            ws.Cell($"A{rowIterator}").Value = $"Подбор оптимальных альтернатив";
            ws.Cell($"A{rowIterator}").Style.Font.Bold = true;
            rowIterator++;
            ws.Cell($"A{rowIterator}").Value = $"Альтернатива";
            ws.Cell($"A{rowIterator}").Style.Font.Bold = true;
            for (int i = 0; i < alt.Rows.Count - 1; i++)
            {
                ws.Cell($"A{rowIterator}").Value = alt.Rows[i].Cells[0].Value.ToString();
                ws.Range($"A{rowIterator}:H{rowIterator}").Merge();
                rowIterator++;
            }
            var rngTable = ws.Range($"A1:H" + (rowIterator - 1));
            rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Columns().AdjustToContents();
            workbook.SaveAs("kek.xlsx");
        }
    }
}
