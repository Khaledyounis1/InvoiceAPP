using InvoiceAPI.GenericRepositories;
using InvoiceAPI.Logs;

using Microsoft.EntityFrameworkCore;
using Serilog;
using InvoiceApi.DTOS.Store;
using InvoiceApi.Models;
using Data_access_layer.Models;

namespace InvoiceAPI.Services.StoreService
{
    public class StoreService : IStoreService
    {
        //private readonly IStoreRepository storeRepo;
        private static readonly Serilog.ILogger _logger = Log.ForContext<StoreService>();
        private readonly IGenericRepository<Store> storeRepo;

        public StoreService(IGenericRepository<Store> storeRepo)//IStoreRepository storeRepo,
        {
            this.storeRepo = storeRepo;
            //this.storeRepo = storeRepo;
        }
        public StoreDto GetById(int id)
        {

            try
            {
                Store store = storeRepo.GetAll().Include(s=>s.items).Include(s => s.units).FirstOrDefault(s=>s.Id==id);

                StoreDto storedto = new StoreDto();

                storedto.Id = store.Id;
                storedto.Name = store.Name;

                foreach (var item in store.items)
                {
                    StoreitemsDto storeitems = new StoreitemsDto();
                    storeitems.itemid = item.Id;
                    storeitems.itemname = item.Name;
                    storedto.storeitems.Add(storeitems);
                }
                foreach (var item in store.units)
                {
                    StoreUnitsDto storeunit = new StoreUnitsDto();
                    storeunit.unitid = item.Id;
                    storeunit.unitname = item.Name;
                    storedto.storeunits.Add(storeunit);
                }
                return storedto;
            }
            catch 
            {
                _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Error, 
                    $"Error occurred while retrieving Store with ID {id}.");
                return null;
            }




        }


        public List<StoreDto> GetAllStores()
        {
            try
            {
                var stores = storeRepo.GetAll().Include(s=>s.items).Include(s=>s.units).ToList();

                List<StoreDto> storeDtos = new List<StoreDto>();

                foreach (var store in stores)
                {
                    StoreDto storeDto = new StoreDto
                    {
                        Id = store.Id,
                        Name = store.Name
                    };

                    foreach (var item in store.items)
                    {
                        StoreitemsDto storeItem = new StoreitemsDto
                        {
                            itemid = item.Id,
                            itemname = item.Name
                        };
                        storeDto.storeitems.Add(storeItem);
                    }

                    foreach (var unit in store.units)
                    {
                        StoreUnitsDto storeUnit = new StoreUnitsDto
                        {
                            unitid = unit.Id,
                            unitname = unit.Name
                        };
                        storeDto.storeunits.Add(storeUnit);
                    }

                    storeDtos.Add(storeDto);
                }

                return storeDtos;
            }
            catch
            {
                _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Error,
                    $"Error occurred while retrieving All Stores.");
                return null;
            }
        }

    }
}
