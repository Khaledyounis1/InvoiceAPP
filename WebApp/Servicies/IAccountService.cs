using WebApp.DTOS;

namespace WebApp.Servicies
{
    public interface IAccountService
    {
        public  Task<bool> UserLoginAsync(LoginDto userfromrequest);
        public Task<int> UserRegisterAsync(RegisterDto userfromrequest);
    }
}
