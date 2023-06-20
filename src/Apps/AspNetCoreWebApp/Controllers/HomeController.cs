using AspNetCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Practice.Client.Services.ClientManagers.Managers;
using Practice.Foundation.Infrastructure.DemoTypes;
using Practice.Foundation.Infrastructure.Types;
using System.Diagnostics;

namespace AspNetCoreWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PostgresRepository _postgresRepository;
        private readonly DataManager _dataManager;
        public HomeController(ILogger<HomeController> logger, PostgresRepository postgresRepository, DataManager dataManager)
        {
            _logger = logger;
            _postgresRepository = postgresRepository;
            _dataManager = dataManager;
        }

        public IActionResult Index()
        {
            List<BaseItem> Items = new List<BaseItem>();
            BaseItem baseItem1 = new BaseItem("Demo1");
            BaseItem baseItem2 = new BaseItem("Demo2");
            BaseItem baseItem3 = new BaseItem("Demo3");
            baseItem1.SetLanguageField("Name", "Nishchay");
            baseItem2.SetLanguageField("Name", "Hoshima");
            baseItem3.SetLanguageField("Name", "Vrinda");
            baseItem1.SetLanguageField("successfull", "TRUE");
            baseItem2.SetLanguageField("successfull", "TRUE");
            baseItem3.SetLanguageField("successfull", "TRUE");
            Items.Add(baseItem1);
            Items.Add(baseItem2);
            Items.Add(baseItem3);
            return View(Items);
        }

        [HttpPost]
        public IActionResult ExportToExcel(ExcelData excelData)
        {
            // Sample data for demonstration
            List<string> columns = excelData.Columns;
            Dictionary<string, List<string>> data = excelData.Data;

            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                // Create a new worksheet
                var worksheet = package.Workbook.Worksheets.Add("Data");

                // Write column headers
                for (int i = 0; i < columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = columns[i];
                }

                // Write data rows
                int row = 2;
                foreach (var kvp in data)
                {
                    string columnName = kvp.Key;
                    List<string> rowValues = kvp.Value;

                    for (int i = 0; i < rowValues.Count; i++)
                    {
                        worksheet.Cells[row, i + 1].Value = rowValues[i];
                    }

                    row++;
                }

                // Generate a unique file name
                string fileName = "data_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                // Save the Excel package to a stream
                var stream = new MemoryStream();
                package.SaveAs(stream);

                // Set the position of the stream to the beginning
                stream.Position = 0;

                // Return the Excel file as a FileStreamResult with the "application/octet-stream" content type
                return new FileStreamResult(stream, "application/octet-stream")
                {
                    FileDownloadName = fileName
                };
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult UploadExcel(IFormFile excelFile)
        {
            if (excelFile != null && excelFile.Length > 0)
            {
                ExcelData data = new ExcelData();
                var filePath = Path.Combine("D:\\", excelFile.FileName);
                (data.Columns, data.Data) = _dataManager.PublishToMySql(filePath);
                return View(data);
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}