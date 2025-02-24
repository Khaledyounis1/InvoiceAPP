using InvoiceApi.DTOS.Store;
using InvoiceApi.Models;

namespace InvoiceAPI.Services.StoreService
{
    public interface IStoreService
    {
        public StoreDto GetById(int id);
        public List<StoreDto> GetAllStores();
    }
}
