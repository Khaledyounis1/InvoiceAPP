using Microsoft.AspNetCore.Mvc;
using WebApp.Servicies;
using WebApp.DTOS;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using InvoiceAPI.Logs;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {

        private readonly AccountService _accountService;
        //private readonly ILogger logger;
        private static readonly Serilog.ILogger _logger = Log.ForContext<InvoiceService>();
   
        public AccountController(AccountService accountService)//, ILogger<AccountController> logger)
        {
            _accountService = accountService;
           // this.logger = logger;
        }
        public IActionResult Loginpage()
        {
            return View("Loginpage");
        }
       
        [HttpPost]
        public async Task< IActionResult> LoginUser(string username, string password)
            {
                var userfromrequest = new LoginDto
                {
                    UserName = username,
                    Password = password
                };
            if (ModelState.IsValid)
            {
                var result = await _accountService.UserLoginAsync(userfromrequest);
                if (result)
                {
                    return RedirectToAction("AllInvoices", "Invoice");
                }
                else
                {
                    _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Warning,
                        $"Failed login attempt for username: {userfromrequest.UserName}");
                    ViewData["Errormessage"] = "Invalid Username or Password";
                    return View("Loginpage", userfromrequest);
                }
            }
            _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Error,
                    $"Invalid login request received for username: { userfromrequest.UserName}");
            return View("Loginpage", userfromrequest);
        }


        public IActionResult Registerpage()
        {
            return View("Registerpage");
        }


        [HttpPost]
        public async Task<IActionResult> RegisterUser(string username,string Email, string password)
        {
            var userfromrequest = new RegisterDto
            {
                Username = username,
                Email = Email,
                Password = password
            };
            
            if (ModelState.IsValid)
            {
                var result = await _accountService.UserRegisterAsync(userfromrequest);
                if (result == 200)
                {
                    _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Information,
                $"User \"{userfromrequest.Username}\" registered successfully.");
                    return RedirectToAction("Loginpage", "Account");
                }
                else if (result == 409) 
                {
                    _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Warning,
                    $"Attempt to register with an existing username: {userfromrequest.Username}");
                    ViewData["Errormessage"] = "UserName Already Exist !";
                    return View("Registerpage", userfromrequest);
                
                }
                else
                {
                    _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Warning,
                          $"Password validation failed for user: { userfromrequest.Username}");
                    ViewData["PassErrormessage"] = "Password must contain 8 character at least one uppercase letter, one number, and one special character !";
                    return View("Registerpage", userfromrequest);
                }
            }

            _logger.LogWithMethodName((Serilog.Events.LogEventLevel)Microsoft.Extensions.Logging.LogLevel.Error,
                   $"Invalid Register request received for username: {userfromrequest.Username}");

            return View("Registerpage", userfromrequest);
        } 
    }
}
