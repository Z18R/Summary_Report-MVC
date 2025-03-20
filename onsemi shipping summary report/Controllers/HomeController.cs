using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using onsemi_shipping_summary_report.Models;
using System.Data;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;
using onsemi_shipping_summary_report.Services;
using onsemi_shipping_summary_report.Store_Procedure.onsemi;
using LotTrackingApp.Services;

namespace onsemi_shipping_summary_report.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductionOrderService _productionOrderService;
        private readonly IConfiguration _configuration;
        private readonly WaferService _waferService;
        private readonly SearchGemAssyService _searchGemAssyService;
        private readonly DeviceService _deviceService;
        public readonly ExportService _exportService;
        public readonly LotTrackingService _lotTrackingService;
        public readonly DatalogsService _datalogService;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, ProductionOrderService productionOrderService,
            WaferService wafermapService, SearchGemAssyService searchGemAssyService, 
            DeviceService deviceService, ExportService exportService,
            LotTrackingService lotTrackingService, DatalogsService datalogsService)
        {
            _logger = logger;
            _configuration = configuration;
            _productionOrderService = productionOrderService;
            _waferService = wafermapService;
            _searchGemAssyService = searchGemAssyService;
            _deviceService = deviceService;
            _exportService = exportService;
            _lotTrackingService = lotTrackingService;
            _datalogService = datalogsService;

        }
        public  IActionResult LotTrackingService()
        {
            ViewData["ActiveNav"] = "Home";
            return View(Home);
        }

        [HttpGet]
        public IActionResult GetLotDetails(string lotAlias)
        {
            var lotDetails = _lotTrackingService.GetLotDetails(lotAlias);
            return PartialView("_LotDetailsPartial", lotDetails);
        }

        public IActionResult Home()
        {
            ViewData["ActiveNav"] = "Home";
            return View(Home);
        }

        public IActionResult Index()
        {
            ViewData["ActiveNav"] = "Index";
            var model = new DateFilterViewModel
            {
                FromDate = new DateTime(2024, 1, 1),
                ToDate = new DateTime(2024, 6, 19)
            };
            return View(model);
        }

        public IActionResult Privacy(int pageNumber = 1, int pageSize = 10)
        {
            ViewData["ActiveNav"] = "Gem";
            var productionOrders = _productionOrderService.GetProductionOrders(pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRecords = _productionOrderService.GetTotalRecordCount();
            return View(productionOrders);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult SearchWafermap(string device)
        {
            var wafermapList = _waferService.SearchWafermap(device);
            return View("SearchResults", wafermapList);

        }

        //datalogs

        [HttpGet]
        public IActionResult SearchDatalogs(string device)
        {
            var datalogsList = _datalogService.SearchDatalogs(device);
            return View("SearchResultsDatalogs", datalogsList);

        }

        [HttpGet]

        public IActionResult SearchGemAssy(string lotnumber)
        {
            var search_GemLotsList = _searchGemAssyService.SearchGemLots(lotnumber);
            return View("search_GemLots", search_GemLotsList);

        }

        //INSERTING DATA TO THE DATABASE
        [HttpPost]
        public IActionResult InsertDevice(ViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Model state is not valid.";
                return RedirectToAction("Index");
            }

            try
            {
                _deviceService.InsertDevice(model);
                TempData["SuccessMessage"] = "Device inserted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while inserting the device: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ExportToExcel(DateFilterViewModel model)
        {
            var result = await _exportService.ExportDataAsync(model, Class.sp_ReportDataExport, 
                                                                "ShippingReport", true, false);
            return HandleExportResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> ExportToExcelSummary(DateFilterViewModel model)
        {
            var result = await _exportService.ExportDataAsync(model, Class.usp_RPT_Onsemi_Shipped_Lot,
                                                                "SummaryReport", false, false);
            return HandleExportResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> ExportToExcelDeviceSkip(DateFilterViewModel model)
        {
            var result = await _exportService.ExportDataAsync(model, Class.usp_ONSEMI_SkipLot_Extractor, 
                                                                "OnsemiDeviceReport", false, true);
            return HandleExportResult(result);
        }

        private IActionResult HandleExportResult(ExportResult result)
        {
            if (result.IsSuccess)
            {
                return File(result.Stream, result.ContentType, result.FileName);
            }
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToAction("Index");
        }

    }
}