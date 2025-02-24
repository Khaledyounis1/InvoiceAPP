using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data_access_layer.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int Invoicenumber { get; set; }
        public DateTime Date { get; set; }

        [JsonIgnore]
        public DateTime? UpdateON { get; set; }

        public decimal Total { get; set; }
        public decimal Taxes  { get; set; }
        public decimal Net { get; set; }

        [ForeignKey("store")]
        public int storid { get; set;}
        [JsonIgnore]
        public Store? store { get; set; }
        [JsonIgnore]
        public List<InvoiceItem> Invoiceitems { get; set; } = new List<InvoiceItem>();
    }
}
