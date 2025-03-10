using System.Text.Json.Serialization;

namespace InvoiceApp.Service.DTOS.Invoice
{
    public class InvoiceDto
    {

        public int invoiceid { get; set; } = 0;
        public int InvoiceNO { get; set; }
        public string? InvoiceDate { get; set; }
        public decimal nettotal { get; set; }
        public decimal taxes { get; set; }
        public decimal NetAftertaxes { get; set; }
        public string? storeName { get;  set; }
        public int storeid { get; set; }

        public List<InvoiceitemDto>? invoiceitems { get; set; } = new List<InvoiceitemDto>();
    }
}
