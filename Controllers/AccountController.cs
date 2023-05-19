using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingAPI.Model.DTO.Account;
using System.Threading.Tasks;

namespace OnlineShoppingAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public UserManager<IdentityUser> _userManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }
        public SignInManager<IdentityUser> _signInManager { get; }

        public AccountController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegisterDTO register, string roleName)
        {
            IdentityUser newUser = new IdentityUser();
            newUser.UserName = register.Name;
            newUser.Email = register.Email;

            IdentityResult result = await _userManager.CreateAsync(newUser, register.Password);
            if (result.Succeeded)
            {
                var roleAddResult = await _userManager.AddToRoleAsync(newUser, roleName);
                await _signInManager.SignInAsync(newUser, true);
                return StatusCode(StatusCodes.Status201Created, newUser.UserName);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, newUser.UserName);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleUIDTO sendedRole)
        {
            IdentityRole newRole = new IdentityRole
            {
                Name = sendedRole.Name
            };
            var result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                return StatusCode(StatusCodes.Status201Created, newRole.Name);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, newRole.Name);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUIDTO login)
        {
            var existUser = await _userManager.FindByEmailAsync(login.Email);
            if (existUser is null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            var result = await _signInManager.PasswordSignInAsync(existUser, login.Password, true, false);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest();
        }


        [HttpPost]
        public async Task<IActionResult> AddRoleToUser(AddRoleUserUIDTO roleUserUIDTO)
        {
            var user = await _userManager.FindByEmailAsync(roleUserUIDTO.Email);
            await _userManager.AddToRoleAsync(user, roleUserUIDTO.RoleName);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }



 
}
