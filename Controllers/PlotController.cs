using Microsoft.AspNetCore.Mvc;
using System.IO;
using OfficeOpenXml;
using System.Diagnostics;

namespace LinearRegressionPlot.Controllers
{
    public class PlotController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadDirectory = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads";
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                var filePath = Path.Combine(uploadDirectory, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Call Python script
                var result = RunPythonScript(filePath);
                return Json(new { success = true, imagePath = result });
            }

            return Json(new { success = false });
        }

        private string RunPythonScript(string filePath)
        {
            var pythonPath = "C:\\Users\\sis.guest\\AppData\\Local\\Programs\\Python\\Python312\\python.exe"; // or the full path to your Python executable
            var scriptPath = "C:\\Users\\sis.guest\\source\\repos\\Quick_ML\\ExcelReader\\Models\\Py_Plot.py";

            var startInfo = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = $"{scriptPath} {filePath}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
                return $"wwwroot/plot.png"; // Update with the actual output path of the image
            }
        }
    }
}