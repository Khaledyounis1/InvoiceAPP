using InvoiceApp.Service.DTOS.Store;
using InvoiceApp.Infrastructure.Models;

namespace InvoiceApp.Api.Services.StoreService
{

    public interface IStoreService
    {
        public StoreDto GetById(int id);
        public List<StoreDto> GetAllStores();
    }
}
 