using   WebApp.DTOS;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;


namespace WebApp.Servicies
{
    public class InvoiceService
    {
        private readonly HttpClient _httpClient;

        public InvoiceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> PostInvoiceAsync(InvoiceDto invoice)
        {
            var json = JsonConvert.SerializeObject(invoice);
      
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5296/api/Invoice", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<InvoiceDto>> GetAllInvoiceAsync() 
        {
            var response = await _httpClient.GetAsync($"http://localhost:5296/api/Invoice");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<InvoiceDto>>(jsonData);
            }
            return null;
        }

        public async Task<InvoiceDto> GetInvoiceByIdAsync(int id )
        {
            var response = await _httpClient.GetAsync($"http://localhost:5296/api/Invoice/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<InvoiceDto>(jsonData);
            }
            return null;
        }


        public async Task<bool> UpdateInvoiceAsync(InvoiceDto invoice)
        {
            var json = JsonConvert.SerializeObject(invoice);
            var content = new StringContent(json,Encoding.UTF8,"application/json");

            var response = await _httpClient.PutAsync("http://localhost:5296/api/Invoice", content);

            return response.IsSuccessStatusCode;
        
        }
        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            
            var response = await _httpClient.DeleteAsync($"http://localhost:5296/api/Invoice/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else { return false; }

        }
    }
}
