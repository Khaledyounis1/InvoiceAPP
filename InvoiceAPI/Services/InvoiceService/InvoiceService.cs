using InvoiceAPI.GenericRepositories;
using InvoiceAPI.Logs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Identity.Client;
using Serilog;
using Serilog.Core;
using System.Diagnostics.Eventing.Reader;
using InvoiceApi.DTOS.Invoice;
using InvoiceApi.Models;
using Data_access_layer.Models;


namespace InvoiceAPI.Services.InvoiceService
{
    public class InvoiceService : IinvoiceService
    {
        private readonly IGenericRepository<Invoice> invoiceRepo;

        //private readonly IinvoiceRepository invoiceRepo;
       
        private static readonly Serilog.ILogger _logger = Log.ForContext<InvoiceService>();


        public InvoiceService( IGenericRepository<Invoice> invoiceRepo)//,ILogger<IinvoiceService>logger)
        {
            this.invoiceRepo = invoiceRepo;
            //_logger = logger;
        }
        public string addInvoice(InvoiceDto invoicefromrequest)
        {
            try
            {
                Invoice invoice = new Invoice();

                invoice.Invoicenumber = invoicefromrequest.InvoiceNO;
                invoice.Date = DateTime.Parse(invoicefromrequest.InvoiceDate);
                invoice.Total = invoicefromrequest.nettotal;
                invoice.Taxes = invoicefromrequest.taxes;
                invoice.Net = invoicefromrequest.NetAftertaxes;
                invoice.storid = invoicefromrequest.storeid;
                foreach (var item in invoicefromrequest.invoiceitems)
                {
                    InvoiceItem invoiceitem = new InvoiceItem();
                    invoiceitem.ItemId = item.itemid;
                    invoiceitem.UnitId = item.unitid;
                    invoiceitem.Price = item.price;
                    invoiceitem.Quantity = item.qty;
                    invoiceitem.Totalprice = item.total;
                    invoiceitem.Discount = item.discount;
                    invoiceitem.Net = item.net;

                    invoice.Invoiceitems.Add(invoiceitem);
                }

                invoiceRepo.Add(invoice);
                invoiceRepo.Save();

                return "Success";

            }
            catch 
            {

                _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Error, $"Error occurred while adding an invoice with InvoiceNO: {invoicefromrequest.InvoiceNO}");
                return "Failed";
            }

        }


        public List<InvoiceDto> GetAll()
        {
            try
            {

                var invoices = invoiceRepo.GetAll().Include(i => i.store).ToList();



                var invoiceDtos =invoices.Select(i => new InvoiceDto
                {
                    invoiceid = i.Id,
                    InvoiceNO = i.Invoicenumber,
                    InvoiceDate = i.Date.ToString("yyyy-MM-dd"),
                    nettotal = i.Total,
                    taxes = i.Taxes,
                    NetAftertaxes = i.Net,
                    storeName = i.store !=null ? i.store.Name: "N/A"
               }).ToList();

                return invoiceDtos;
            }
            catch 
            {
                _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Error, "Error Occured when Get all Invoices !");
                return null;
            }
          


        }

        public InvoiceDto GetById(int id)
        {
            try
            {
                Invoice invoice = invoiceRepo.GetAll().Include(i=>i.Invoiceitems).FirstOrDefault(i=>i.Id==id);
                if (invoice == null)
                {
                    _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Error, $"Invoice with ID {id} not found");
                    return null;
                }
                InvoiceDto invoiceDto = new InvoiceDto();
                invoiceDto.invoiceid = invoice.Id;
                invoiceDto.InvoiceNO = invoice.Invoicenumber;
                invoiceDto.InvoiceDate = invoice.Date.ToString("yyyy-MM-dd");
                invoiceDto.nettotal = invoice.Total;
                invoiceDto.taxes = invoice.Taxes;
                invoiceDto.NetAftertaxes = invoice.Net;
                invoiceDto.storeid = invoice.storid;
                foreach (var item in invoice.Invoiceitems)
                {
                    InvoiceitemDto invoiceitem = new InvoiceitemDto();

                    invoiceitem.itemid = item.ItemId;
                    invoiceitem.unitid = item.UnitId;
                    invoiceitem.price = item.Price;
                    invoiceitem.qty = item.Quantity;
                    invoiceitem.total = item.Totalprice;
                    invoiceitem.discount = item.Discount;
                    invoiceitem.net = item.Net;

                    invoiceDto.invoiceitems.Add(invoiceitem);
                }

                return invoiceDto;
            }
            catch 
            {

                _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Error, $"Error occurred while retrieving invoice with ID {id}.");

                return null;

            }

        }


        public bool Update(InvoiceDto invoicefromrequest)
        {
            try
            {
                var invoice = invoiceRepo.GetAll().Include(i => i.Invoiceitems).FirstOrDefault(i => i.Id ==invoicefromrequest.invoiceid);
                if (invoice == null)
                {
                    _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Error, $"Invoice with ID {invoicefromrequest.invoiceid} not found for update.");
                    return false;
                }

                //my Logic
                if (invoice.Date != DateTime.Parse(invoicefromrequest.InvoiceDate))
                {
                    invoice.UpdateON = DateTime.Parse(invoicefromrequest.InvoiceDate);
                }
                else
                {
                    invoice.Date = DateTime.Parse(invoicefromrequest.InvoiceDate);
                }
                invoice.Invoicenumber = invoicefromrequest.InvoiceNO;
                invoice.Total = invoicefromrequest.nettotal;
                invoice.Taxes = invoicefromrequest.taxes;
                invoice.Net = invoicefromrequest.NetAftertaxes;
                invoice.storid = invoicefromrequest.storeid;

                foreach (var itemDto in invoicefromrequest.invoiceitems)
                {
                    var existingItem = invoice.Invoiceitems
                        .FirstOrDefault(i => i.ItemId == itemDto.itemid && i.UnitId == itemDto.unitid);

                    if (existingItem != null)
                    {
                        // Update existing InvoiceItem
                        existingItem.Price = itemDto.price;
                        existingItem.Quantity = itemDto.qty;
                        existingItem.Totalprice = itemDto.total;
                        existingItem.Discount = itemDto.discount;
                        existingItem.Net = itemDto.net;
                    }
                    else
                    {
                        // Add new InvoiceItem
                        InvoiceItem newItem = new InvoiceItem
                        {
                            InvoiceId = invoice.Id,
                            ItemId = itemDto.itemid,
                            UnitId = itemDto.unitid,
                            Price = itemDto.price,
                            Quantity = itemDto.qty,
                            Totalprice = itemDto.total,
                            Discount = itemDto.discount,
                            Net = itemDto.net
                        };
                        invoice.Invoiceitems.Add(newItem);
                    }
                }
                invoiceRepo.Save();
                return true;
            }
            catch 
            {
                _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Error, $"Error occurred while updating invoice with ID {invoicefromrequest.invoiceid}.");
                return false;
            }
        }





        public bool Delete(int id)
        {
            try
            {
                invoiceRepo.Delete(id);
                invoiceRepo.Save();
                return true;
            }
            catch 
            {
                _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Error, $"Error occurred while deleting invoice with ID {id}.");
                return false;
            }
        }




    }
}
