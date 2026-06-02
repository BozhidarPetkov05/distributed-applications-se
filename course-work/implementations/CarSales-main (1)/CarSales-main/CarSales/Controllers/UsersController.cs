using CarSales.Contracts.DTOs.Request.User;
using CarSales.Contracts.DTOs.Response.User;
using CarSales.Contracts.Interfaces;
using CarSales.Data.Entities;
using CarSales.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        } 

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] UserPageRequest request)
        {
            if (!User.HasClaim("isAdmin", "True"))
            {
                throw new ForbidException("You do not have access to this operation!");
            }

            var response = await _userService.GetAllUsersPagedAsync(request);

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var loggedUserId = User.FindFirstValue("loggedUserId");

            if (!User.HasClaim("isAdmin", "True") && Guid.Parse(loggedUserId) != id)
            {
                throw new ForbidException("You do not have access to this operation!");
            }

            User? user = await _userService.GetByIdAsync(id);
            if (user is null)
            {
                throw new NotFoundException("The user was not found!");
            }

            UserDetailedResponse response = _userService.MapToDetailedResponse(user);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserRequest model)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data");
            }

            if (await _userService.UsernameExists(model.Username))
            {
                throw new BadRequestException("User with this username already exists!");
            }

            User user = _userService.CreateUser(model);
            await _userService.AddAsync(user);
            return Created();
        }

        [Authorize]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put([FromBody] UserRequest model, [FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data");
            }

            var loggedUserId = User.FindFirstValue("loggedUserId");

            if (!User.HasClaim("isAdmin", "True") && Guid.Parse(loggedUserId) != id)
            {
                throw new ForbidException("You cannot update users other than you!");
            }

            User? user = await _userService.GetByIdAsync(id);
            if (user is null)
            {
                throw new NotFoundException("This user was not found!");
            }

            if (user.Username != model.Username && await _userService.UsernameExists(model.Username))
            {
                throw new BadRequestException("User with this username already exists!");
            }

            if (!User.HasClaim("isAdmin", "True"))
            {
                model.IsAdmin = false;
            }

            User updatedUser = _userService.UpdateUser(model, user);
            await _userService.UpdateAsync(updatedUser);

            UpdatedUserResponse response = _userService.MapToUpdatedUserResponse(updatedUser);

            return Ok(response);
        }

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var loggedUserId = User.FindFirstValue("loggedUserId");

            if (!User.HasClaim("isAdmin", "True") && Guid.Parse(loggedUserId) != id)
            {
                throw new ForbidException("You cannot delete users other than you!");
            }

            User? user = await _userService.GetByIdAsync(id);
            if (user is null)
            {
                throw new NotFoundException("This user was not found!");
            }

            UpdatedUserResponse deactivated = _userService.MapToUpdatedUserResponse(user);
            await _userService.DeleteAsync(user);
            return Ok(deactivated);
        }
    }
}
