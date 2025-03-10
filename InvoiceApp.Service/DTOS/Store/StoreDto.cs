namespace InvoiceApp.Service.DTOS.Store
{
    public class StoreDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<StoreitemsDto> storeitems { get; set; } = new List<StoreitemsDto>();
        public List<StoreUnitsDto> storeunits { get; set; } = new List<StoreUnitsDto>();

    }
}
