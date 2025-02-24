using WebApp.DTOS;
using Microsoft.AspNetCore.Mvc;
using WebApp.Servicies;
using WebApp.ViewMOdels;
using Serilog;
using InvoiceAPI.Logs;

namespace WebApp.Controllers
{
    public class StoreController : Controller
    {
        private readonly StoreService _storeService;
        private static readonly Serilog.ILogger _logger = Log.ForContext<InvoiceService>();
        public StoreController(StoreService storeService)
        {
            _storeService = storeService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var storeData = await _storeService.GetAllStoreAsync();
                if (storeData == null)
                {
                    _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Warning
                   , "No store data found");
                    return NotFound();
                }
                var viewModel = new StoreInvoiceViewModel
                {
                    Store = storeData,
                    Invoice = null
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Error
                    , "Error While fetching store data");
                return View();
            }

        }


        public async Task<IActionResult> GetById([FromBody]int id)
        {
            try
            {
                var storeData = await _storeService.GetStoreByIdAsync(id);
                if (storeData == null)
                {
                    _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Warning
                   , "No store data found");
                    return Json(new { Success = false });
                }
                //var viewModel = new StoreInvoiceViewModel
                //{
                //    Store = storeData,
                //    Invoice = null
                //};
           
                return Json(new { Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Error
                    , "Error While fetching store data");
                return View();
            }

        }

    }

}
