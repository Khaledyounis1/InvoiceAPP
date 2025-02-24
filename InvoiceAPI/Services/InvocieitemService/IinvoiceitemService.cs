using Data_access_layer.Models;
using InvoiceApi.Models;

namespace InvoiceAPI.Services.InvocieitemService
{
    public interface IinvoiceitemService
    {
        public string update(InvoiceItem invoiceItem);
    }
}
