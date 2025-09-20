using dentist_panel_api.DTOs;
using dentist_panel_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace dentist_panel_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserServices userServices;

        public UsersController(UserServices userServices)
        {
            this.userServices = userServices;
        }

        [HttpPost("auth/sign-in")]
        public async Task<ActionResult<AuthResponse>> SignIn([FromBody] SignInDTO signInDTO)
        {
            return await userServices.SignIn(signInDTO);
        }

        [HttpPost("auth/sign-up")]
        public async Task<ActionResult<AuthResponse>> SignUp([FromBody] SignUpDTO signUpDTO)
        {
            return await userServices.SignUp(signUpDTO);
        }
    }
}
