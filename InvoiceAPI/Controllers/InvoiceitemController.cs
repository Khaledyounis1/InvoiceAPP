using InvoiceApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InvoiceAPI.Services.InvocieitemService;
using Data_access_layer.Models;

namespace InvoiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceitemController : ControllerBase
    {
        private readonly IinvoiceitemService invoiceitemser;

        public InvoiceitemController(IinvoiceitemService invoiceitemser)
        {
            this.invoiceitemser = invoiceitemser;
        }
        [HttpPut]
        public IActionResult Update(InvoiceItem invoiceItem)
        { 
           string Isupdated = invoiceitemser.update(invoiceItem);


            if (Isupdated == "Yes")
            {
                return NoContent();
            }
            else
            {
                return NotFound("Not found to update");
            }
        }
    }
}
