using InvoiceApi.DTOS.Invoice;
using InvoiceApi.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace InvoiceAPI.Services.InvoiceService
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
