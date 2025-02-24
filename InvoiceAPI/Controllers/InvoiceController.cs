using InvoiceApi.DTOS.Invoice;
using InvoiceApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InvoiceAPI.Services.InvoiceService;

namespace InvoiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class InvoiceController : ControllerBase
    {
        private readonly IinvoiceService invoiceser;

        public InvoiceController(IinvoiceService invoiceser)
        {
            this.invoiceser = invoiceser;
        }


        [HttpPost]
        
        public IActionResult Add([FromBody]InvoiceDto invoicefromrequest)
          {

            string flag  = invoiceser.addInvoice(invoicefromrequest);
            if (flag == "Success")
            {
                return Ok();
            }
            else { 
               
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var invoice = invoiceser.GetById(id);
            if (invoice != null)
            {   
                return Ok(invoice);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<InvoiceDto> allinvoices = invoiceser.GetAll();
            if (allinvoices.Count != 0)
            {
                return Ok(allinvoices);
            }
            return NotFound("NO Invoices Found");
        }
        [HttpPut]
        public IActionResult UpdateInvoice([FromBody] InvoiceDto invoiceDto)
        {
            if (invoiceDto == null )
            {
                return BadRequest();
            }

            var result = invoiceser.Update(invoiceDto);

            if (result)
            {
                return NoContent(); // 204 No Content
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteInvoice(int id)
        {
            var Isdeleted = invoiceser.Delete(id);

         
            if (Isdeleted)
            {
                return NoContent();
            }
            else {
                return NotFound();
            }

            
        }

    }
}
