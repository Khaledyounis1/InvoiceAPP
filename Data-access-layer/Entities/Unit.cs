using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceAppData.Models
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }


        //Edited
        [ForeignKey("store")]
        public int? Store_id { get; set; }

        public Store? store { get; set; }
    }
}
