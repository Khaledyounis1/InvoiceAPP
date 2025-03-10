using Microsoft.AspNetCore.Http.Features;

namespace InvoiceAppData.Models
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Item> items { get; set; } = new List<Item>();
        public List<Unit> units { get; set; } = new List<Unit>();




    }
}
