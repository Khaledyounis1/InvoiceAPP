using InvoiceApp.Service.DTOS.Invoice;
using InvoiceApp.Infrastructure.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace InvoiceApp.Api.Services.InvoiceService
{
    public interface IinvoiceService
    {
         string addInvoice(InvoiceDto invoicefromrequest);
         InvoiceDto GetById(int id);

         bool Update(InvoiceDto invoicefromrequest);
         List<InvoiceDto> GetAll();
         bool Delete(int id);

    }
}
