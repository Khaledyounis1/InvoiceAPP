using WebApp.DTOS;
using Newtonsoft.Json;

namespace WebApp.Servicies
{
    public class StoreService
    {
        private readonly HttpClient _httpClient;

        public StoreService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<StoreDto>> GetAllStoreAsync()
        {
            var response = await _httpClient.GetAsync($"http://localhost:5296/api/Store");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<StoreDto>>(jsonData);
            }
            return null;
        }

        internal async Task<StoreDto> GetStoreByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5296/api/Store/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<StoreDto>(jsonData);
            }
            return null;
        }
    }
}
