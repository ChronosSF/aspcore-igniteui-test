using Infragistics.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using IgniteUI.AspCore.Test.Models;
using System.Diagnostics;

namespace IgniteUI.AspCore.Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        #region Controller Actions
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GridModel()
        {
            GridPaging paging = new GridPaging();
            paging.PageSize = 5;
            paging.Locale = new Infragistics.Web.Mvc.Grid.Paging.PagingLocale();
            paging.Locale.PagerRecordsLabelTemplate = "${startRecord} - ${endRecord}";
            GridModel grid = new GridModel();
            grid.DataSource = Orders.AsQueryable();
            grid.Width = "1000px";
            grid.Features.Add(paging);
            return View(grid);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        #endregion

        [HttpGet]
        [ActionName("GetWord")]
        public FileStreamResult WordDocument()
        {
            WordExportingModel wordExporter = new();
            wordExporter.PopulateWordDocument(Orders);
            return SendForDownload(wordExporter.WordMemoryStream);
        }

        public async Task<IActionResult> Download()
        {
            WordExportingModel wordExporter = new();
            wordExporter.PopulateWordDocument(Orders);
            wordExporter.WordMemoryStream.Position = 0;
            return File(wordExporter.WordMemoryStream, "application/octet-stream", "word.doc");
        }

        [GridDataSourceAction]
        public IActionResult GetData()
        {
            return View(Orders.AsQueryable());
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private FileStreamResult SendForDownload(MemoryStream ms)
        {
            return new FileStreamResult(ms, "application/octet-stream")
            {
                FileDownloadName = "word.doc"
            };
        }

        private List<Order> Orders
        {
            get
            {
                return new()
                {
                    new() { OrderID = 1, ContactName = "Some Name 1", OrderDate = DateTime.Today, ShipAddress = "Some Address" },
                    new() { OrderID = 2, ContactName = "Some Name 2", OrderDate = DateTime.Now, ShipAddress = "Some Address" },
                    new() { OrderID = 3, ContactName = "Some Name 3", OrderDate = DateTime.MaxValue, ShipAddress = "Some Address" },
                };
            }
        }
    }
}
