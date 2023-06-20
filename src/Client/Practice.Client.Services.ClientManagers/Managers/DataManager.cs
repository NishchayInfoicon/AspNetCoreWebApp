using OfficeOpenXml;
using System.Data.Common;

namespace Practice.Client.Services.ClientManagers.Managers
{
    public class DataManager
    {


        public (List<string>, Dictionary<string, List<string>>) PublishToMySql(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.First();
                int totalColumn = worksheet.Dimension.End.Column;
                int totalRow = worksheet.Dimension.End.Row;
                List<string> columns = new List<string>();
                for (int column = 1; column <= totalColumn; column++)
                {
                    List<string> RowData = new List<string>();

                    for (int row = 1; row < totalRow; row++)
                    {
                        if (row == 1 && !columns.Contains(worksheet.Cells[1, column].Text))
                        {
                            columns.Add(worksheet.Cells[row, column].Text);
                        }
                        if (!String.IsNullOrEmpty(worksheet.Cells[row, column].Text))
                        {
                            RowData.Add(worksheet.Cells[row, column].Text);
                        }
                    }
                    if (RowData.Any())
                    {
                        RowData.RemoveAt(0);
                    }
                    data[worksheet.Cells[1, column].Text] = RowData;

                }
                return (columns, data);
            }
        }
    }
}
