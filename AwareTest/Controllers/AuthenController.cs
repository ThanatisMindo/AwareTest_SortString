using AwareTest.Model.Model;
using AwareTest.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwareTest.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AuthenController : ControllerBase
    {
        private readonly IJwtUtilsService _jwtUtilsService;
        public AuthenController(IJwtUtilsService jwtUtilsService)
        {
            _jwtUtilsService = jwtUtilsService;
        }

        [AllowAnonymous]
        [HttpPost("GenerateToken")]
        public IActionResult GenerateToken(AuthenticateRequestModel model)
        {
            //mock if user and password is test,test allow authen
            var userModel = new UserModel();
            if (model.Username == "Test" && model.Password == "Test")
            {
                userModel = new UserModel()
                {
                    Username = model.Username,
                    Password = model.Password
                };
            }
            var response = _jwtUtilsService.GenerateJwtToken(userModel);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
    }
}
