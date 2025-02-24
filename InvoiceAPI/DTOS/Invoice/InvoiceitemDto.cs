namespace InvoiceApi.DTOS.Invoice
{
    public class InvoiceitemDto
    {
        public int itemid { get; set; }
        public int unitid { get; set; }
        public decimal price { get; set; }
        public decimal qty { get; set; }
        public decimal total { get; set; }
        public decimal discount { get; set; }
        public decimal net { get; set; }



    }
}
