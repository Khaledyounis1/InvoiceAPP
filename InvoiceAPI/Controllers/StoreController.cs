using InvoiceApp.Api.Services.StoreService;
using InvoiceApp.Service.DTOS.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService storeService;

        public StoreController(IStoreService storeService)
        {
            this.storeService = storeService;
        }

        [HttpGet("{id}")]
        //[Authorize]
        public IActionResult Get(int id)
        {
           StoreDto store =  storeService.GetById(id);
            if (store != null)
            {
                 return  Ok(store);
            }

            else
            {
                return NotFound($" Store with ID {id} is Not found");
            }
        }

        [HttpGet]
        //[Authorize]
        public IActionResult GetAll()
        {
            var stores = storeService.GetAllStores();
            if (stores == null || stores.Count == 0)
            {
                return NotFound("No stores found.");
            }
            return Ok(stores);
        }
    }
}
