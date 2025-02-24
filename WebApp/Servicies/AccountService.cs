using Newtonsoft.Json;
using System.Text;
using WebApp.Servicies;
using WebApp.DTOS;

namespace WebApp.Servicies
{
    public class AccountService :IAccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<bool> UserLoginAsync(LoginDto userfromrequest)
        {
            var json = JsonConvert.SerializeObject(userfromrequest);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5296/api/Account/Login", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<int> UserRegisterAsync(RegisterDto userfromrequest)
        {
         

                var json = JsonConvert.SerializeObject(userfromrequest);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("http://localhost:5296/api/Account/Register", content);
                return (int) response.StatusCode;
         

        }
     
    }
}
