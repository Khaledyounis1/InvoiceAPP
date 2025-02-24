using InvoiceAPI.Logs;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Linq.Expressions;
using System.Security.Permissions;
using WebApp.DTOS;
using WebApp.Servicies;
using WebApp.ViewMOdels;

namespace WebApp.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly InvoiceService _invoiceService;
        private readonly StoreService _storeService;
        private static readonly Serilog.ILogger _logger = Log.ForContext<InvoiceService>();
        public InvoiceController(InvoiceService invoiceService , StoreService storeService)
        {
            _invoiceService = invoiceService;
            _storeService = storeService;
        }
       
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InvoiceDto invoice)
            {
          
            if (!ModelState.IsValid)
            {
                _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Warning,
                      "Invalid model state for invoice creation");
                return View("Index", invoice);
            }

            try
            {
                bool result = await _invoiceService.PostInvoiceAsync(invoice);

                if (result)
                {
                    _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Information,
                "Invoice created successfully");
                    return Json(new { success = true, message = "Invoice created successfully." });
                }
            }
            catch{
                _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Error,
                  "Invalid model state for invoice creation");
            }


                ModelState.AddModelError("", "Failed to submit invoice.");
                return View("Error", "Failed to submit invoice.");
            
            
            
        }
        public async Task<IActionResult> AlLInvoices()
        {
            try
            {
                var invoiceData = await _invoiceService.GetAllInvoiceAsync();
                if (invoiceData == null)
                {

                    // string Notfound = "NO Invoices Found";
                    _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Warning,
                        "No invoices found");
                    return View("AllInvoices", invoiceData);
                }
                
                return View("AllInvoices", invoiceData);
            }
            catch
            {
                _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Error,
                         "Error while fetching invoices");
                return View("AllInvoices","");
            }
           

        }


        public IActionResult Success()
        { 
            return View("Success");
        }

        [Route("Invoice/GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var specificInvoiceData = await _invoiceService.GetInvoiceByIdAsync(id);
                var storeData = await _storeService.GetAllStoreAsync();
                if (specificInvoiceData == null)
                {
                    _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Warning,
                                 $"Invoice with ID {id} not found");
                    return NotFound();
                }

                var viewModel = new StoreInvoiceViewModel
                {
                    Store = storeData,
                    Invoice = specificInvoiceData
                };
                return View("Details", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Error,
                $"Error retrieving invoice with ID {id}");
                return View("Error", "Error retrieving invoice");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody]InvoiceDto invoiceDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _invoiceService.UpdateInvoiceAsync(invoiceDto);
                    if (result)
                    {
                        _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Information,
                            "Invoice updated successfully");
                        return Json(new { Success = true });
                    }
                }
                catch 
                {
                    _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Error
                        , $"Error While Updating Invocie ID {invoiceDto.invoiceid}");
                }
            }
            _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Warning
                         , "Invalid model state for invoice update");

            return Json(new { Success = false });
        }
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _invoiceService.DeleteInvoiceAsync(id);
                if (result)
                {
                    _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Information,
                       $"Invoice with ID {id} deleted successfully");
                    ViewData["DeleteSuccessfully"] = "Invoice is deleted successfully";
                }
                else
                {
                    _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Warning,
                      $"Failed to delete invoice with ID {id}");
                    ViewData["DeleteFailed"] = "Failed to delete invoice";
                }
            }
            catch
            {
                _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Error,
                    $"Error deleting invoice with ID {id}");
            }
            return RedirectToAction("AlLInvoices");
        }


    }
}
