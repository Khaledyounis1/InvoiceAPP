using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data_access_layer.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal  Quantity { get; set; }

        public decimal Totalprice { get; set; }
        public decimal  Discount { get; set; }
        public decimal  Net { get; set; }

        [ForeignKey("invoice")]
        public int  InvoiceId { get; set; }
        [ForeignKey("item")]
        public int ItemId { get; set; }
        [ForeignKey("unit")]
        public int UnitId { get; set; }

        //Navigation property 

        [JsonIgnore]
        public  Invoice? invoice { get; set; }
        [JsonIgnore]
        public Item? item { get; set; }
        [JsonIgnore]
        public Unit? unit { get; set; }
    }
}
