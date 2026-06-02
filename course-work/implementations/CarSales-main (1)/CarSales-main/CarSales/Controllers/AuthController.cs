using CarSales.Contracts.DTOs.Request;
using CarSales.Contracts.Interfaces;
using CarSales.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CarSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromForm] AuthTokenRequest model)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("One or more validation errors occurred!");
            }

            var user = (await _userService.GetAllAsync()).FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
            if (user is null)
            {
                throw new BadRequestException("Username or password is invalid!");
            }

            var token = _tokenService.CreateToken(user);
            return Ok(new {token});
        }   
    }
}
