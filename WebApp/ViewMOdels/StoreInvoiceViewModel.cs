using WebApp.DTOS;

namespace WebApp.ViewMOdels
{
    public class StoreInvoiceViewModel
    {
        public List<StoreDto> Store { get; set; } = new List<StoreDto>();
        public InvoiceDto Invoice { get; set; } = new InvoiceDto();
    }
}
