using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Organiser.Cores.Class;
//using System.Web.Http;
using Organiser.Cores.Entities;

namespace Organiser.Cores.Controllers
{
    public class AccountController : ControllerBase //ApiController
    {
        private readonly UserManager<Users> userManager;
        private readonly SignInManager<Users> signInMenager;

        public AccountController(UserManager<Users> userManager, 
                                 SignInManager<Users> signInMenager) 
        { 
            this.userManager = userManager;
            this.signInMenager = signInMenager;
        }
        [HttpGet]

        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDTO)
        {
            var newUser = new Users()
            {
                Email = userRegisterDTO.Email,
                UserName = userRegisterDTO.Email
            };

            var result = await userManager.CreateAsync(newUser, userRegisterDTO.Password);
            
            if (result.Succeeded)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
                await userManager.ConfirmEmailAsync(newUser, token);
                return Ok();
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var user = await userManager.FindByEmailAsync(userLoginDTO.Email);

            if (user == null)
            {
                return NotFound();
            }
            var result = signInMenager.PasswordSignInAsync(user, userLoginDTO.Password, true, false);
            if (result.IsCompletedSuccessfully)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
