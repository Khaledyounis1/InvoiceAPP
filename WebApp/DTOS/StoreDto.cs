
namespace WebApp.DTOS
{
    public class StoreDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<StoreitemsDto> storeitems {  get; set; } = new List<StoreitemsDto>();
        public List<StoreUnitsDto> storeunits { get; set; } = new List<StoreUnitsDto>();

        public static implicit operator List<object>(StoreDto v)
        {
            throw new NotImplementedException();
        }
    }
}
