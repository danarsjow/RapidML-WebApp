using ExcelDataReader;
using ExcelReader.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace ExcelReader.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ExcelFileReader()
            { return View(); }

        [HttpPost]
        public async Task<IActionResult>ExcelFileReader(IFormFile file)
            {   // Upload File
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                if(file != null && file.Length > 0)
                {
                    var uploadDirectory = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads";
                    if (!Directory.Exists(uploadDirectory))
                        {
                        Directory.CreateDirectory(uploadDirectory);
                        }

                    var filePath = Path.Combine(uploadDirectory, file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))

                        {
                        await file.CopyToAsync(stream);
                        }
                //Read File
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                    var excelData = new List<List<object>>();
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Choose one of either 1 or 2:

                        // 1. Use the reader methods
                        do
                        {
                            while (reader.Read())
                            {
                                // reader.GetDouble(0);
                                var rowData = new List<object>();
                                for (int column = 0; column < reader.FieldCount; column++)
                                {
                                    rowData.Add(reader.GetValue(column));
                                }
                                excelData.Add(rowData);
                            }
                        } while (reader.NextResult());

                        //// 2. Use the AsDataSet extension method  // ---- ALTERNATIVE METHOD
                        //var result = reader.AsDataSet();

                        //// The result of each spreadsheet is in result.Tables
                        ///
                        ViewBag.excelData = excelData;
                    }
                }

            }
                return View();
            }

        public IActionResult ML()
        { return View(); }

    }
            
}


