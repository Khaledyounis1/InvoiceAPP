using InvoiceAppData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InvoiceApp.Service.DTOS.Account;

namespace InvoiceApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto userfromrequest)
        {
            if (ModelState.IsValid)
            {

                var found = await userManager.FindByNameAsync(userfromrequest.Username);
                if (found == null)
                {
                    ApplicationUser user = new ApplicationUser();

                    user.UserName = userfromrequest.Username;
                    user.Email = userfromrequest.Email;

                    IdentityResult result =
                        await userManager.CreateAsync(user, userfromrequest.Password);

                    if (result.Succeeded)
                    {
                        return Ok("created");
                    }


                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("password", item.Description);
                    }
                    return BadRequest(ModelState);
                }
                else
                {
                    return Conflict("Already Exist");
                }
            }
            else
            {
                return BadRequest();
            }


        }


        [HttpPost("Login")]
            public async Task<IActionResult> Login(LoginDto userfromrequest)
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser userfromDb = await userManager.FindByNameAsync(userfromrequest.UserName);
                    if (userfromDb != null)
                    {
                        bool found = await userManager.CheckPasswordAsync(userfromDb, userfromrequest.Password);

                        if (found)
                        {
                            List<Claim> userclaims = new List<Claim>();

                            userclaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                            userclaims.Add(new Claim(ClaimTypes.NameIdentifier, userfromDb.Id));
                            userclaims.Add(new Claim(ClaimTypes.Name, userfromrequest.UserName));
                            var UserRoles = await userManager.GetRolesAsync(userfromDb);
                            foreach (var RoleName in UserRoles)
                            {
                                userclaims.Add(new Claim(ClaimTypes.Role, RoleName));
                            }

                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secritkey"]));
                            SigningCredentials signingCred = new SigningCredentials(key: key, SecurityAlgorithms.HmacSha256);
                            JwtSecurityToken Mytoken = new JwtSecurityToken(
                                 audience: configuration["JWT:AudienceIP"],
                                 issuer: configuration["JWT:IssuerIP"],
                                 expires: DateTime.Now.AddHours(1),
                                 claims: userclaims,
                                 signingCredentials: signingCred

                            );

                            return Ok(
                                new
                                {
                                    token = new JwtSecurityTokenHandler().WriteToken(Mytoken),
                                    Expiration = DateTime.Now.AddHours(1)
                                });

                        }
                    }
                }

                return BadRequest("Wrong User name or password");

            }
        
    }
}
